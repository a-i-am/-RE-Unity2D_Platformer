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
    [SerializeField] private Image imageChatInputType;      // 대화 입력 속성 버튼의 이미지
    [SerializeField] private TextMeshProUGUI textInput;     // 대화 입력 속성에 따라 대화 입력창에 작성되는 텍스트 색상 변경

    private ChatType currentInputType = ChatType.Normal;    // 현재 대화 입력 속성 (Normal, Party, Guild) 
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

        ChatNetworkManager.Instance.SendMessageToServer(inputField.text);

        PrintChatData(currentInputType, currentTextColor, inputField.text);
        inputField.text = "";

        UpdateChatWithCommand(inputField.text);
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
