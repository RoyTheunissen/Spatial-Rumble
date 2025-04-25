using RoyTheunissen.UnityHaptics.Rumbling;
using RoyTheunissen.UnityHaptics.Rumbling.Sequencing;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class RumbleLoopingClip : RumbleClip
{
    [Space] [SerializeField] public RumbleLoopingConfig rumbleLooping;
    
    public override RumbleConfigBase Config => rumbleLooping;

    public override bool IsLooping => true;

    // Factory method that generates a playable based on this asset
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        ScriptPlayable<RumbleLoopingBehaviour> playable = ScriptPlayable<RumbleLoopingBehaviour>.Create(graph);

        RumbleLoopingBehaviour behaviour = playable.GetBehaviour();
        behaviour.origin = origin.Resolve(graph.GetResolver());
        behaviour.opacity = opacity;
        
        behaviour.rumbleLooping = rumbleLooping;
        
        return playable;
    }
}
