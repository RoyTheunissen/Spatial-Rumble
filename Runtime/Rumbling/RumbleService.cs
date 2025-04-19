//#define GRAPH_RUMBLE

using System;
using System.Collections.Generic;
using RoyTheunissen.UnityHaptics.Curves;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Haptics;

#if GRAPH_RUMBLE
using RoyTheunissen.Graphing;
#endif // GRAPH_RUMBLE

namespace RoyTheunissen.UnityHaptics.Rumbling
{
    /// <summary>
    /// Responsible for managing the playback of rumble effects and passing them on to the hardware.
    /// </summary>
    public sealed class RumbleService : MonoBehaviour
    {
        public const float SpatialRadiusDefault = 15;
        
        public const float RumbleGracePeriod = 3;

        [SerializeField] private CurveReference rumbleRollOff;
        
        private List<RumbleListener> listeners = new List<RumbleListener>();
        private bool HasListener => listeners.Count > 0;
        private RumbleListener Listener => HasListener ? listeners[listeners.Count - 1] : null;
        
        private List<IRumble> rumbles = new List<IRumble>();
        
        private List<RumblePlayback> playbacks = new List<RumblePlayback>();
        
        private RumbleProperties rumblePropertiesCombined = new RumbleProperties();
        private RumbleProperties rumblePropertiesIndividual = new RumbleProperties();
        
        private List<object> pausers = new List<object>();

        private float startTime;

        private bool IsPaused =>
            pausers.Count > 0 || Time.time < startTime + RumbleGracePeriod || Mathf.Approximately(Time.timeScale, 0.0f);
        
        private bool enableRumble = true;
        public bool EnableRumble
        {
            get => enableRumble;
            set => enableRumble = value;
        }

        private void Awake()
        {
            startTime = Time.time;
            
#if UNITY_EDITOR
            UnityEditor.EditorApplication.pauseStateChanged += HandleEditorPauseStateChangedEvent;
#endif // UNITY_EDITOR

#if GRAPH_RUMBLE
            Graph.Create("Low Frequency Rumble", Color.red, () => rumblePropertiesCombined.LowFrequencyRumble)
                .AddLine("High Frequency Rumble", Color.green, () => rumblePropertiesCombined.HighFrequencyRumble);
#endif // GRAPH_RUMBLE
        }

        private void OnDestroy()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.pauseStateChanged -= HandleEditorPauseStateChangedEvent;
#endif // UNITY_EDITOR

            ResetAllHaptics();
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

        private void Update()
        {
            CullFinishedRumblePlaybacks();
            ComputeRumbleProperties();
            PassRumbleOnToHardware(rumblePropertiesCombined);
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
            PlaybackType playback = new PlaybackType();
            playback.Initialize(config, origin, opacity);
            
            playbacks.Add(playback);
            
            AddRumble(playback);
            
            playback.Start();

            return playback;
        }
    }
}
