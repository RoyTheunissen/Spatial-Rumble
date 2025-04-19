using UnityEngine;

namespace RoyTheunissen.UnityHaptics.Rumbling
{
    /// <summary>
    /// Responsible for the playback of a specific rumble.
    /// </summary>
    public abstract class RumblePlayback : IRumble
    {
        protected RumbleConfigBase baseConfig;

        private float startTime;

        private bool isPlaying;
        
        private Transform origin;
        private float opacity = 1.0f;
        
        public RumbleBlendModes BlendMode => baseConfig.BlendMode;
        public virtual float Opacity => baseConfig.Opacity * opacity;
        public float SpatialBlend => origin == null ? 0.0f : baseConfig.SpatialBlend;
        public Vector3 SpatialOrigin => origin == null ? Vector3.zero : origin.position;
        public float SpatialRadius => baseConfig.SpatialRadius;

        protected float Time => UnityEngine.Time.time - startTime;

        public virtual bool IsFinished => !isPlaying;
        
        public void Initialize(RumbleConfigBase config, Transform origin = null, float opacity = 1.0f)
        {
            baseConfig = config;
            this.origin = origin;
            this.opacity = opacity;
        }

        public void Start()
        {
            if (isPlaying)
                return;

            isPlaying = true;
            
            startTime = UnityEngine.Time.time;
        }
        
        public void Stop()
        {
            if (!isPlaying)
                return;

            isPlaying = false;
        }

        public void GetRumble(ref RumbleProperties rumbleProperties)
        {
            if (!isPlaying)
                return;

            GetRumbleInternal(ref rumbleProperties);
        }
        
        public virtual void Cleanup()
        {
            Stop();
        }

        protected abstract void GetRumbleInternal(ref RumbleProperties rumbleProperties);
    }
    
    public abstract class RumblePlaybackWithConfig<ConfigType, ThisType> : RumblePlayback
        where ConfigType : RumbleConfigBase
        where ThisType : RumblePlayback
    {
        protected ConfigType Config => (ConfigType)baseConfig;
    }
}
