using UnityEditor;

namespace RoyTheunissen.UnityHaptics.Curves
{
    /// <summary>
    /// 
    /// </summary>
    [CustomEditor(typeof(CurveAsset))]
    public class CurveAssetEditor : Editor 
    {
        private SerializedProperty animationCurve;

        private void OnEnable()
        {
            animationCurve = serializedObject.FindProperty("animationCurve");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            serializedObject.Update();

            EditorGUILayout.PropertyField(animationCurve);
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}
