using System.Collections.Generic;
using UnityEngine;
using Newtonsoft;
using Newtonsoft.Json;
using System.IO;
using UnityEngine.UI;

public class InventoryDatabase : Singleton<InventoryDatabase>
{
    
    // 아이템 정보표시용
    private GameObject[] ItemInfo; 
    private GameObject[] CharacterInfo;

    [Header("슬롯")]
    [SerializeField] private GameObject[] InvenSlot; // 일단 슬롯 개수는 캐릭터 & 아이템 동기화
    [SerializeField] private GameObject characterInvenSlotUI;
    [SerializeField] private GameObject itemInvenSlotUI;
    [SerializeField] private GameObject characterSlotNumText;
    [SerializeField] private GameObject itemSlotNumText;

    [Header("카테고리 및 탭")]
    [SerializeField] private GameObject characterSubTab;
    [SerializeField] private GameObject itemSubTab;
    [SerializeField] private Image[] mainTabImage;
    [SerializeField] private Image[] characterSubTabImage;
    [SerializeField] private Image[] itemSubTabImage;
    [SerializeField] private Sprite mainTabIdleSprite, mainTabSelectSprite;
    [SerializeField] private Sprite subTabIdleSprite, subTabSelectSprite;
    [SerializeField] private string curMainTabType = "Character";
    [SerializeField] private string curSubTabType = "Absorption";

    [Header("텍스트 에셋")]
    [SerializeField] private TextAsset itemDBText, characterDBText;
    
    [Header("캐릭터/아이템 리스트")]
    private List<Character.CharacterData> curCharacterList = new List<Character.CharacterData>();
    private List<Item.ItemData> curItemList = new List<Item.ItemData>();
    private List<Character.CharacterData> allCharacterList = new List<Character.CharacterData>();
    [SerializeField] private List<Character.CharacterData> myCharacterList = new List<Character.CharacterData>();
    [SerializeField] private List<Item.ItemData> myItemList = new List<Item.ItemData>();
    private List<Item.ItemData> allItemList = new List<Item.ItemData>();
    
    [Header("캐릭터/아이템 DB")]
    public List<Item.ItemData> itemDB = new List<Item.ItemData>();
    public List<Character.CharacterData> characterDB = new List<Character.CharacterData>();
    [Space(25)]
    [Header("필드 아이템 배치")]
    [SerializeField] private GameObject fieldItemPrefab;
    [SerializeField] private Vector3[] pos;

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
            Follower characterPrefab = Resources.Load<Follower>(row[6]); // 캐릭터 프리팹 경로에서 로드

