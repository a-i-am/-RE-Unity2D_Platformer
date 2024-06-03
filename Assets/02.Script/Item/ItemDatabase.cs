using System.Collections.Generic;
using UnityEngine;
using Newtonsoft;
using Newtonsoft.Json;
using System.IO;
using UnityEngine.UI;

public class ItemDatabase : MonoBehaviour
{


    public GameObject characterInvenSlotUI;
    public GameObject itemInvenSlotUI;
    public GameObject characterSlotNumText;
    public GameObject itemSlotNumText;

    public static ItemDatabase instance;
    public TextAsset itemDBText, characterDBText;

    public List<Character.CharacterData> allCharacterList = new List<Character.CharacterData>();
    public List<Character.CharacterData> myCharacterList = new List<Character.CharacterData>();
    public List<Character.CharacterData> curCharacterList = new List<Character.CharacterData>();

    public List<Item.ItemData> allItemList = new List<Item.ItemData>();
    public List<Item.ItemData> myItemList = new List<Item.ItemData>();
    public List<Item.ItemData> curItemList = new List<Item.ItemData>();

    public string curType = "Character";
    public GameObject[] InvenSlot, ItemInfo, CharacterInfo;
    public Image[] tabImage;
    public Sprite tabIdleSprite, tabSelectSprite;

    public List<Item.ItemData> itemDB = new List<Item.ItemData>();
    public List<Character.CharacterData> characterDB = new List<Character.CharacterData>();

    [Space(25)]
    public GameObject fieldItemPrefab;
    public Vector3[] pos;

    void Start()
    {
        // �ʵ忡 ������ ����Ʈ �� ���� ����
        for (int i = 0; i < 7; i++) // ������ ������ ������ŭ �ݺ� // �ν����Ϳ� Pos ���� �� ���� ��ġ �ۼ� �ʿ�
        {
            GameObject go = Instantiate(fieldItemPrefab, pos[i], Quaternion.identity);
            FieldItems fieldItems = go.GetComponent<FieldItems>();
            fieldItems.SetRandomItem();
        }

        // ���� �� ������ �����ϰ� �ؽ�Ʈ�� ����
        // ���๮��(\n)�� �����ڷ� ������ ����
        string[] characterLine = characterDBText.text.Substring(0, characterDBText.text.Length - 1).Split('\n');
        for (int i = 0; i < characterLine.Length; i++)
        {
            string[] row = characterLine[i].Split('\t');
            Sprite characterImage = Resources.Load<Sprite>(row[5]); // ĳ���� �̹��� ��ο��� ��������Ʈ �ε�
            GameObject characterPrefab = Resources.Load<GameObject>(row[6]); // ĳ���� ������ ��ο��� �ε�

            allCharacterList.Add(new Character.CharacterData(
                row[0],  // type
                row[1],  // name
                row[2],  // explain
                row[3],  // number
                row[4] == "TRUE",  // isUsing
                characterImage,  // characterImage
                characterPrefab  // characterPrefab
            ));
        }

        string[] itemLine = itemDBText.text.Substring(0, itemDBText.text.Length - 1).Split('\n');
        //print(line.Length);
        for (int i = 0; i < itemLine.Length; i++)
        {
            string[] row = itemLine[i].Split('\t');
            allItemList.Add(new Item.ItemData(row[0], row[1], row[2], row[3], row[4] == "TRUE", row[5]));
        }

        CharacterSave();
        CharacterLoad();
        ItemSave();
        ItemLoad();

        // TabClick �޼��� ȣ���Ͽ� �ʱ� �� ����
        TabClick("Character");
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
                itemInvenSlotUI.SetActive(false);
                itemSlotNumText.SetActive(false);
                characterInvenSlotUI.SetActive(true);
                characterSlotNumText.SetActive(true);

                break;
            case 1:
                characterInvenSlotUI.SetActive(false);
                characterSlotNumText.SetActive(false);
                itemInvenSlotUI.SetActive(true);
                itemSlotNumText.SetActive(true);

                break;
            default:
                characterInvenSlotUI.SetActive(false);
                itemInvenSlotUI.SetActive(false);
                characterSlotNumText.SetActive(false);
                itemSlotNumText.SetActive(false);

                break;
        }

    }

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
        myCharacterList = JsonConvert.DeserializeObject<List<Character.CharacterData>>(jdata);
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
