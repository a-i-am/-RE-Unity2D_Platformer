using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatDataManager : Singleton<ChatDataManager>
{
    private List<string> chatHistory = new List<string>();
    public string LastChat { get; private set; } = "";
    public string LastWhisperTarget { get; private set; } = "";

    public void AddChat(string chat)
    {
        chatHistory.Add(chat);
        LastChat = chat;
    }

    public void SetLastWhisperTarget(string target)
    {
        LastWhisperTarget = target;
    }
}
