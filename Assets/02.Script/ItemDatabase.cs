using System.Collections.Generic;
using UnityEngine;
using Newtonsoft;
using Newtonsoft.Json;
using System.IO;
using UnityEngine.UI;

[System.Serializable] // ������ ����ȭ(������� Inspector�� ǥ������)
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
    public TextAsset itemDBText; // ItemDatabase
    public List<Item> allItemList, myItemList, curItemList;
    public string curType = "Item";
    public GameObject[] InvenSlot, ItemInfo;
    public Image[] tabImage;
    public Sprite tabIdleSprite, tabSelectSprite;



    void Start()
    {
        // ���� �� ������ �����ϰ� �ؽ�Ʈ�� ����
        // ���๮��(\n)�� �����ڷ� ������ ����
        string[] line = itemDBText.text.Substring(0, itemDBText.text.Length - 1).Split('\n');
        //print(line.Length);
        for (int i = 0; i < line.Length; i++)
        {
            string[] row = line[i].Split('\t');
            allItemList.Add(new Item(row[0], row[1], row[2], row[3], row[4] == "TRUE"));
        }
        Save();
        Load();
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
        
        // ���԰� �ؽ�Ʈ ���̱�
        for (int i = 0; i < InvenSlot.Length; i++)
        {
            InvenSlot[i].SetActive(i < curItemList.Count);
            //ItemInfo[i].GetComponentInChildren<Text>().text = i < curItemList.Count ? curItemList[i].Name : "";
        }

        int tabNum = 0;
        switch (tabName)
        {
            case "Character": tabNum = 0; break;
            case "Item": tabNum = 1; break;
        }
        for (int i = 0; i < tabImage.Length; i++)
            tabImage[i].sprite = i == tabNum ? tabSelectSprite : tabIdleSprite;
    }
    void Save()
    {
        string jdata = JsonConvert.SerializeObject(allItemList); // JSON���� ����Ʈ�� string���� ��ȯ
        //print(jdata);
        File.WriteAllText(Application.dataPath + "/07.Resources/MyItemText.txt", jdata);
        //print(Application.dataPath);
    }

    void Load()
    {
        string jdata = File.ReadAllText(Application.dataPath + "/07.Resources/MyItemText.txt");
        myItemList = JsonConvert.DeserializeObject<List<Item>>(jdata);

        TabClick(curType);
    }
}
