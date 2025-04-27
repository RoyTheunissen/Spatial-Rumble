using UnityEngine;

namespace RoyTheunissen.SpatialRumble.Curves
{
    /// <summary>
    /// Acts like an Animation Curve except it's more reusable.
    /// </summary>
    [CreateAssetMenu(fileName = "CurveAsset", menuName = "ScriptableObject/Unity Haptics/Curve Asset")]
    public class CurveAsset : ScriptableObject 
    {
        [SerializeField]
        private AnimationCurve animationCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
        public AnimationCurve AnimationCurve
        {
            get => animationCurve;
            set
            {
                // Create a new animation curve but with copies of its keys.
                animationCurve = new AnimationCurve();
                foreach (Keyframe inputKeyframe in value.keys)
                {
                    animationCurve.AddKey(inputKeyframe);
                }
            }
        }

        public float Evaluate(float time)
        {
            return animationCurve.Evaluate(time);
        }

        public Keyframe[] keys
        {
            get => animationCurve.keys;
            set => animationCurve.keys = value;
        }

        public Keyframe this[int index] => animationCurve[index];

        public int length => animationCurve.length;

        public WrapMode preWrapMode
        {
            get => animationCurve.preWrapMode;
            set => animationCurve.preWrapMode = value;
        }

        public WrapMode postWrapMode
        {
            get => animationCurve.postWrapMode;
            set => animationCurve.postWrapMode = value;
        }

        public float Duration
        {
            get
            {
                if (animationCurve.length < 2)
                    return 0.0f;
            
                return animationCurve[animationCurve.length - 1].time;
            }
        }

        public static implicit operator AnimationCurve(CurveAsset curveAsset)
        {
            return curveAsset == null ? null : curveAsset.AnimationCurve;
        }
    }
}
