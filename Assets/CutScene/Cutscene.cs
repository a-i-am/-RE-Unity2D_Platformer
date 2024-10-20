using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Playables;
using System;
using UltEvents;

public class Cutscene : MonoBehaviour
{
[System.Serializable]
public class CutscenePart
{
     public PlayableDirector playableDir;
     public DirectorWrapMode wrapMode = DirectorWrapMode.None;
     public bool continueDialogue = false;
     public bool waitForFixedUpdateAfterStop = false;
}

[System.Serializable]
public class DialougeArea
{
    [TextArea]
    public string dialogue;
    public Sprite actorIcon;
    //public Sprite iconBack;
    public string actorName;
    public string title;
}
    // UltEvent
    public UltEvent<Cutscene> OnStart; // UltEvent is similar to UnityEvent but with more features
    public UltEvent<Cutscene> OnStop;
    public UltEvent<Cutscene> OnComplete;

    [SerializeField] private DialougeArea[] dialougeArea;
    [SerializeField] private Image image_ActorIcon;
    [SerializeField] private Image image_IconBack;
    [SerializeField] private Image image_DialougeBox;
    [SerializeField] private TextMeshProUGUI txt_TitleText;
    [SerializeField] private TextMeshProUGUI txt_ActorName;
    [SerializeField] private TextMeshProUGUI txt_Dialogue;
    private bool isDialogue = false;
    private int count = 0;

    // 컷신-타임라인 루프 연결
    public static Cutscene ActiveCutscene;
    public List<CutscenePart> cutsceneParts = new List<CutscenePart>();
    Tuple<float, float> loop;
    
    //Property

    //읽기 전용 프로퍼티
    //  현재 컷신의 실행 중 여부 확인

    public bool IsCurrentCutScene
    {
        get
        {
            return playableDir.playableGraph.IsValid() && playableDir.playableGraph.IsPlaying(); ;
        }
    }

    [SerializeField] private PlayableDirector playableDir;
    public virtual void Awake()
    {
        playableDir = GetComponent<PlayableDirector>();
        playableDir.played += (dir) =>
        {
            if (dir == playableDir)
                ActiveCutscene = this;
        };
    }

    void FixedUpdate()
    {
        if(loop != null && playableDir.time > loop.Item2){
            playableDir.time = loop.Item1;
        }    
    }

    public void Loop(float start, float end, bool withOffset = true)
    {
        start = withOffset ? start : start + (float)playableDir.time;
        end = withOffset ? end : end + (float)playableDir.time;
        loop = new Tuple<float, float>(start, end);
    }

    void Start()
    {
        OnOff(false);
    }
    void Update()
    {
        if (isDialogue)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                if (count < dialougeArea.Length)
                    NextDialogue();
                else
                    OnOff(false);

            }
        }
    }

    // 대화창 띄움/넘김
    public void ShowDialogue()
    {
        OnOff(true);
        count = 0;
        isDialogue = true;
        NextDialogue();
    }
    private void NextDialogue()
    {
        txt_Dialogue.text = dialougeArea[count].dialogue;
        txt_TitleText.text = dialougeArea[count].title;
        txt_ActorName.text = dialougeArea[count].actorName;

        if (string.IsNullOrEmpty(dialougeArea[count].title))
        {
            image_ActorIcon.sprite = dialougeArea[count].actorIcon;
            image_IconBack.gameObject.SetActive(true);
            image_ActorIcon.gameObject.SetActive(true);
        }
        else
        {
            image_ActorIcon.gameObject.SetActive(false);
            image_IconBack.gameObject.SetActive(false);
        }
        count++;
    }
    private void OnOff(bool _flag)
    {
        image_DialougeBox.gameObject.SetActive(_flag);
        image_ActorIcon.gameObject.SetActive(_flag);
        image_IconBack.gameObject.SetActive(_flag);
        txt_Dialogue.gameObject.SetActive(_flag);
        txt_TitleText.gameObject.SetActive(_flag);
        txt_ActorName.gameObject.SetActive(_flag);
        isDialogue = _flag;
    }

// 타임라인 

    // 타임라인 일시정지
    public void Pause()
    {
        playableDir.playableGraph.GetRootPlayable(0).SetSpeed(0);
    }

    //  타임라인의 루프(구간 반복) 중지
    public void EndLoop()
    {
        loop = null;
    }

    // 타임라인 이어서 재생 (처음부터 재생 x, 멈춘 부분부터 재생)
    public void Continue()
    {
        playableDir.playableGraph.GetRootPlayable(0).SetSpeed(1);
        EndLoop();
    }

    // 타임라인 재생 종료(초기화)
    public void ForceEnd()
    {
        if (IsCurrentCutScene)
        {
            playableDir.playableGraph.Stop();
        }
    }


}
