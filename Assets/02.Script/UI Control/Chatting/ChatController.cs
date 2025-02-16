using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ChatController : MonoBehaviour
{
    private ChatType currentInputType;     // 현재 대화 입력 속성 (Normal, Party, Guild) 
    private ChatType currentViewType;      // 현재 대화 보기 속성 (Normal, Party, Guild, Whisper, System)

    //private string lastChatData = "";   // 마지막에 작성한 대화 내용
    //private string lastWhisperID = "";  // 마지막에 귓말을 보낸 대상

    //private string ID = "AIAM";         // 본인 아이디 (임시)
    //private string friendID = "U"; // 친구 아이디 (임시)

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

    public void UpdateChatWithCommand(string chat)
    {
        if (!chat.StartsWith('/'))
        {
            lastChatData = chat;
            PrintChatData(currentInputType, currentTextColor, lastChatData);
            return;
        }

        if (chat.StartsWith("/re"))
        {
            if (lastChatData.Equals(""))
            {
                inputField.text = "";
                return;
            }
            UpdateChatWithCommand(lastChatData);
        }
        else if (chat.StartsWith("/w "))
        {
            lastChatData = chat;

            string[] whisper = chat.Split(' ', 3);

            if (whisper[1] == friendID)
            {
                lastWhisperID = whisper[1];
                PrintChatData(ChatType.Whisper, ChatTypeToColor(ChatType.Whisper), $"[to {whisper[1]}] {whisper[2]}");
            }
            else
            {
                PrintChatData(ChatType.System, ChatTypeToColor(ChatType.System), $"Do not find [{whisper[1]}]");
            }
        }
        else if (chat.StartsWith("/r "))
        {
            if (lastWhisperID.Equals(""))
            {
                inputField.text = "";
                return;
            }

            lastChatData = chat;

            string[] whisper = chat.Split(' ', 2);

            PrintChatData(ChatType.Whisper, ChatTypeToColor(ChatType.Whisper), $"[to {lastWhisperID}] {whisper[1]}");
        }
    }
}