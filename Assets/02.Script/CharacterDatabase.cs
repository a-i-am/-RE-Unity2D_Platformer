using System.Collections.Generic;
using UnityEngine;
using Newtonsoft;
using Newtonsoft.Json;
using System.IO;

[System.Serializable] // 데이터 직렬화(순서대로 Inspector에 표시해줌)

public class Character
{
    public Character(string _type, string _name, string _explain, string _number, bool _isUsing, string _tabName = "Character")
    { tabName = _tabName; type = _type; name = _name; explain = _explain; number = _number; isUsing = _isUsing; }

    public string tabName, type, name, explain, number; // string이어야 JSON 파싱 시 잘 된다고 함
    public bool isUsing;
}

public class CharacterDatabase : MonoBehaviour
{
    public static CharacterDatabase instance;
    public TextAsset characterDBText; // CharacterDatabase
    public List<Character> allItemList, myCharacterList;
    public string curType = "Character";

    void Start()
    {
        // 라인 끝 공백을 제외하고 텍스트를 읽음
        // 개행문자(\n)을 구분자로 데이터 분할
        string[] line = characterDBText.text.Substring(0, characterDBText.text.Length - 1).Split('\n');
        //print(line.Length);
        for (int i = 0; i < line.Length; i++)
        {
            string[] row = line[i].Split('\t');
            allItemList.Add(new Character(row[0], row[1], row[2], row[3], row[4] == "TRUE"));
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
        curType = tabName;
    }
    void Save()
    {
        string jdata = JsonConvert.SerializeObject(allItemList); // JSON으로 리스트를 string으로 변환
        //print(jdata);
        File.WriteAllText(Application.dataPath + "/07.Resources/MyCharacterText.txt", jdata);
        //print(Application.dataPath);
    }

    void Load()
    {
        string jdata = File.ReadAllText(Application.dataPath + "/07.Resources/MyCharacterText.txt");
        myCharacterList = JsonConvert.DeserializeObject<List<Character>>(jdata);
    }
}
