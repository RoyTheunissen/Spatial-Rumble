using UnityEngine;

namespace RoyTheunissen.UnityHaptics.Rumbling
{
    /// <summary>
    /// Represents the settings for a rumble that plays for a variable amount of time.
    /// </summary>
    [CreateAssetMenu(fileName = "RumbleLoopingConfig", menuName = MenuPaths.ScriptableObjectsRumble + "Rumble Config (Looping)")]
    public sealed class RumbleLoopingConfig : RumbleConfigGeneric<RumbleLoopingPlayback>
    {
        private void Reset()
        {
            curveLF.postWrapMode = WrapMode.Loop;
            curveHF.postWrapMode = WrapMode.Loop;
        }
    }
    
    public sealed class RumbleLoopingPlayback : RumblePlaybackWithConfig<RumbleLoopingConfig, RumbleLoopingPlayback>
    {
        private float opacityMultiplier = 1.0f;

        public override float Opacity => base.Opacity * opacityMultiplier;
        
        // TODO
        // private Tween cachedTween;
        // public Tween Tween
        // {
        //     get
        //     {
        //         if (cachedTween == null)
        //             cachedTween = new Tween(f => opacityMultiplier = f).SetContinuous(true).SkipToIn();
        //         return cachedTween;
        //     }
        // }

        public override void Cleanup()
        {
            base.Cleanup();
            
            // TODO
            //cachedTween.Cleanup();
        }

        protected override void GetRumbleInternal(ref RumbleProperties rumbleProperties)
        {
            if (Config.UseLowFrequency)
                rumbleProperties.LowFrequencyRumble = Config.CurveLF.Evaluate(Time);
            
            if (Config.UseHighFrequency)
                rumbleProperties.HighFrequencyRumble = Config.CurveHF.Evaluate(Time);
        }
    }
}
