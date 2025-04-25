using System;
using RoyTheunissen.UnityHaptics.Rumbling;

namespace RoyTheunissen.UnityHaptics
{
    /// <summary>
    /// Class that exposes the instances of the various haptics related systems.
    /// Setting it up this way means that you have control of which instance is considered the 'current' instance.
    /// </summary>
    public static class HapticsServices
    {
        [NonSerialized] private static IRumbleService cachedRumbleService;
        [NonSerialized] private static bool didCacheRumbleService;
        public static IRumbleService Rumble
        {
            get => cachedRumbleService;
            set => cachedRumbleService = value;
        }
    }
}
