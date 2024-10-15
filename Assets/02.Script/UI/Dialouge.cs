using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class DialougeArea 
{
    [TextArea]
    public string dialogue;
    public Sprite actorIcon;
    public string actorName;   
    public string title;
}

public class Dialouge : MonoBehaviour
{
    [SerializeField] private Image image_ActorIcon;
    [SerializeField] private Image image_DialougeBox;
    [SerializeField] private TextMeshProUGUI txt_TitleText;
    [SerializeField] private TextMeshProUGUI txt_ActorName;
    [SerializeField] private TextMeshProUGUI txt_Dialogue;
    private bool isDialogue = false;
    private int count = 0;

    [SerializeField] private DialougeArea[] dialougeArea;

    public void ShowDialogue()
    {
        OnOff(true);
        count = 0;
        isDialogue = true;
        NextDialogue();
    }

    private void OnOff(bool _flag)
    {
        image_DialougeBox.gameObject.SetActive(_flag);
        image_ActorIcon.gameObject.SetActive(_flag);
        txt_Dialogue.gameObject.SetActive(_flag);
        txt_TitleText.gameObject.SetActive(_flag);
        txt_ActorName.gameObject.SetActive(_flag);  
        isDialogue = _flag;
    }


    private void NextDialogue()
    {
        txt_Dialogue.text = dialougeArea[count].dialogue;
        image_ActorIcon.sprite = dialougeArea[count].actorIcon;
        txt_TitleText.text = dialougeArea[count].title;
        txt_ActorName.text = dialougeArea[count].actorName;
        count++;
    }

    void Start()
    {
        
    }

    void Update()
    {
        if(isDialogue)
        {
            if(Input.GetKeyDown(KeyCode.Space)) 
            {
                if (count < dialougeArea.Length)
                    NextDialogue();
                else
                    OnOff(false);

            }
        }
    }
}
