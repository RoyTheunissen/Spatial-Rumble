using UnityEditor;

namespace RoyTheunissen.UnityHaptics.Rumbling
{
    //[CustomEditor(typeof(RumbleConfigBase), true)]
    public sealed class RumbleConfigEditor : Editor
    {
        private SerializedProperty opacityProperty;
        private SerializedProperty blendModeProperty;
        
        private SerializedProperty useLowFrequencyProperty;
        private SerializedProperty curveLFProperty;
        private SerializedProperty useHighFrequencyProperty;
        private SerializedProperty curveHFProperty;
        
        private SerializedProperty spatialBlendProperty;
        private SerializedProperty overrideSpatialRadiusProperty;
        private SerializedProperty spatialRadiusOverrideProperty;

        private void OnEnable()
        {
            opacityProperty = serializedObject.FindProperty("opacity");
            blendModeProperty = serializedObject.FindProperty("blendMode");
            
            useLowFrequencyProperty = serializedObject.FindProperty("useLowFrequency");
            curveLFProperty = serializedObject.FindProperty("curveLF");
            useHighFrequencyProperty = serializedObject.FindProperty("useHighFrequency");
            curveHFProperty = serializedObject.FindProperty("curveHF");
            spatialBlendProperty = serializedObject.FindProperty("spatialBlend");
            
            overrideSpatialRadiusProperty = serializedObject.FindProperty("overrideSpatialRadius");
            spatialRadiusOverrideProperty = serializedObject.FindProperty("spatialRadiusOverride");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            serializedObject.Update();

            // Blending
            EditorGUILayout.PropertyField(opacityProperty);
            EditorGUILayout.PropertyField(blendModeProperty);
            
            // Curves
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(useLowFrequencyProperty);
            if (useLowFrequencyProperty.boolValue)
                EditorGUILayout.PropertyField(curveLFProperty);
            
            EditorGUILayout.PropertyField(useHighFrequencyProperty);
            if (useHighFrequencyProperty.boolValue)
                EditorGUILayout.PropertyField(curveHFProperty);
            
            // Spatialization
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(spatialBlendProperty);
            EditorGUILayout.PropertyField(overrideSpatialRadiusProperty);
            if (overrideSpatialRadiusProperty.boolValue)
                EditorGUILayout.PropertyField(spatialRadiusOverrideProperty);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
