using UnityEditor;
using UnityEngine;

namespace RoyTheunissen.SpatialRumble.Curves
{
    /// <summary>
    /// Draws an animation curve to serve as an asset so you can re-use it.
    /// </summary>
    [CustomEditor(typeof(RumbleCurveAsset))]
    public class CurveAssetEditor : Editor 
    {
        private SerializedProperty animationCurve;

        private void OnEnable()
        {
            animationCurve = serializedObject.FindProperty("animationCurve");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(animationCurve, GUIContent.none);
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}
