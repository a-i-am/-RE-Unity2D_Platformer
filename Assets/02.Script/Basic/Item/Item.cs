using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item
{
    [System.Serializable]
    public class ItemData
    {
        //public SpriteRenderer image;
        public string tabName, type, name, explain, number; // string이어야 JSON 파싱 시 잘 된다고 함
        private bool isUsing;
        [JsonIgnore]
        public Sprite itemImage;
        [JsonIgnore]
        public List<ItemEffect> efts = new List<ItemEffect>();
        public ItemData(string _type, string _name, string _explain, string _number, bool _isUsing, Sprite _itemImage, string _tabName = "Item")
        { 
            tabName = _tabName;
            type = _type;
            name = _name;
            explain = _explain;
            number = _number;
            itemImage = _itemImage;
            isUsing = _isUsing; 
            
        }
        public bool UseItem()
        {
            isUsing = false;

            foreach (ItemEffect eft in efts)
            {
                if (eft == null) continue;
                isUsing = eft.ExecuteRole();
            }
            return isUsing;
        }
    }
}

