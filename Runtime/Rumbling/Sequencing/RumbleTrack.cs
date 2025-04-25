using UnityEngine.Timeline;

/// <summary>
/// Timeline track that contains clips which in turn create the behaviours that drive runtime functionality.
/// </summary>
[TrackClipType(typeof(RumbleClip))]
[System.Serializable]
public class RumbleTrack : TrackAsset
{
}
