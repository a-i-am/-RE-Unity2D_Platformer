using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[System.Serializable]
public class DialogueClip : PlayableAsset, ITimelineClipAsset
{
    public DialougeArea dialogueArea; // 대화 내용

    public ClipCaps clipCaps
    {
        get { return ClipCaps.None; }
    }

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<DialogueBehaviour>.Create(graph);
        DialogueBehaviour behaviour = playable.GetBehaviour();
        behaviour.dialogueArea = dialogueArea;
        return playable;
    }
}

public class DialogueBehaviour : PlayableBehaviour
{
    public DialougeArea dialogueArea;
    private bool isDialogueShown = false;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        // UI 관련 컴포넌트를 받아옴
        var dialogueManager = playerData as Dialouge;
        if (dialogueManager == null)
            return;

        if (!isDialogueShown)
        {
            // ShowDialogue 메서드 호출
            dialogueManager.ShowDialogue();
            isDialogueShown = true;
        }
    }

    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        isDialogueShown = false;
    }
}