            allCharacterList.Add(new Character.CharacterData(
                row[0],  // type
                row[1],  // name
                row[2],  // explain
                row[3],  // number
                row[4] == "TRUE",  // isUsing
                characterImage,  
                characterPrefab  
            ));
        }

        string[] itemLine = itemDBText.text.Substring(0, itemDBText.text.Length - 1).Split('\n');
        //print(itemLine.Length);
        for (int i = 0; i < itemLine.Length; i++)
        {
            string[] row = itemLine[i].Split('\t');
            allItemList.Add(new Item.ItemData(
                row[0],
                row[1],
                row[2],
                row[3],
                row[4] == "FALSE"));
        }

        ItemSave();
        ItemLoad();
        CharacterSave();
        CharacterLoad();

        // TabClick 메서드 호출하여 초기 탭 설정
        //MainTabClick(curMainTabType);
        //SubTabClick(curSubTabType);

        ActiveCharacterList(curMainTabType);
        ActiveItemList(curMainTabType);
        ActiveCharacterList(curSubTabType);
        ActiveItemList(curSubTabType);
    }

    private void ActiveCharacterList(string tabName)
    {
        curCharacterList = myCharacterList.FindAll(x => x.type == tabName);

        // Character 리스트 슬롯과 텍스트 보이기
        for (int i_Character = 0; i_Character < InvenSlot.Length; i_Character++)
        {
            InvenSlot[i_Character].SetActive(i_Character < curCharacterList.Count);
            //ItemInfo[i_Character].GetComponentInChildren<Text>().text = i < curItemList.Count ? curItemList[i].Name : "";
        }

    }

    private void ActiveItemList(string tabName)
    {
        curItemList = myItemList.FindAll(x => x.type == tabName);

        // Item 리스트 슬롯과 텍스트 보이기
        for (int i_item = 0; i_item < InvenSlot.Length; i_item++)
        {
            InvenSlot[i_item].SetActive(i_item < curCharacterList.Count);
            //ItemInfo[i_item].GetComponentInChildren<Text>().text = i < curItemList.Count ? curItemList[i].Name : "";
        }

    }

    public void MainTabClick(string tabName)
    {
        // 현재 아이템 리스트에 클릭한 타입만 추가
        //curMainTabType = ;
        //ActiveCharacterList(curMainTabType);
        //ActiveItemList(curMainTabType);
        ActiveCharacterList(curMainTabType);
        ActiveItemList(curMainTabType);

        int tabNum = 0;
        switch (tabName)
        {
            case "Character": tabNum = 0; break;
            case "Item": tabNum = 1; break;
            default: tabNum = 2; break;
        }

            for (int i = 0; i < mainTabImage.Length; i++)
            mainTabImage[i].sprite = i == tabNum ? mainTabSelectSprite : mainTabIdleSprite;

        switch (tabNum)
        {
            case 0:
                itemInvenSlotUI.SetActive(false);
                itemSlotNumText.SetActive(false);
                characterInvenSlotUI.SetActive(true);
                characterSlotNumText.SetActive(true);
                characterSubTab.SetActive(true);
                itemSubTab.SetActive(false);
                break;
            case 1:
                characterInvenSlotUI.SetActive(false);
                characterSlotNumText.SetActive(false);
                itemInvenSlotUI.SetActive(true);
                itemSlotNumText.SetActive(true);
                itemSubTab.SetActive(true);
                characterSubTab.SetActive(false);
                break;
            default:
                characterInvenSlotUI.SetActive(false);
                itemInvenSlotUI.SetActive(false);
                characterSlotNumText.SetActive(false);
                itemSlotNumText.SetActive(false);
                characterSubTab.SetActive(false);
                itemSubTab.SetActive(false);
                break;
        }

    }


    public void CharacterSubTabClick(string tabName)
    {
        // 현재 아이템 리스트에 클릭한 타입만 추가
        //curSubTabType = tabName;
        ActiveCharacterList(curSubTabType);
        ActiveItemList(curSubTabType);

        int tabNum = 0;
        switch (tabName)
        {
            case "A": tabNum = 0; break;
            case "B": tabNum = 1; break;
            case "C": tabNum = 2; break;
            case "D": tabNum = 3; break;
            default: tabNum = 4; break;
        }

        for (int i = 0; i < characterSubTabImage.Length; i++)
            characterSubTabImage[i].sprite = i == tabNum ? subTabSelectSprite : subTabIdleSprite;

        characterInvenSlotUI.SetActive(true);
        characterSlotNumText.SetActive(true);
        itemInvenSlotUI.SetActive(false);
        itemSlotNumText.SetActive(false);

        switch (tabNum)
        {
            case 0:

                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            default:
                break;
        }
    }

    public void ItemSubTabClick(string tabName)
    {
        // 현재 아이템 리스트에 클릭한 타입만 추가
        //curSubTabType = tabName;
        int tabNum = 0;
        switch (tabName)
        {
            case "Absorption": tabNum = 0; break;
            case "Equipment": tabNum = 1; break;
            case "Etc": tabNum = 2; break;
            case "Mission": tabNum = 3; break;
            default: tabNum = 4; break;
        }

        for (int i = 0; i < itemSubTabImage.Length; i++)
            itemSubTabImage[i].sprite = i == tabNum ? subTabSelectSprite : subTabIdleSprite;

        itemInvenSlotUI.SetActive(true);
        itemSlotNumText.SetActive(true);
        characterInvenSlotUI.SetActive(false);
        characterSlotNumText.SetActive(false);

        switch (tabNum)
        {
            case 0:

                break;
            case 1:

                break;
            case 2:
                break;
            case 3:
                break;
            default:
                break;
        }
    }




    void CharacterSave()
    {
        string jdata = JsonConvert.SerializeObject(allCharacterList, Formatting.Indented); // JSON으로 리스트를 string으로 변환
        print(jdata);
        File.WriteAllText(Application.dataPath + "/Data/MyCharacterText.txt", jdata);
        //print(Application.dataPath);
    }

    void CharacterLoad()
    {
        string jdata = File.ReadAllText(Application.dataPath + "/Data/MyCharacterText.txt");
        myCharacterList = JsonConvert.DeserializeObject<List<Character.CharacterData>>(jdata);
        MainTabClick(curMainTabType); // 맨 처음 선택되어있는 메인 탭(캐릭터탭)
    }
    void ItemSave()
    {
        string jdata = JsonConvert.SerializeObject(allItemList, Formatting.Indented); // JSON으로 리스트를 string으로 변환
        print(jdata);
        File.WriteAllText(Application.dataPath + "/Data/MyItemText.txt", jdata);
        //print(Application.dataPath);
    }

    void ItemLoad()
    {
        string jdata = File.ReadAllText(Application.dataPath + "/Data/MyItemText.txt");
        myItemList = JsonConvert.DeserializeObject<List<Item.ItemData>>(jdata);
    }



}
