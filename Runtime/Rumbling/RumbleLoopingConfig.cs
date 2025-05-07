using UnityEngine;

#if SCAFFOLDING_TWEENING
using RoyTheunissen.Scaffolding.Tweening;
#endif // SCAFFOLDING_TWEENING

namespace RoyTheunissen.SpatialRumble.Rumbling
{
    /// <summary>
    /// Represents the settings for a rumble that plays for a variable amount of time.
    /// </summary>
    [CreateAssetMenu(fileName = "RumbleLoopingConfig", menuName = MenuPaths.ScriptableObjects + "Rumble Config (Looping)")]
    public sealed class RumbleLoopingConfig : RumbleConfigGeneric<RumbleLoopingPlayback>
    {
        private void Reset()
        {
            curveLowFrequency.postWrapMode = WrapMode.Loop;
            curveHighFrequency.postWrapMode = WrapMode.Loop;
        }
    }
    
    public sealed class RumbleLoopingPlayback : RumblePlaybackWithConfig<RumbleLoopingConfig, RumbleLoopingPlayback>
    {
        private float opacityMultiplier = 1.0f;
        public float OpacityMultiplier
        {
            get => opacityMultiplier;
            set => opacityMultiplier = value;
        }

        public override float Opacity => base.Opacity * opacityMultiplier;
        
#if SCAFFOLDING_TWEENING
        private Tween cachedTween;
        public Tween Tween
        {
            get
            {
                if (cachedTween == null)
                    cachedTween = new Tween(f => opacityMultiplier = f).SetContinuous(true).SkipToIn();
                return cachedTween;
            }
        }
#endif // SCAFFOLDING_TWEENING

        public override void Cleanup()
        {
            base.Cleanup();
            
#if SCAFFOLDING_TWEENING
            cachedTween.Cleanup();
#endif // SCAFFOLDING_TWEENING
        }

        protected override void GetRumbleInternal(ref RumbleProperties rumbleProperties)
        {
            if (Config.UseLowFrequency)
                rumbleProperties.LowFrequencyRumble = Config.CurveLowFrequency.Evaluate(Time);
            
            if (Config.UseHighFrequency)
                rumbleProperties.HighFrequencyRumble = Config.CurveHighFrequency.Evaluate(Time);
        }
    }
}
