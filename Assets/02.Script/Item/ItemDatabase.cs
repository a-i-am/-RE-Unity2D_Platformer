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
        // 필드에 아이템 리스트 중 랜덤 생성
        for (int i = 0; i < 7; i++) // 생성할 아이템 개수만큼 반복 // 인스펙터에 Pos 개수 및 생성 위치 작성 필요
        {
            GameObject go = Instantiate(fieldItemPrefab, pos[i], Quaternion.identity);
            FieldItems fieldItems = go.GetComponent<FieldItems>();
            fieldItems.SetRandomItem();
        }

        // 라인 끝 공백을 제외하고 텍스트를 읽음
        // 개행문자(\n)을 구분자로 데이터 분할
        string[] characterLine = characterDBText.text.Substring(0, characterDBText.text.Length - 1).Split('\n');
        for (int i = 0; i < characterLine.Length; i++)
        {
            string[] row = characterLine[i].Split('\t');
            Sprite characterImage = Resources.Load<Sprite>(row[5]); // 캐릭터 이미지 경로에서 스프라이트 로드
            GameObject characterPrefab = Resources.Load<GameObject>(row[6]); // 캐릭터 프리팹 경로에서 로드

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

        // TabClick 메서드 호출하여 초기 탭 설정
        TabClick("Character");
    }

    private void Awake()
    {
        instance = this;
    }

    public void TabClick(string tabName)
    {
        // 현재 아이템 리스트에 클릭한 타입만 추가
        curType = tabName;
        curItemList = myItemList.FindAll(x => x.type == tabName);
        curCharacterList = myCharacterList.FindAll(x => x.type == tabName);

        // Character 리스트 슬롯과 텍스트 보이기
        for (int i_Character = 0; i_Character < InvenSlot.Length; i_Character++)
        {
            InvenSlot[i_Character].SetActive(i_Character < curCharacterList.Count);
            //ItemInfo[i_Character].GetComponentInChildren<Text>().text = i < curItemList.Count ? curItemList[i].Name : "";
        }

        // Item 리스트 슬롯과 텍스트 보이기
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
            default: tabNum = 2; break; // case조건들에 전부 해당하지 않는 경우
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
        string jdata = JsonConvert.SerializeObject(allCharacterList, Formatting.Indented); // JSON으로 리스트를 string으로 변환
        //print(jdata);
        File.WriteAllText(Application.dataPath + "/07.Resources/MyCharacterText.txt", jdata);
        //print(Application.dataPath);
    }

    void CharacterLoad()
    {
        string jdata = File.ReadAllText(Application.dataPath + "/07.Resources/MyCharacterText.txt");
        myCharacterList = JsonConvert.DeserializeObject<List<Character.CharacterData>>(jdata);
        TabClick(curType); // 맨 처음 선택되어있는 메인 탭(캐릭터탭)
    }

    void ItemSave()
    {
        string jdata = JsonConvert.SerializeObject(allItemList, Formatting.Indented); // JSON으로 리스트를 string으로 변환
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
