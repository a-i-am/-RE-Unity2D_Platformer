using System.Collections.Generic;
using UnityEngine;
using Newtonsoft;
using Newtonsoft.Json;
using System.IO;
using UnityEngine.UI;

public class ItemDatabase : MonoBehaviour
{
    public GameObject chInvenSlotUI;
    public GameObject itemInvenSlotUI;

    public static ItemDatabase instance;
    public TextAsset itemDBText, characterDBText;

    public List<Item.CharacterData> allCharacterList = new List<Item.CharacterData>();
    public List<Item.CharacterData> myCharacterList = new List<Item.CharacterData>();
    public List<Item.CharacterData> curCharacterList = new List<Item.CharacterData>();

    public List<Item.ItemData> allItemList = new List<Item.ItemData>();
    public List<Item.ItemData> myItemList = new List<Item.ItemData>();
    public List<Item.ItemData> curItemList = new List<Item.ItemData>();

    public string curType = "Character";
    public GameObject[] InvenSlot, ItemInfo, CharacterInfo;
    public Image[] tabImage;
    public Sprite tabIdleSprite, tabSelectSprite;

    public List<Item.ItemData> itemDB = new List<Item.ItemData>();
    [Space(25)]
    public GameObject fieldItemPrefab;
    public Vector3[] pos;

    void Start()
    {
        // �ʵ忡 ������ ����Ʈ �� ���� ����
        for (int i = 0; i < 6; i++)
        {
            GameObject go = Instantiate(fieldItemPrefab, pos[i], Quaternion.identity);
            //go.GetComponent<FieldItems>().SetItem(itemDB[Random.Range(0, 5)]);
            FieldItems fieldItems = go.GetComponent<FieldItems>();
            fieldItems.SetRandomItem();
        }
        // ���� �� ������ �����ϰ� �ؽ�Ʈ�� ����
        // ���๮��(\n)�� �����ڷ� ������ ����
        string[] characterLine = characterDBText.text.Substring(0, characterDBText.text.Length - 1).Split('\n');
        //print(line.Length);
        for (int i = 0; i < characterLine.Length; i++)
        {
            string[] row = characterLine[i].Split('\t');
            allCharacterList.Add(new Item.CharacterData(row[0], row[1], row[2], row[3], row[4] == "TRUE"));
        }

        string[] itemLine = itemDBText.text.Substring(0, itemDBText.text.Length - 1).Split('\n');
        //print(line.Length);
        for (int i = 0; i < itemLine.Length; i++)
        {
            string[] row = itemLine[i].Split('\t');
            allItemList.Add(new Item.ItemData(row[0], row[1], row[2], row[3], row[4] == "TRUE"));
        }

        //SyncAllItemListWithItemDB();

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


        switch (tabNum)
        {
            case 0:
                chInvenSlotUI.SetActive(true);
                itemInvenSlotUI.SetActive(false);
                break;
            case 1:
                chInvenSlotUI.SetActive(false);
                itemInvenSlotUI.SetActive(true);
                break;
            default:
                chInvenSlotUI.SetActive(false);
                itemInvenSlotUI.SetActive(false);
                break;
        }

    }


    //void SyncAllItemListWithItemDB()
    //{
    //    itemDB.Clear();
    //    itemDB.AddRange(allItemList);
    //}

    void CharacterSave()
    {
        string jdata = JsonConvert.SerializeObject(allCharacterList, Formatting.Indented); // JSON���� ����Ʈ�� string���� ��ȯ
        //print(jdata);
        File.WriteAllText(Application.dataPath + "/07.Resources/MyCharacterText.txt", jdata);
        //print(Application.dataPath);
    }

    void CharacterLoad()
    {
        string jdata = File.ReadAllText(Application.dataPath + "/07.Resources/MyCharacterText.txt");
        myCharacterList = JsonConvert.DeserializeObject<List<Item.CharacterData>>(jdata);
        TabClick(curType); // �� ó�� ���õǾ��ִ� ���� ��(ĳ������)
    }

    void ItemSave()
    {
        string jdata = JsonConvert.SerializeObject(allItemList, Formatting.Indented); // JSON���� ����Ʈ�� string���� ��ȯ
        //print(jdata);
        File.WriteAllText(Application.dataPath + "/07.Resources/MyItemText.txt", jdata);
        //print(Application.dataPath);
    }

    void ItemLoad()
    {
        string jdata = File.ReadAllText(Application.dataPath + "/07.Resources/MyItemText.txt");
        myItemList = JsonConvert.DeserializeObject<List<Item.ItemData>>(jdata);
    }


}
