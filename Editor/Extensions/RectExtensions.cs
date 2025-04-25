using System;

#if UNITY_EDITOR
using UnityEditor;
#endif // UNITY_EDITOR

namespace UnityEngine
{
    internal static class RectExtensions
    {
        public static Vector2 GetLeftCenter(this Rect rect)
        {
            return new Vector2(rect.min.x, rect.center.y);
        }
        
        public static Vector2 GetRightCenter(this Rect rect)
        {
            return new Vector2(rect.max.x, rect.center.y);
        }
        
        public static Vector2 GetBottomCenter(this Rect rect)
        {
            return new Vector2(rect.center.x, rect.min.y);
        }
        
        public static Vector2 GetTopCenter(this Rect rect)
        {
            return new Vector2(rect.center.x, rect.max.y);
        }
        
        public static Rect Scale(this Rect rect, float scale)
        {
            return new Rect(rect.position * scale, rect.size * scale);
        }
        
        public static Rect Scale(this Rect rect, Vector2 scale)
        {
            return new Rect(rect.position * scale, rect.size * scale);
        }
        
        public static Vector2 GetPosition(this Rect rect, Vector2 localPosition)
        {
            return new Vector2(rect.xMin + rect.width * localPosition.x, rect.yMin + rect.height * localPosition.y);
        }
        
        public static Rect Expand(this Rect rect, float xMin, float xMax, float yMin, float yMax)
        {
            rect.xMin -= xMin;
            rect.xMax += xMax;
            rect.yMin -= yMin;
            rect.yMax += yMax;
            return rect;
        }
        
        public static Rect Expand(this Rect rect, float amount)
        {
            return rect.Expand(amount, amount, amount, amount);
        }
        
        public static Rect Inset(this Rect rect, float xMin, float xMax, float yMin, float yMax)
        {
            rect.xMin += xMin;
            rect.xMax -= xMax;
            rect.yMin += yMin;
            rect.yMax -= yMax;
            return rect;
        }
        
