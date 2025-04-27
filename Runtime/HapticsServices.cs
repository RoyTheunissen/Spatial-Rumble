using System;
using RoyTheunissen.SpatialRumble.Rumbling;
using UnityEngine;

namespace RoyTheunissen.SpatialRumble
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
            set
            {
                if (cachedRumbleService != null && value != null)
                {
                    Debug.LogWarning($"You are overriding the Rumble service from an instance of " +
                                     $"'{cachedRumbleService.GetType()}' to an instance of '{value.GetType()}'. " +
                                     $"Is this intended?");
                }
                
                cachedRumbleService = value;
            }
        }
    }
}
