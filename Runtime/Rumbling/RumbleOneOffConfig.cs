using UnityEngine;

namespace RoyTheunissen.UnityHaptics.Rumbling
{
    /// <summary>
    /// Represents the settings for a brief rumble that plays once.
    /// </summary>
    [CreateAssetMenu(fileName = "RumbleOneOffConfig", menuName = MenuPaths.ScriptableObjectsRumble + "Rumble Config (One-Off)")]
    public sealed class RumbleOneOffConfig : RumbleConfigGeneric<RumbleOneOffPlayback>
    {
        public float Duration => Mathf.Max(CurveLowFrequency.Duration, CurveHighFrequency.Duration);
    }
    
    public sealed class RumbleOneOffPlayback : RumblePlaybackWithConfig<RumbleOneOffConfig, RumbleOneOffPlayback>
    {
        private float Duration => Config.Duration;

        public override bool IsFinished => Time >= Duration;

        protected override void GetRumbleInternal(ref RumbleProperties rumbleProperties)
        {
            if (Config.UseLowFrequency)
                rumbleProperties.LowFrequencyRumble = Config.CurveLowFrequency.Evaluate(Time);
            
            if (Config.UseHighFrequency)
                rumbleProperties.HighFrequencyRumble = Config.CurveHighFrequency.Evaluate(Time);
        }
    }
}
