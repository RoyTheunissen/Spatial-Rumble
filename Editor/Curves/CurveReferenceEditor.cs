using UnityEditor;
using UnityEngine;

namespace RoyTheunissen.SpatialRumble.Curves
{
    /// <summary>
    /// Draws a curve reference in a more compact way.
    /// </summary>
    [CustomPropertyDrawer(typeof(RumbleCurveReference))]
    public class CurveReferencePropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return RectExtensions.GetHeightForLines(2);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            
            Rect firstLineRect = position.GetControlFirstRect();
            Rect secondLineRect = firstLineRect.GetControlNextRect();
            
            SerializedProperty modeProperty = property.FindPropertyRelative("mode");
            SerializedProperty curveProperty = property.FindPropertyRelative("curve");
            SerializedProperty curveAssetProperty = property.FindPropertyRelative("curveAsset");
            
            EditorGUI.PropertyField(firstLineRect, modeProperty, label);
            
            using (new EditorGUI.IndentLevelScope())
            {
                if (modeProperty.intValue == (int)RumbleCurveReference.Modes.NewCurve)
                    EditorGUI.PropertyField(secondLineRect, curveProperty);
                else if (modeProperty.intValue == (int)RumbleCurveReference.Modes.CurveAsset)
                    EditorGUI.PropertyField(secondLineRect, curveAssetProperty);
            }
            
            EditorGUI.EndProperty();
        }
    }
}
