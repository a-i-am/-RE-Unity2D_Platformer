using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatUIController : MonoBehaviour
{
    [SerializeField] private GameObject textChatprefab;     // 대화를 출력하는 Text UI 프리팹
    [SerializeField] private Transform parentContent;       // 대화가 출력되는 ScrollView의 Content
    [SerializeField] private TMP_InputField inputField;     // 대화 입력창
    [SerializeField] private Sprite[] spriteChatInputType;  // 대화 입력 속성 버튼에 적용할 이미지 에셋
    [SerializeField] private Image imageChatInputType;      // 대화 입력 속성 버튼의 이미지 버튼
    [SerializeField] private TextMeshProUGUI textInput;     // 대화 입력 속성에 따라 대화 입력창에 작성되는 텍스트 색상 변경

    private ChatType currentInputType = ChatType.Normal;    // 현재 대화 입력 속성 (Normal, Party, Guild) 
    private ChatType currentViewType;                       // 현재 대화 보기 속성 (Normal, Party, Guild, Whisper, System)
    private Color currentTextColor = Color.white;
    private List<ChatCell> chatList = new List<ChatCell>(); // 대화창에 출력되는 모든 대화를 보관하는 리스트
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && inputField.isFocused == false)
        {
            inputField.ActivateInputField();
        }

        if (Input.GetKeyDown(KeyCode.Tab) && inputField.isFocused == true)
        {
            SetCurrentInputType();
        }
    }
    public void SetCurrentInputType()
    {
        currentInputType = (int)currentInputType < (int)ChatType.Count - 3 ? currentInputType + 1 : 0;
        imageChatInputType.sprite = spriteChatInputType[(int)currentInputType];
        currentTextColor = ChatTypeToColor(currentInputType);
        textInput.color = currentTextColor == Color.white ? Color.black : currentTextColor;
    }

    public void SetCurrentViewType(int newType)
    {
        currentViewType = (ChatType)newType;

        if (currentViewType == ChatType.Normal)
        {
            for (int i = 0; i < chatList.Count; ++i)
            {
                chatList[i].gameObject.SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < chatList.Count; ++i)
            {
                chatList[i].gameObject.SetActive(chatList[i].ChatType == currentViewType);
            }
        }
    }



    public void OnEndEditEventMethod()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            UpdateChat();
        }
    }

    public void UpdateChat()
    {
        if (string.IsNullOrEmpty(inputField.text)) return;

        ChatNetworkManager.Instance?.SendMessageToServer(inputField.text);

        UpdateChatWithCommand(inputField.text);

        ChatDataManager.Instance?.AddChat(inputField.text);
        PrintChatData(currentInputType, currentTextColor, inputField.text);
        inputField.text = "";
    }

    public void UpdateChatWithCommand(string chat)
    {
        if (!chat.StartsWith('/'))
        {
            ChatDataManager.Instance?.AddChat(chat);
            PrintChatData(currentInputType, currentTextColor, chat);
            return;
        }

        if (chat.StartsWith("/re"))
        {
            if (string.IsNullOrEmpty(ChatDataManager.Instance?.LastChat))
            {
                inputField.text = "";
                return;
            }
            UpdateChatWithCommand(chat);
        }
        else if (chat.StartsWith("/w "))
        {
            string[] whisper = chat.Split(' ', 3);

            if (whisper[1] == ChatDataManager.Instance?.LastWhisperTarget)
            {
                PrintChatData(ChatType.Whisper, ChatTypeToColor(ChatType.Whisper), $"[to {whisper[1]}] {whisper[2]}");
            }
            else
            {
                PrintChatData(ChatType.System, ChatTypeToColor(ChatType.System), $"Do not find [{whisper[1]}]");
            }
        }
        else if (chat.StartsWith("/r "))
        {
            if (string.IsNullOrEmpty(ChatDataManager.Instance?.LastWhisperTarget))
            {
                inputField.text = "";
                return;
            }

            string[] whisper = chat.Split(' ', 2);
            PrintChatData(ChatType.Whisper, ChatTypeToColor(ChatType.Whisper), $"[to {ChatDataManager.Instance?.LastWhisperTarget}] {whisper[1]}");
        }
    }

    private void PrintChatData(ChatType type, Color color, string text)
    {
        if (string.IsNullOrEmpty(text)) return;
        GameObject clone = Instantiate(textChatprefab, parentContent);
        ChatCell cell = clone.GetComponent<ChatCell>();

        cell.Setup(type, color, text);
        chatList.Add(cell);
    }
    private Color ChatTypeToColor(ChatType type)
    {
        Color[] colors = new Color[(int)ChatType.Count] {
            Color.white, Color.blue, Color.green, Color.magenta, Color.yellow };

        return colors[(int)type];
    }
}