        public static Rect Inset(this Rect rect, float inset)
        {
            return rect.Inset(inset, inset, inset, inset);
        }
        
#if UNITY_EDITOR
        public static float GetHeightForLines(int lineCount, bool alwaysIncludingSpacingAtEnd = false)
        {
            if (lineCount == 0)
                return 0;
            
            if (lineCount == 1 && !alwaysIncludingSpacingAtEnd)
                return EditorGUIUtility.singleLineHeight;
            
            return (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * lineCount;
        }
        
        public static float GetCombinedHeight(params float[] heights)
        {
            float combinedHeight = 0.0f;
            for (int i = 0; i < heights.Length; i++)
            {
                combinedHeight += heights[i];
                
                if (i < heights.Length - 1)
                    combinedHeight += EditorGUIUtility.standardVerticalSpacing;
            }
            return combinedHeight;
        }
        
        public static Rect GetControlRect(this Rect rect, int index)
        {
            return GetControlRect(rect, index, EditorGUIUtility.singleLineHeight);
        }
        
        public static Rect GetControlRect(this Rect rect, int index, float height)
        {
            return new Rect(
                rect.x, rect.yMin + (height + EditorGUIUtility.standardVerticalSpacing) * index, rect.width, height);
        }
        
        public static Rect GetControlFirstRect(this Rect rect)
        {
            return GetControlFirstRect(rect, EditorGUIUtility.singleLineHeight);
        }

        public static Rect GetControlFirstRect(this Rect rect, float height)
        {
            return new Rect(rect.x, rect.y, rect.width, height);
        }
        
        public static Rect GetControlLastRect(this Rect rect)
        {
            return GetControlLastRect(rect, EditorGUIUtility.singleLineHeight);
        }

        public static Rect GetControlLastRect(this Rect rect, float height)
        {
            return new Rect(rect.x, rect.yMax - height, rect.width, height);
        }
        
        public static Rect GetControlPreviousRect(this Rect rect)
        {
            return GetControlPreviousRect(rect, EditorGUIUtility.singleLineHeight);
        }

        public static Rect GetControlPreviousRect(this Rect rect, float height)
        {
            return new Rect(rect.x, rect.yMin - height - EditorGUIUtility.standardVerticalSpacing, rect.width, height);
        }

        public static Rect GetControlNextRect(this Rect rect)
        {
            return GetControlNextRect(rect, EditorGUIUtility.singleLineHeight);
        }

        public static Rect GetControlNextRect(this Rect rect, float height)
        {
            return new Rect(rect.x, rect.yMax + EditorGUIUtility.standardVerticalSpacing, rect.width, height);
        }
        
        public static Rect GetControlFirstRectHorizontal(this Rect rect, float width)
        {
            return new Rect(rect.x, rect.y, width, rect.height);
        }

        public static Rect GetControlLastRectHorizontal(this Rect rect, float width)
        {
            return new Rect(rect.xMax - width, rect.y, width, rect.height);
        }

        public static Rect GetControlPreviousRectHorizontal(this Rect rect, float width)
        {
            return new Rect(rect.xMin - width - EditorGUIUtility.standardVerticalSpacing, rect.y, width, rect.height);
        }

        public static Rect GetControlNextRectHorizontal(this Rect rect, float width)
        {
            return new Rect(rect.xMax + EditorGUIUtility.standardVerticalSpacing, rect.y, width, rect.height);
        }

        public static Rect GetControlRemainderVertical(this Rect rect, Rect occupant)
        {
            return new Rect(rect.x, occupant.yMax, rect.width, rect.height - occupant.height);
        }
        
        public static Rect GetLabelRect(this Rect rect)
        {
            return new Rect(
                rect.x, rect.y, EditorGUIUtility.labelWidth,
                EditorGUIUtility.singleLineHeight);
        }
        
        public static Rect GetLabelRectRemainder(this Rect rect)
        {
            return new Rect(
                rect.x + EditorGUIUtility.labelWidth + EditorGUIUtility.standardVerticalSpacing, rect.y,
                rect.width - EditorGUIUtility.labelWidth,
                EditorGUIUtility.singleLineHeight);
        }
        
        public static Rect GetLabelRect(this Rect rect, out Rect remainder)
        {
            remainder = rect.GetLabelRectRemainder();
            return rect.GetLabelRect();
        }

        public static Rect Indent(this Rect rect)
        {
            return EditorGUI.IndentedRect(rect);
        }
        
        public static Rect Indent(this Rect rect, int amount)
        {
            int originalIndentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = amount;
            Rect result = EditorGUI.IndentedRect(rect);
            EditorGUI.indentLevel = originalIndentLevel;
            
            return result;
        }

        public static void Debug(this Rect rect)
        {
            Debug(rect, Color.red);
        }

        public static void Debug(this Rect rect, Color color)
        {
            EditorGUI.DrawRect(rect, new Color(color.r, color.g, color.b, 0.5f));
        }
#endif

        public static Rect GetSubRectFromLeft(this Rect rect, float width)
        {
            return new Rect(rect.x, rect.y, width, rect.height);
        }
        
        public static Rect GetSubRectFromLeft(this Rect rect, float width, out Rect remainder)
        {
            remainder = rect.GetSubRectFromRight(rect.width - width);
            return rect.GetSubRectFromLeft(width);
        }
        
        public static Rect GetSubRectFromRight(this Rect rect, float width)
        {
            return new Rect(rect.xMax - width, rect.y, width, rect.height);
        }
        
        public static Rect GetSubRectFromRight(this Rect rect, float width, out Rect remainder)
        {
            remainder = rect.GetSubRectFromLeft(rect.width - width);
            return rect.GetSubRectFromRight(width);
        }
        
        public static Rect GetSubRectFromTop(this Rect rect, float height)
        {
            return new Rect(rect.x, rect.y, rect.width, height);
        }
        
        public static Rect GetSubRectFromTop(this Rect rect, float height, out Rect remainder)
        {
            remainder = rect.GetSubRectFromBottom(rect.height - height);
            return rect.GetSubRectFromTop(height);
        }
        
        public static Rect GetSubRectFromBottom(this Rect rect, float height)
        {
            return new Rect(rect.x, rect.yMax - height, rect.width, height);
        }
        
        public static Rect GetSubRectFromBottom(this Rect rect, float height, out Rect remainder)
        {
            remainder = rect.GetSubRectFromTop(rect.height - height);
            return rect.GetSubRectFromBottom(height);
        }
        
        public static Rect SubtractFromLeft(this Rect rect, float width)
        {
            return new Rect(rect.x + width, rect.y, rect.width - width, rect.height);
        }
        
        public static Rect SubtractFromRight(this Rect rect, float width)
        {
            return new Rect(rect.x, rect.y, rect.width - width, rect.height);
        }
        
        public static Rect InverseTransform(this Rect rect, Rect child)
        {
            return new Rect(child.x - rect.x, child.y - rect.y, child.width, child.height);
        }
        
        public static Rect Transform(this Rect rect, Rect child)
        {
            return new Rect(child.x + rect.x, child.y + rect.y, child.width, child.height);
        }
        
        public static Rect GetHorizontalSection(this Rect rect, int index, int count)
        {
            float sectionWidth = rect.width / count;
            return new Rect(rect.x + sectionWidth * index, rect.y, sectionWidth, rect.height);
        }
        
        public static Rect GetVerticalSection(this Rect rect, int index, int count)
        {
            float sectionHeight = rect.height / count;
            return new Rect(rect.x, rect.y + sectionHeight * index, rect.width, sectionHeight);
        }
    }
}
