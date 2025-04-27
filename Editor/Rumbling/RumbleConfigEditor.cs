using UnityEditor;
using UnityEngine;

namespace RoyTheunissen.SpatialRumble.Rumbling
{
    [CustomEditor(typeof(RumbleConfigBase), true)]
    public sealed class RumbleConfigEditor : Editor
    {
        private const float ToggleWidth = 16;
        
        private readonly GUIContent lowFrequencyLabel = new("Low Frequency");
        private readonly GUIContent highFrequencyLabel = new("High Frequency");
        private readonly GUIContent spatialRadiusOverrideLabel = new("Radius Override");
        
        private SerializedProperty opacityProperty;
        private SerializedProperty blendModeProperty;
        
        private SerializedProperty useLowFrequencyProperty;
        private SerializedProperty curveLowFrequencyProperty;
        private SerializedProperty useHighFrequencyProperty;
        private SerializedProperty curveHighFrequencyProperty;
        
        private SerializedProperty spatialBlendProperty;
        private SerializedProperty overrideSpatialRadiusProperty;
        private SerializedProperty spatialRadiusOverrideProperty;

        private void OnEnable()
        {
            blendModeProperty = serializedObject.FindProperty("blendMode");
            opacityProperty = serializedObject.FindProperty("opacity");
            
            useLowFrequencyProperty = serializedObject.FindProperty("useLowFrequency");
            curveLowFrequencyProperty = serializedObject.FindProperty("curveLowFrequency");
            useHighFrequencyProperty = serializedObject.FindProperty("useHighFrequency");
            curveHighFrequencyProperty = serializedObject.FindProperty("curveHighFrequency");
            spatialBlendProperty = serializedObject.FindProperty("spatialBlend");
            
            overrideSpatialRadiusProperty = serializedObject.FindProperty("overrideSpatialRadius");
            spatialRadiusOverrideProperty = serializedObject.FindProperty("spatialRadiusOverride");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // Blending
            EditorGUILayout.PropertyField(blendModeProperty);
            EditorGUILayout.PropertyField(opacityProperty);
            
            // Curves
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Curves", EditorStyles.boldLabel);
            
            DrawFrequencyCurve(useLowFrequencyProperty, curveLowFrequencyProperty, lowFrequencyLabel);
            
            EditorGUILayout.Space();
            
            DrawFrequencyCurve(useHighFrequencyProperty, curveHighFrequencyProperty, highFrequencyLabel);
            
            // Spatialization
            EditorGUILayout.PropertyField(spatialBlendProperty);
            EditorGUILayout.BeginHorizontal();
            overrideSpatialRadiusProperty.boolValue = EditorGUILayout.Toggle(
                GUIContent.none, overrideSpatialRadiusProperty.boolValue, GUILayout.Width(ToggleWidth));

            using (new EditorGUI.DisabledScope(!overrideSpatialRadiusProperty.boolValue))
            {
                const float inset = ToggleWidth + 3;
                EditorGUIUtility.labelWidth -= inset;
                EditorGUILayout.PropertyField(spatialRadiusOverrideProperty, spatialRadiusOverrideLabel);
                EditorGUIUtility.labelWidth += inset;
            }

            EditorGUILayout.EndHorizontal();

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawFrequencyCurve(
            SerializedProperty enabledProperty, SerializedProperty curveProperty, GUIContent label)
        {
            EditorGUILayout.BeginHorizontal();
            enabledProperty.boolValue = EditorGUILayout.Toggle(
                GUIContent.none, enabledProperty.boolValue, GUILayout.Width(ToggleWidth));

            using (new EditorGUI.DisabledScope(!enabledProperty.boolValue))
            {
                EditorGUILayout.PropertyField(curveProperty, label);
            }

            EditorGUILayout.EndHorizontal();
        }
    }
}
