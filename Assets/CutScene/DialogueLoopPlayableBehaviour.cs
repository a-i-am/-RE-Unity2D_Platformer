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
            switch (pauseMethod)
            {
                case CutscenePauseMethod.Loop:
                    var start = 0;
                    var end = start + playable.GetDuration();
                    Cutscene.ActiveCutscene?.Loop((float)start, (float)end, withOffset: false);
                    break;
                case CutscenePauseMethod.Pause:
                    Cutscene.ActiveCutscene?.Pause();
                    break;
                default:
                    break;
            }
            //DialoguePopup.instance?.TryToContinue();
            firstFrame = false;
        }
    }
}