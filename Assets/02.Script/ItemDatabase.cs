using System.Collections.Generic;
using UnityEngine;
using Newtonsoft;
using Newtonsoft.Json;
using System.IO;
using UnityEngine.UI;

[System.Serializable] // ������ ����ȭ(����Ʈ ������� Inspector�� ǥ������)
public class Character
{
    public Character(string _type, string _name, string _explain, string _number, bool _isUsing, string _tabName = "Character")
    { tabName = _tabName; type = _type; name = _name; explain = _explain; number = _number; isUsing = _isUsing; }

    public string tabName, type, name, explain, number; // string�̾�� JSON �Ľ� �� �� �ȴٰ� ��
    public bool isUsing;
}

[System.Serializable] // ������ ����ȭ(����Ʈ ������� Inspector�� ǥ������)
public class Item
{
    public Item(string _type, string _name, string _explain, string _number, bool _isUsing, string _tabName = "Item")
    { tabName = _tabName; type = _type; name = _name; explain = _explain; number = _number; isUsing = _isUsing; }

    public string tabName, type, name, explain, number; // string�̾�� JSON �Ľ� �� �� �ȴٰ� ��
    public bool isUsing;

}



public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase instance;
    public TextAsset itemDBText, characterDBText;
    //public TextAsset ; // CharacterDatabase
    public List<Character> allCharacterList, myCharacterList, curCharacterList;
    public List<Item> allItemList, myItemList, curItemList;
    public string curType = "Character";
    public GameObject[] InvenSlot, ItemInfo, CharacterInfo;
    public Image[] tabImage;
    public Sprite tabIdleSprite, tabSelectSprite;

    void Start()
    {
        // ���� �� ������ �����ϰ� �ؽ�Ʈ�� ����
        // ���๮��(\n)�� �����ڷ� ������ ����
        string[] characterLine = characterDBText.text.Substring(0, characterDBText.text.Length - 1).Split('\n');
        //print(line.Length);
        for (int i = 0; i < characterLine.Length; i++)
        {
            string[] row = characterLine[i].Split('\t');
            allCharacterList.Add(new Character(row[0], row[1], row[2], row[3], row[4] == "TRUE"));
        }

        string[] itemLine = itemDBText.text.Substring(0, itemDBText.text.Length - 1).Split('\n');
        //print(line.Length);
        for (int i = 0; i < itemLine.Length; i++)
        {
            string[] row = itemLine[i].Split('\t');
            allItemList.Add(new Item(row[0], row[1], row[2], row[3], row[4] == "TRUE"));
        }

        CharacterSave();
        CharacterLoad();
        ItemSave();
        ItemLoad();
    }

    private void Awake()
    {
        instance = this;
    }

    public void TabClick(string tabName)
    {
        // ���� ������ ����Ʈ�� Ŭ���� Ÿ�Ը� �߰�
        curType = tabName;
        curItemList = myItemList.FindAll(x => x.type == tabName);
        curCharacterList = myCharacterList.FindAll(x => x.type == tabName);

        // Character ����Ʈ ���԰� �ؽ�Ʈ ���̱�
        for (int i_Character = 0; i_Character < InvenSlot.Length; i_Character++)
        {
            InvenSlot[i_Character].SetActive(i_Character < curCharacterList.Count);

            //ItemInfo[i_Character].GetComponentInChildren<Text>().text = i < curItemList.Count ? curItemList[i].Name : "";
        }

        // Item ����Ʈ ���԰� �ؽ�Ʈ ���̱�
        for (int i_item = 0; i_item < InvenSlot.Length; i_item++)
        {
            InvenSlot[i_item].SetActive(i_item < curCharacterList.Count);

            //ItemInfo[i_item].GetComponentInChildren<Text>().text = i < curItemList.Count ? curItemList[i].Name : "";
        }

        int tabNum = 0;
        switch (tabName)
        {
            case "Character": tabNum = 0; break;
            case "Item": tabNum = 1; break;
            default: tabNum = 2; break; // case���ǵ鿡 ���� �ش����� �ʴ� ���
        }
        
        for (int i = 0; i < tabImage.Length; i++)
            tabImage[i].sprite = i == tabNum ? tabSelectSprite : tabIdleSprite;
    }

    void CharacterSave()
    {
        string jdata = JsonConvert.SerializeObject(allCharacterList); // JSON���� ����Ʈ�� string���� ��ȯ
        //print(jdata);
        File.WriteAllText(Application.dataPath + "/07.Resources/MyCharacterText.txt", jdata);
        //print(Application.dataPath);
    }

    void CharacterLoad()
    {
        string jdata = File.ReadAllText(Application.dataPath + "/07.Resources/MyCharacterText.txt");
        myCharacterList = JsonConvert.DeserializeObject<List<Character>>(jdata);
    }

    void ItemSave()
    {
        string jdata = JsonConvert.SerializeObject(allItemList); // JSON���� ����Ʈ�� string���� ��ȯ
        //print(jdata);
        File.WriteAllText(Application.dataPath + "/07.Resources/MyItemText.txt", jdata);
        //print(Application.dataPath);
    }

    void ItemLoad()
    {
        string jdata = File.ReadAllText(Application.dataPath + "/07.Resources/MyItemText.txt");
        myItemList = JsonConvert.DeserializeObject<List<Item>>(jdata);

        TabClick(curType);
    }


}
