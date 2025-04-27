using UnityEngine;

namespace RoyTheunissen.SpatialRumble.Rumbling
{
    /// <summary>
    /// A rumble to be combined with other rumbles and sent to the hardware. 
    /// </summary>
    public interface IRumble
    {
        RumbleBlendModes BlendMode { get; }

        float Opacity { get; }

        float SpatialBlend { get; }
        Vector3 SpatialOrigin { get; }
        float SpatialRadius { get; }

        void GetRumble(ref RumbleProperties rumbleProperties);
    }

    public enum RumbleBlendModes
    {
        Additive,
        Override,
    }
}
