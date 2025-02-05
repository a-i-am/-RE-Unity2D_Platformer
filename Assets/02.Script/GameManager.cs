using Assets;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    //public int maxMessages = 25;
    //public GameObject ChatPanel, textObject;

    [SerializeField]
    //List<Message> messageList = new List<Message>();

    // 싱글톤 인스턴스
    private static GameManager _instance;
    // 델리게이트 선언
    public delegate void GameOverHandler(); // = delegate void = Action 
    // 게임 오버 핸들러를 가리키는 델리게이트 인스턴스
    public event GameOverHandler gameOverDele;
    // 델리게이트 호출 메소드
    private bool isGameOver = false;

    // 싱글톤 인스턴스 반환
    public static GameManager Instance
    {
        get {
            #region 싱글톤 인스턴스 할당
        if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
                // _instance = this;
                DontDestroyOnLoad(_instance.gameObject.transform.root.gameObject);
                // 입력 매개변수가 0개 
                //GameManager.Instance.gameOverDele += () => { GameManager.Instance.isDead = true; };
                GameManager.Instance.gameOverDele += () => { GameManager.Instance.isGameOver = true; };
            }
            return _instance;
            #endregion
        }
    }

    // Start is called before the first frame update
    void Start() {
        GetComponent<Button>().onClick.AddListener(Replay);
        PlayerScr player = FindObjectOfType<PlayerScr>();
        if (gameOverDele != null) {
            // isDead = true;
            isGameOver = true;
        }
    }
    
    void Update() 
    {
    }

    // 게임 오버 & 리플레이 로직(델리게이트)
    public void GameOverDeath()
    {
        // 게임 오버 조건(목숨 0) -> gameOverDele delegate => 게임 오버 상태 true
        // 게임 오버시 나타나야할 것(화면 전환, 오디오)
        // 1) 게임오버 창
        SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
        // 2) 
    }
    public void OnDeath()
    {
        // gameOverDele 델리게이트 호출
        // 캐릭터 입력 차단

        gameOverDele?.Invoke();
        // GameOverDeath() : 목숨 0, isGameOver true, UI, BGM
        if (isGameOver) {
        ///
        }
        //else // RespawnDeath() : 리스폰, 목숨 깎임
        //Time.deltaTime = 0;
    }
    void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // = SceneManager.LoadScene("2D Scene");
        // 게임 새 시작시 isGameOver false
        isGameOver = false;
    }
    //public void TabClick(string tabName)
    //{
    //}

    //public void SendMessageToChat(string text)
    //{
    //    if (messageList.Count >= maxMessages)
    //        messageList.Remove(messageList[0]);

    //    Message newMessage = new Message();

    //    newMessage.text = text;
        
    //    messageList.Add(newMessage);
    //}

    //[System.Serializable]
    //public class Message
    //{
    //    public string text;
    //}
}
