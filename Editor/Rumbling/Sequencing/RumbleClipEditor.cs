using UnityEditor;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Timeline;

namespace RoyTheunissen.UnityHaptics.Rumbling
{
    /// <summary>
    /// Draws a nice preview of the rumble in the clip.
    /// </summary>
    [CustomTimelineEditor(typeof(RumbleClip))]
    public sealed class RumbleClipEditor : ClipEditor 
    {
        public override ClipDrawOptions GetClipOptions(TimelineClip clip)
        {
            ClipDrawOptions clipOptions = base.GetClipOptions(clip);
            clipOptions.highlightColor = Color.yellow;
            return clipOptions;
        }

        public override void OnClipChanged(TimelineClip clip)
        {
            base.OnClipChanged(clip);

            RumbleClip rumbleClip = (RumbleClip)clip.asset;

            if (rumbleClip.Config != null)
                clip.displayName = rumbleClip.Config.name;
            else
                clip.displayName = "N/A";

            if (rumbleClip.IsLooping)
                clip.displayName += " (Loop)";
        }

        public override void DrawBackground(TimelineClip clip, ClipBackgroundRegion region)
        {
            base.DrawBackground(clip, region);
            
            RumbleClip rumbleClip = (RumbleClip)clip.asset;
            if (rumbleClip.Config != null)
            {
                if (rumbleClip.Config.UseLowFrequency)
                    DrawCurve(rumbleClip, region, rumbleClip.Config.CurveLowFrequency, 1.0f, new Color(1, 0, 0, 0.5f));
                if (rumbleClip.Config.UseHighFrequency)
                    DrawCurve(rumbleClip, region, rumbleClip.Config.CurveHighFrequency, 1.0f, new Color(0, 1, 0, 0.5f));
            }
        }
        
        private float GetFraction(float value, float start, float end)
        {
            if (Mathf.Approximately(start, end))
                return 0.0f;
        
            return Mathf.Clamp01((value - start) / (end - start));
        }

        private void DrawCurve(
            RumbleClip rumbleClip, ClipBackgroundRegion region, AnimationCurve curve, float thickness, Color color)
        {
            Rect area = region.position;
            Vector2 GetAreaPos(Vector2 curvePos)
            {
                float timeInClip = curvePos.x;
                float timeInRegion = GetFraction(timeInClip, (float)region.startTime, (float)region.endTime);
                return new Vector2(
                    Mathf.RoundToInt(area.xMin + timeInRegion * area.width),
                    Mathf.RoundToInt(area.yMax - Mathf.Clamp01(curvePos.y) * area.height)
                    );
            }
            
            float duration = (float)(region.endTime - region.startTime);
            float interval = 0.025f;
            float startTime = (float)region.startTime;
            Vector2 posPrevious = new Vector2(startTime, curve.Evaluate(0.0f));
            for (float x = startTime + interval; x < startTime + duration; x += interval)
            {
                Vector2 pos = new Vector2(x, curve.Evaluate(x));
                
                DrawLine(GetAreaPos(posPrevious), GetAreaPos(pos), thickness, color);

                posPrevious = pos;
            }
        }
        
        private void DrawLine(Vector2 from, Vector2 to, float thickness, Color color)
        {
            Vector2 delta = to - from;
            float distance = delta.magnitude;
            if (Mathf.Approximately(distance, 0.0f))
                return;

            float angle = Mathf.Rad2Deg * Mathf.Atan2(delta.y, delta.x);

            GUIUtility.RotateAroundPivot(angle, from);
            EditorGUI.DrawRect(new Rect(from.x, from.y + thickness * -0.5f, distance, thickness), color);
            GUI.matrix = Matrix4x4.identity;
        }
    }
}
