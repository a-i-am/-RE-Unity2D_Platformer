using Assets;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public delegate void GameOverHandler(); // = delegate void = Action 
    public event GameOverHandler gameOverDele;
    private bool isGameOver = false;

    private List<IUpdatable> updatables = new List<IUpdatable>();
    private List<IFixedUpdatable> fixedUpdatables = new List<IFixedUpdatable>();

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(Replay);
        PlayerScr player = FindObjectOfType<PlayerScr>();
        if (gameOverDele != null)
        {
            // isDead = true;
            isGameOver = true;
        }
    }
    private void Update()
    {
        foreach (var updatable in updatables)
        {
            updatable.OnUpdate();
        }
    }

    private void FixedUpdate()
    {
        foreach (var fixedUpdatable in fixedUpdatables)
        {
            fixedUpdatable.OnFixedUpdate();
        }
    }

    public Action keyAction = null;
    public void OnUpdate()
    {
        if (Input.anyKey == false) return;

        if (keyAction != null)
        {
            keyAction.Invoke();
        }
    }

    public void RegisterUpdatable(IUpdatable updatable)
    {
        updatables.Add(updatable);
    }

    public void RegisterFixedUpdatable(IFixedUpdatable fixedUpdatable)
    {
        fixedUpdatables.Add(fixedUpdatable);
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
        if (isGameOver)
        {
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
    //    public void TabClick(string tabName)
    //    {
    //    }

    //    public void SendMessageToChat(string text)
    //    {
    //        if (messageList.Count >= maxMessages)
    //            messageList.Remove(messageList[0]);

    //        Message newMessage = new Message();

    //        newMessage.text = text;

    //        messageList.Add(newMessage);
    //    }

    //    [System.Serializable]
    //    public class Message
    //    {
    //        public string text;
    //    }
}
