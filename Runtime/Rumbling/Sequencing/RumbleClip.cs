using RoyTheunissen.SpatialRumble.Rumbling;
using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// Serialized clip in the timeline. This creates behaviours which are responsible for runtime functionality.
/// </summary>
[System.Serializable]
public abstract class RumbleClip : PlayableAsset
{
    [SerializeField] protected ExposedReference<Transform> origin;
    [SerializeField] protected float opacity = 1.0f;

    public abstract RumbleConfigBase Config { get; }

    public abstract bool IsLooping { get; }
}
