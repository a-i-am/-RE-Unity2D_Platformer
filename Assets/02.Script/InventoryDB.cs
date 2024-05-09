using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft;
using Newtonsoft.Json;
using System.IO;

[System.Serializable] // 데이터 직렬화(순서대로 Inspector에 표시해줌)
public class Item
{
    public Item(string _type, string _name, string _explain, string _number, bool _isUsing)
    { type = _type; name = _name; explain = _explain; number = _number; isUsing = _isUsing; }
    
    public string type, name, explain, number; // string이어야 JSON 파싱 시 잘 된다고 함
    public bool isUsing;
}
public class InventoryDB : MonoBehaviour
{
    public static InventoryDB instance;
    public TextAsset itemDBText; // ItemDatabase
    public List<Item> allItemList, myItemList;

    void Start()
    {
        // 라인 끝 공백을 제외하고 텍스트를 읽음
        // 개행문자(\n)을 구분자로 데이터 분할
        string[] line = itemDBText.text.Substring(0, itemDBText.text.Length - 1).Split('\n');
        //print(line.Length);
        for (int i = 0; i < line.Length; i++)
        {
            string[] row = line[i].Split('\t');
            allItemList.Add(new Item(row[0], row[1], row[2], row[3], row[4] == "TRUE"));
        }
        Save();
    }   

    private void Awake()
    {
        instance = this;
    }

    void Save()
    {
        string jdata = JsonConvert.SerializeObject(allItemList);
        print(jdata);
        File.WriteAllText(Application.dataPath + "/07.Resources/MyItemText.txt", "하이");
        //print(Application.dataPath);
    }
}
