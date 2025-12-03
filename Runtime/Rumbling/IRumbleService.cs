using UnityEngine;

namespace RoyTheunissen.SpatialRumble.Rumbling
{
    public interface IRumbleService
    {
        bool EnableRumble { get; set; }
        RumbleProperties RumbleProperties { get; }
        float SpatialRadiusDefault { get; }
        void Cleanup();
        void Update();
        
        void Pause(object owner);
        void Resume(object owner);
        void RegisterListener(RumbleListener listener);
        void UnregisterListener(RumbleListener listener);
        void AddRumble(IRumble rumble);
        void RemoveRumble(IRumble rumble);

        void RegisterCustomRumbleHandler(ICustomRumbleHandler customRumbleHandler);
        void UnregisterCustomRumbleHandler(ICustomRumbleHandler customRumbleHandler);

        PlaybackType Play<PlaybackType>(RumbleConfigBase config, Transform origin, float opacity)
            where PlaybackType : RumblePlayback, new();
    }
}
