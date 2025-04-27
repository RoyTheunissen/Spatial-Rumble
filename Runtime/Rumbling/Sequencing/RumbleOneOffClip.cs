using RoyTheunissen.SpatialRumble.Rumbling;
using RoyTheunissen.SpatialRumble.Rumbling.Sequencing;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class RumbleOneOffClip : RumbleClip
{
    [Space] [SerializeField] public RumbleOneOffConfig rumbleOneOff;

    public override RumbleConfigBase Config => rumbleOneOff;

    public override bool IsLooping => false;

    // Factory method that generates a playable based on this asset
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        ScriptPlayable<RumbleOneOffBehaviour> playable = ScriptPlayable<RumbleOneOffBehaviour>.Create(graph);

        RumbleOneOffBehaviour behaviour = playable.GetBehaviour();
        behaviour.origin = origin.Resolve(graph.GetResolver());
        behaviour.opacity = opacity;
        
        behaviour.rumbleOneOff = rumbleOneOff;
        
        return playable;
    }
}
