using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Playables;
using System;
using UltEvents;
using UnityEngine.Timeline;

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
        public string actorName;
        public string title;
    }

    // UltEvent
    public UltEvent<Cutscene> OnStart;
    public UltEvent<Cutscene> OnStop;
    public UltEvent<Cutscene> OnComplete;
    bool isResumeClip;
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

    [SerializeField] private PlayableDirector playableDir;
    public TimelineAsset timeline;

    void Start()
    {
        OnOff(false);

        // pause
        GetComponent<PlayableDirector>().playableGraph.GetRootPlayable(0).SetSpeed(0);
        // play
        GetComponent<PlayableDirector>().playableGraph.GetRootPlayable(0).SetSpeed(1);
    }
    public virtual void Awake()
    {
        playableDir = GetComponent<PlayableDirector>();

        ActiveCutscene = this;

        playableDir.played += (dir) =>
        {
            if (dir == playableDir)
            {
                //ActiveCutscene = this;
                Debug.Log("PlayableDirector 재생됨: ActiveCutscene 할당 완료");
            }
        };

        playableDir.stopped += (dir) =>
        {
            if (dir == playableDir)
            {
                Debug.Log("PlayableDirector 중지됨");
            }
        };

        // PlayableGraph의 상태를 확인
        if (!playableDir.playableGraph.IsValid() || !playableDir.playableGraph.IsPlaying())
        {
            playableDir.Play(); // PlayableDirector를 재생하여 ActiveCutscene 설정
        }
    }

    void FixedUpdate()
    {
        if (loop != null && playableDir.time > loop.Item2)
        {
            playableDir.time = loop.Item1;
            Debug.Log($"타임라인 루프 중: {loop.Item1}초에서 {loop.Item2}초로 돌아감");
        }
    }

    public void Loop(float start, float end, bool withOffset = true)
    {
        start = withOffset ? start : start + (float)playableDir.time;
        end = withOffset ? end : end + (float)playableDir.time;
        loop = new Tuple<float, float>(start, end);
        Debug.Log($"타임라인 루프 시작: 시작 {start}초, 끝 {end}초");
    }


    void Update()
    {
        if (isDialogue)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (count < dialougeArea.Length)
                {
                    Continue();
                    NextDialogue();
                }
                else
                {
                    OnOff(false);
                    SceneController.Instance.NextLevel();
                }
            }
        }
    }




    public void ShowDialogue()
    {
        OnOff(true);
        DialogueImageActvieCheck();
        count = 1;
        isDialogue = true;
        Pause();
    }

    public void NextDialogue()
    {
        ResumeFromClosestClip();
        if (isResumeClip)
        {
            if (count < dialougeArea.Length)
            {
                txt_Dialogue.text = dialougeArea[count].dialogue;
                txt_TitleText.text = dialougeArea[count].title;
                txt_ActorName.text = dialougeArea[count].actorName;

                DialogueImageActvieCheck();
                count++;
            }
            else
            {
                OnOff(false);  // 대화가 끝났을 때
            }
        }

    }


    private void DialogueImageActvieCheck()
    {
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
        // actorIcon이 null인 경우와 아닌 경우에 따라 활성화/비활성화 처리
        if (dialougeArea[count].actorIcon != null)
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



    // 타임라인에서 가장 가까운 애니메이션 클립으로 이동해서 이어서 재생
    public void ResumeFromClosestClip()
    {
        isResumeClip = true;
        double currentTime = playableDir.time;
        TimelineClip closestClip = null;
        double closestStartTime = double.MaxValue;

        // 타임라인에 있는 트랙을 순회하며 가장 가까운 애니메이션 클립을 찾음
        foreach (var track in timeline.GetOutputTracks())
        {
            foreach (TimelineClip clip in track.GetClips())
            {
                // 현재 시간보다 뒤에 있는 클립 중 가장 가까운 것을 찾음
                if (clip.start > currentTime && clip.start < closestStartTime)
                {
                    closestClip = clip;
                    closestStartTime = clip.start;
                }
            }
        }

        if (closestClip != null)
        {
            Debug.Log($"Resuming from closest clip at time {closestClip.start}");
            playableDir.time = closestClip.start; // 가장 가까운 클립의 시작점으로 타임라인 시간을 설정
            playableDir.Play(); // 타임라인 재생
        }
        else
        {
            Debug.Log("No upcoming clip found.");
        }
    }









    // 타임라인 관련 메서드들
    public void Pause()
    {
        playableDir.playableGraph.GetRootPlayable(0).SetSpeed(0);
        Debug.Log("PlayableDirector 일시정지됨");
    }

    public void EndLoop()
    {
        loop = null;
        Debug.Log("타임라인 루프 중지");
    }

    public void Continue()
    {
        playableDir.playableGraph.GetRootPlayable(0).SetSpeed(1);
        EndLoop();
        Debug.Log("타임라인 이어서 재생됨");
    }

    public void ForceEnd()
    {
        if (IsCurrentCutScene)
        {
            playableDir.playableGraph.Stop();
            Debug.Log("타임라인 강제 종료됨");
        }
    }

    public bool IsCurrentCutScene
    {
        get
        {
            return playableDir.playableGraph.IsValid() && playableDir.playableGraph.IsPlaying();
        }
    }
}
