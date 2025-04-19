using UnityEngine;
using RoyTheunissen.UnityHaptics.Curves;

namespace RoyTheunissen.UnityHaptics.Rumbling
{
    /// <summary>
    /// A 'rumble' that you can play back.
    /// </summary>
    public abstract class RumbleConfigBase : ScriptableObject
    {
        [SerializeField] private float opacity = 1.0f;
        public float Opacity => opacity;
        
        [SerializeField] private RumbleBlendModes blendMode;
        public RumbleBlendModes BlendMode => blendMode;

        [Header("Low Frequency")]
        [SerializeField] private bool useLowFrequency;
        public bool UseLowFrequency => useLowFrequency;
        
        [SerializeField]
        protected CurveReference curveLF = new(new Keyframe(0.0f, 0.0f), new Keyframe(0.5f, 1.0f), new Keyframe(1.0f, 0.0f));
        public CurveReference CurveLF => curveLF;
        
        [Header("High Frequency")]
        [SerializeField] private bool useHighFrequency;
        public bool UseHighFrequency => useHighFrequency;
        
        [SerializeField]
        protected CurveReference curveHF = new(new Keyframe(0.0f, 0.0f), new Keyframe(0.5f, 1.0f), new Keyframe(1.0f, 0.0f));
        public CurveReference CurveHF => curveHF;

        [Header("Spatialization")]
        [SerializeField, Range(0, 1)] private float spatialBlend = 1.0f;
        public float SpatialBlend => spatialBlend;

        [SerializeField] private bool overrideSpatialRadius;
        [SerializeField]
        private float spatialRadiusOverride = RumbleService.SpatialRadiusDefault;
        public float SpatialRadius =>
            overrideSpatialRadius ? spatialRadiusOverride : RumbleService.SpatialRadiusDefault;
        
        // TODO:
        //protected static ServiceReference<RumbleService> rumbleService = new();
    }
    
    public abstract class RumbleConfigGeneric<PlaybackType> : RumbleConfigBase
        where PlaybackType : RumblePlayback, new()
    {
        public PlaybackType Play(Transform origin = null, float opacity = 1.0f)
        {
            // TODO:
            //return rumbleService.Reference.Play<PlaybackType>(this, origin, opacity);
            return default;
        }
    }
}
