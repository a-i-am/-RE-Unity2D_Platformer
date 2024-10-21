using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DialogueLoopPlayableBehaviour : PlayableBehaviour
{
    public CutscenePauseMethod pauseMethod = CutscenePauseMethod.Pause;
    bool firstFrame = true;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        if (firstFrame)
        {
            Debug.Log("ProcessFrame called. Pause Method: " + pauseMethod);

            switch (pauseMethod)
            {
                case CutscenePauseMethod.Loop:
                    var start = 0f;
                    var end = (float)playable.GetDuration();  // end를 playable의 duration 값으로 설정
                    Debug.Log($"Looping from start: {start} to end: {end}");

                    if (Cutscene.ActiveCutscene != null)
                    {
                        Cutscene.ActiveCutscene.Loop(start, end, withOffset: false);
                        Debug.Log("Cutscene loop activated.");
                    }
                    else
                    {
                        Debug.LogWarning("Cutscene.ActiveCutscene is null.");
                    }
                    break;

                case CutscenePauseMethod.Pause:
                    if (Cutscene.ActiveCutscene != null)
                    {
                        Cutscene.ActiveCutscene.Pause();
                        Debug.Log("Cutscene paused.");
                    }
                    else
                    {
                        Debug.LogWarning("Cutscene.ActiveCutscene is null.");
                    }
                    break;

                default:
                    Debug.LogWarning("Unknown pause method.");
                    break;
            }

            firstFrame = false;  // 첫 프레임이 실행된 후에는 false로 설정
        }
    }
}
