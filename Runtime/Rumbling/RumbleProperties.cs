using UnityEngine;

namespace RoyTheunissen.UnityHaptics.Rumbling
{
    /// <summary>
    /// All of the exposed rumble properties that are sent to the hardware. 
    /// </summary>
    public struct RumbleProperties
    {
        private float lowFrequencyRumble;
        public float LowFrequencyRumble
        {
            get => lowFrequencyRumble;
            set => lowFrequencyRumble = Mathf.Clamp01(value);
        }

        private float highFrequencyRumble;
        public float HighFrequencyRumble
        {
            get => highFrequencyRumble;
            set => highFrequencyRumble = Mathf.Clamp01(value);
        }

        public void Reset()
        {
            lowFrequencyRumble = 0.0f;
            highFrequencyRumble = 0.0f;
        }
    }
}
