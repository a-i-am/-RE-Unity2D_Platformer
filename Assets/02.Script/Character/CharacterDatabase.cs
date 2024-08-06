//using System.Collections.Generic;
//using UnityEngine;
//using Newtonsoft;
//using Newtonsoft.Json;
//using System.IO;
//using UnityEngine.UI;

//[System.Serializable] // 데이터 직렬화(순서대로 Inspector에 표시해줌)

//public class Character
//{
//    public Character(string _type, string _name, string _explain, string _number, bool _isUsing, string _tabName = "Character")
//    { tabName = _tabName; type = _type; name = _name; explain = _explain; number = _number; isUsing = _isUsing; }

//    public string tabName, type, name, explain, number; // string이어야 JSON 파싱 시 잘 된다고 함
//    public bool isUsing;
//}

//public class CharacterDatabase : MonoBehaviour
//{
//    public static CharacterDatabase instance;
//    public TextAsset characterDBText; // CharacterDatabase
//    public List<Character> allItemList, myCharacterList, curCharacterList;
//    public GameObject[] InvenSlot, CharacterInfo;
//    public string curType = "Character";
//    public Image[] tabImage;
//    public Sprite tabIdleSprite, tabSelectSprite;

//    void Start()
//    {
//        // 라인 끝 공백을 제외하고 텍스트를 읽음
//        // 개행문자(\n)을 구분자로 데이터 분할
//        string[] line = characterDBText.text.Substring(0, characterDBText.text.Length - 1).Split('\n');
//        //print(line.Length);
//        for (int i = 0; i < line.Length; i++)
//        {
//            string[] row = line[i].Split('\t');
//            allItemList.Add(new Character(row[0], row[1], row[2], row[3], row[4] == "TRUE"));
//        }
//        Save();
//        Load();
//    }

//    private void Awake()
//    {
//        instance = this;
//    }

//    public void TabClick(string tabName)
//    {
//        // 현재 아이템 리스트에 클릭한 타입만 추가
//        curType = tabName;
//        curCharacterList = myCharacterList.FindAll(x => x.type == tabName);

//        // 슬롯과 텍스트 보이기
//        for (int i = 0; i < InvenSlot.Length; i++)
//        {
//            InvenSlot[i].SetActive(i < curCharacterList.Count);
//            //CharacterInfo[i].GetComponentInChildren<Text>().text = i < curCharacterList.Count ? curCharacterList[i].Name : "";
//        }

//        int tabNum = 0;
//        switch (tabName)
//        {
//            case "Character": tabNum = 0; break;
//            case "Item": tabNum = 1; break;
//        }
//        for (int i = 0; i < tabImage.Length; i++)
//            tabImage[i].sprite = i == tabNum ? tabSelectSprite : tabIdleSprite;
//    }
//    void Save()
//    {
//        string jdata = JsonConvert.SerializeObject(allItemList); // JSON으로 리스트를 string으로 변환
//        //print(jdata);
//        File.WriteAllText(Application.dataPath + "/07.Resources/MyCharacterText.txt", jdata);
//        //print(Application.dataPath);
//    }

//    void Load()
//    {
//        string jdata = File.ReadAllText(Application.dataPath + "/07.Resources/MyCharacterText.txt");
//        myCharacterList = JsonConvert.DeserializeObject<List<Character>>(jdata);
//    }
//}
