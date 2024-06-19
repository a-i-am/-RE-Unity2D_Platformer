using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable] // 데이터 직렬화(순서대로 Inspector에 표시해줌)
public class Item
{
    [System.Serializable] // 데이터 직렬화(리스트 순서대로 Inspector에 표시해줌)
    public class ItemData
    {
        //public SpriteRenderer image;
        public ItemData(string _type, string _name, string _explain, string _number, bool _isUsing, string _tabName = "Item")
        { tabName = _tabName; type = _type; name = _name; explain = _explain; number = _number; isUsing = _isUsing; }

        public string tabName, type, name, explain, number; // string이어야 JSON 파싱 시 잘 된다고 함
        private bool isUsing;
        public Sprite itemImage;
        public List<ItemEffect> efts; 

        public bool UseItem()
        {
            isUsing = false;
            foreach (ItemEffect eft in efts)
            {
                isUsing = eft.ExecuteRole();
            }
            return isUsing;
        }
    }
}