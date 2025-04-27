using System;
using UnityEngine;

namespace RoyTheunissen.SpatialRumble.Curves
{
    /// <summary>
    /// Acts like an Animation Curve except internally you can define a brand-new animation curve or re-use one from
    /// a Curve Asset.
    /// </summary>
    [Serializable]
    public sealed class CurveReference 
    {
        public enum Modes
        {
            NewCurve,
            CurveAsset,
        }

        [SerializeField] private Modes mode;

        [SerializeField] private AnimationCurve curve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
        [SerializeField] private CurveAsset curveAsset;

        public AnimationCurve AnimationCurve
        {
            get
            {
                switch (mode)
                {
                    case Modes.NewCurve:
                        return curve;
                    case Modes.CurveAsset:
                        return curveAsset;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            set
            {
                if (mode != Modes.NewCurve)
                {
                    Debug.LogWarning($"Tried to set Animation Curve value of a Curve Reference of type 'Curve Asset'. " +
                                     $"This would change an asset, which is probably not what you intended. Ignoring.");
                    return;
                }
                
                // Create a new animation curve but with copies of its keys.
                AnimationCurve = new AnimationCurve();
                foreach (Keyframe inputKeyframe in value.keys)
                {
                    AnimationCurve.AddKey(inputKeyframe);
                }
            }
        }

        public Keyframe[] keys
        {
            get => AnimationCurve.keys;
            set => AnimationCurve.keys = value;
        }

        public Keyframe this[int index] => AnimationCurve[index];

        public int length => AnimationCurve.length;

        public WrapMode preWrapMode
        {
            get => AnimationCurve.preWrapMode;
            set => AnimationCurve.preWrapMode = value;
        }

        public WrapMode postWrapMode
        {
            get => AnimationCurve.postWrapMode;
            set => AnimationCurve.postWrapMode = value;
        }

        public float Duration
        {
            get
            {
                if (AnimationCurve.length < 2)
                    return 0.0f;
            
                return AnimationCurve[AnimationCurve.length - 1].time;
            }
        }

        public CurveReference()
        {
            mode = Modes.NewCurve;
            curve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
        }
        
        public CurveReference(params Keyframe[] keyframes)
        {
            mode = Modes.NewCurve;
            curve = new AnimationCurve(keyframes);
        }
        
        public float Evaluate(float time)
        {
            return AnimationCurve.Evaluate(time);
        }

        public static implicit operator AnimationCurve(CurveReference curveAsset)
        {
            return curveAsset?.AnimationCurve;
        }
    }
}
