//#define GRAPH_RUMBLE

using System;
using System.Collections.Generic;
using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Haptics;
#endif // ENABLE_INPUT_SYSTEM

#if GRAPH_RUMBLE
using RoyTheunissen.Graphing;
#endif // GRAPH_RUMBLE

namespace RoyTheunissen.SpatialRumble.Rumbling
{
    /// <summary>
    /// Responsible for managing the playback of rumble effects and passing them on to the hardware.
    /// 
    /// Default implementation as a pure C# object, so you can instantiate it and register it yourself without relying
    /// on a MonoBehaviour.
    /// </summary>
    public class RumbleService : IRumbleService
    {
        public const float SpatialRadiusRecommendedDefault = 15;
        
        public const float RumbleGracePeriod = 3;

        private readonly AnimationCurve rumbleRollOff;
        
        private float spatialRadiusDefault = SpatialRadiusRecommendedDefault;
        public float SpatialRadiusDefault
        {
            get => spatialRadiusDefault;
            set => spatialRadiusDefault = value;
        }

        private List<RumbleListener> listeners = new();
        private bool HasListener => listeners.Count > 0;
        private RumbleListener Listener => HasListener ? listeners[listeners.Count - 1] : null;
        
        private List<IRumble> rumbles = new();
        
        private List<RumblePlayback> playbacks = new();
        
        private RumbleProperties rumblePropertiesCombined;
        public RumbleProperties RumbleProperties => rumblePropertiesCombined;

        private RumbleProperties rumblePropertiesIndividual;
        
        private List<object> pausers = new();

        private float startTime;

        private bool IsPaused =>
            pausers.Count > 0 || Time.time < startTime + RumbleGracePeriod || Mathf.Approximately(Time.timeScale, 0.0f);
        
        private bool enableRumble = true;
        public bool EnableRumble
        {
            get => enableRumble;
            set => enableRumble = value;
        }
        
        private bool isCleanedUp;
        
        [NonSerialized] private static IRumbleService cachedInstance;
        /// <summary>
        /// The currently active IRumbleService instance that all rumble playback should report to.
        /// </summary>
        public static IRumbleService Instance
        {
            get => cachedInstance;
            set
            {
                if (cachedInstance != null && value != null)
                {
                    Debug.LogWarning($"You are overriding the Rumble Service from an instance of " +
                                     $"'{cachedInstance.GetType()}' to an instance of '{value.GetType()}'. " +
                                     $"Is this intended?");
                }
                
                cachedInstance = value;
            }
        }

        public RumbleService(AnimationCurve rumbleRollOff, float spatialRadiusDefault = SpatialRadiusRecommendedDefault)
        {
            this.rumbleRollOff = rumbleRollOff;
            this.spatialRadiusDefault = spatialRadiusDefault;
            
            startTime = Time.time;
            
#if UNITY_EDITOR
            UnityEditor.EditorApplication.pauseStateChanged += HandleEditorPauseStateChangedEvent;
#endif // UNITY_EDITOR

#if GRAPH_RUMBLE
            Graph.CreateGlobal("Low Frequency Rumble", Color.red, () => rumblePropertiesCombined.LowFrequencyRumble)
                .AddLine("High Frequency Rumble", Color.green, GraphLine.Modes.ContinuousLine, () => rumblePropertiesCombined.HighFrequencyRumble);
#endif // GRAPH_RUMBLE
        }

        /// <summary>
        /// Cleans up after the rumble service. If you don't call this, the rumble values will not be cleared and rumble
        /// will continue, even if you are exiting play mode in the editor.
        /// </summary>
        public void Cleanup()
        {
            if (isCleanedUp)
                return;

            isCleanedUp = true;
            
#if UNITY_EDITOR
            UnityEditor.EditorApplication.pauseStateChanged -= HandleEditorPauseStateChangedEvent;
#endif // UNITY_EDITOR

            ResetAllHaptics();
        }
        
        public void Update()
        {
            CullFinishedRumblePlaybacks();
            ComputeRumbleProperties();
            PassRumbleOnToHardware(rumblePropertiesCombined);
        }

#if UNITY_EDITOR
        private void HandleEditorPauseStateChangedEvent(UnityEditor.PauseState pauseState)
        {
            // Reset all haptics when we pause/unpause to prevent any ongoing rumbles from "sticking around" while you
            // are pausing the editor to check something, because that's really annoying.
            ResetAllHaptics();
        }
#endif // UNITY_EDITOR
        
