using UnityEngine;

namespace RoyTheunissen.UnityHaptics.Rumbling
{
    public interface IRumbleService
    {
        bool EnableRumble { get; set; }
        RumbleProperties RumbleProperties { get; }
        void Cleanup();
        void Update();
        
        void Pause(object owner);
        void Resume(object owner);
        void RegisterListener(RumbleListener listener);
        void UnregisterListener(RumbleListener listener);
        void AddRumble(IRumble rumble);
        void RemoveRumble(IRumble rumble);

        PlaybackType Play<PlaybackType>(RumbleConfigBase config, Transform origin, float opacity)
            where PlaybackType : RumblePlayback, new();
    }
}
