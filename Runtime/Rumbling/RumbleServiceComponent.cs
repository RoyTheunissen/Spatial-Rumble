using System;
using RoyTheunissen.SpatialRumble.Curves;
using UnityEngine;

namespace RoyTheunissen.SpatialRumble.Rumbling
{
    /// <summary>
    /// Responsible for managing the playback of rumble effects and passing them on to the hardware.
    ///
    /// Basically delegates the implementation completely to the pure C# version, but you don't have to care about that.
    /// You can just add this component to your scene somewhere or on a service prefab and it'll work.
    /// </summary>
    public sealed class RumbleServiceComponent : MonoBehaviour, IRumbleService
    {
        [Tooltip("Whether to automatically register this component as the HapticsServices.Rumble service in " +
                 "Awake / unregister it in OnDestroy. Disable this if you want control over the execution order.")]
        [SerializeField] private bool autoRegister = true;
        
        [Space]
        [Tooltip("How the intensity of a rumble playback rolls off over distance to the rumble listener.")]
        [SerializeField] private CurveAsset rumbleRollOff;
        
        [Tooltip("The spatial radius that rumbles have when none is specified explicitly.")]
        [SerializeField] private float spatialRadiusDefault = RumbleService.SpatialRadiusRecommendedDefault;
        public float SpatialRadiusDefault => spatialRadiusDefault;

        public bool EnableRumble
        {
            get => Instance.EnableRumble;
            set => Instance.EnableRumble = value;
        }

        public RumbleProperties RumbleProperties => Instance.RumbleProperties;

        private bool isInitialized;

        private RumbleService instance;
        private RumbleService Instance
        {
            get
            {
                Initialize();
                
                return instance;
            }
        }

        private void Awake()
        {
            Initialize();

            if (autoRegister)
                HapticsServices.Rumble = this;
        }

        private void OnDestroy()
        {
            if (autoRegister && ReferenceEquals(HapticsServices.Rumble, this))
                HapticsServices.Rumble = null;
            
            Cleanup();
        }

        public void Initialize()
        {
            if (isInitialized)
                return;

            isInitialized = true;

            instance = new RumbleService(rumbleRollOff, spatialRadiusDefault);
        }
        
        public void Cleanup()
        {
            if (isInitialized)
                instance.Cleanup();
        }

        public void Update()
        {
            Instance.Update();
        }

        public void Pause(object owner)
        {
            Instance.Pause(owner);
        }

        public void Resume(object owner)
        {
            Instance.Resume(owner);
        }

        public void RegisterListener(RumbleListener listener)
        {
            Instance.RegisterListener(listener);
        }

        public void UnregisterListener(RumbleListener listener)
        {
            Instance.UnregisterListener(listener);
        }

        public void AddRumble(IRumble rumble)
        {
            Instance.AddRumble(rumble);
        }

        public void RemoveRumble(IRumble rumble)
        {
            Instance.RemoveRumble(rumble);
        }

        public PlaybackType Play<PlaybackType>(RumbleConfigBase config, Transform origin, float opacity)
            where PlaybackType : RumblePlayback, new()
        {
            return Instance.Play<PlaybackType>(config, origin, opacity);
        }
    }
}