        private static void ResetAllHaptics()
        {
            Gamepad currentGamepad = Gamepad.current;
            if (currentGamepad == null)
                return;

            IDualMotorRumble motors = currentGamepad;
            if (motors == null)
                return;
            
            // Reset haptics otherwise whatever rumble we passed along lasts will stick around. Super annoying while
            // working in the editor...
            motors.ResetHaptics();
        }

        public void Pause(object owner)
        {
            if (pausers.Contains(owner))
                return;
            
            pausers.Add(owner);
        }

        public void Resume(object owner)
        {
            pausers.Remove(owner);
        }

        public void RegisterListener(RumbleListener listener)
        {
            if (listeners.Contains(listener))
                return;
            
            listeners.Add(listener);
            
            if (listeners.Count > 1)
                Debug.LogWarning("Hey there's more than one Rumble Listener. That doesn't sound right.");
        }

        public void UnregisterListener(RumbleListener listener)
        {
            listeners.Remove(listener);
        }

        private void CullFinishedRumblePlaybacks()
        {
            for (int i = playbacks.Count - 1; i >= 0; i--)
            {
                if (!playbacks[i].IsFinished)
                    continue;
                
                rumbles.Remove(playbacks[i]);
                playbacks.RemoveAt(i);
            }
        }

        private void ComputeRumbleProperties()
        {
            rumblePropertiesCombined.Reset();
            for (int i = 0; i < rumbles.Count; i++)
            {
                rumblePropertiesIndividual.Reset();
                rumbles[i].GetRumble(ref rumblePropertiesIndividual);
                IntegrateRumble(rumblePropertiesIndividual, rumbles[i]);
            }
        }

        private void IntegrateRumble(RumbleProperties rumbleProperties, IRumble rumble)
        {
            float opacity = rumble.Opacity;

            // If specified, modify the opacity based on the position relative to the rumble listener.
            bool isSpatial = rumble.SpatialBlend > 0.0f;
            if (isSpatial && HasListener)
            {
                float distanceToListener = Vector3.Distance(Listener.Position, rumble.SpatialOrigin);
                float distanceNormalized = Mathf.Clamp01(distanceToListener / rumble.SpatialRadius);
                float attenuation = rumbleRollOff.Evaluate(distanceNormalized);
                opacity = Mathf.Lerp(opacity, opacity * attenuation, rumble.SpatialBlend);
            }
            
            // Integrate the rumble into the combined rumble properties in an additive or override way with opacity.
            switch (rumble.BlendMode)
            {
                case RumbleBlendModes.Additive:
                    rumblePropertiesCombined.LowFrequencyRumble += rumbleProperties.LowFrequencyRumble * opacity;
                    rumblePropertiesCombined.HighFrequencyRumble += rumbleProperties.HighFrequencyRumble * opacity;
                    break;
                case RumbleBlendModes.Override:
                    rumblePropertiesCombined.LowFrequencyRumble = Mathf.Lerp(
                        rumblePropertiesCombined.LowFrequencyRumble, rumbleProperties.LowFrequencyRumble, opacity);
                    rumblePropertiesCombined.HighFrequencyRumble = Mathf.Lerp(
                        rumblePropertiesCombined.HighFrequencyRumble, rumbleProperties.HighFrequencyRumble, opacity);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void PassRumbleOnToHardware(RumbleProperties rumbleProperties)
        {
#if ENABLE_INPUT_SYSTEM
            Gamepad currentGamepad = Gamepad.current;
            if (currentGamepad == null)
                return;

            IDualMotorRumble motors = currentGamepad;
            if (motors == null)
                return;

            if (IsPaused || !EnableRumble)
            {
                motors.ResetHaptics();
                motors.PauseHaptics();
                return;
            }

            motors.ResumeHaptics();
            motors.SetMotorSpeeds(rumbleProperties.LowFrequencyRumble, rumbleProperties.HighFrequencyRumble);
#endif // ENABLE_INPUT_SYSTEM
        }

        public void AddRumble(IRumble rumble)
        {
            if (rumbles.Contains(rumble))
                return;
            
            rumbles.Add(rumble);
        }
        
        public void RemoveRumble(IRumble rumble)
        {
            rumbles.Remove(rumble);
        }

        public PlaybackType Play<PlaybackType>(RumbleConfigBase config, Transform origin, float opacity)
            where PlaybackType : RumblePlayback, new()
        {
            PlaybackType playback = new();
            playback.Initialize(config, origin, opacity);
            
            playbacks.Add(playback);
            
            AddRumble(playback);
            
            playback.Start();

            return playback;
        }
    }
}
