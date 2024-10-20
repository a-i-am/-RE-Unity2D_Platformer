using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackClipType(typeof(DialogueClip))] // 이 트랙에서는 DialogueClip만 사용 가능
[TrackBindingType(typeof(Dialouge))]  // 바인딩할 수 있는 타입을 Dialouge로 제한
public class DialogueTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<DialogueMixerBehaviour>.Create(graph, inputCount);
    }
}
