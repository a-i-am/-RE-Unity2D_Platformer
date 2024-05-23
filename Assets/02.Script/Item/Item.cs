using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//public enum TabName { Character, Item }
//public enum ItemType { absorption, Goods, Equipment }

[System.Serializable] // ������ ����ȭ(������� Inspector�� ǥ������)
public class Item
{
    [System.Serializable] // ������ ����ȭ(����Ʈ ������� Inspector�� ǥ������)
    public class CharacterData
    {
        public CharacterData(string _type, string _name, string _explain, string _number, bool _isUsing, string _tabName = "Character")
        { tabName = _tabName; type = _type; name = _name; explain = _explain; number = _number; isUsing = _isUsing; }

        public string tabName, type, name, explain, number; // string�̾�� JSON �Ľ� �� �� �ȴٰ� ��
        public bool isUsing;
        public Sprite characterImage;
        //public List<ItemEffect> efts;
    }
    [System.Serializable] // ������ ����ȭ(����Ʈ ������� Inspector�� ǥ������)
    public class ItemData
    {
        //public SpriteRenderer image;
        public ItemData(string _type, string _name, string _explain, string _number, bool _isUsing, string _tabName = "Item")
        { tabName = _tabName; type = _type; name = _name; explain = _explain; number = _number; isUsing = _isUsing; }

        public string tabName, type, name, explain, number; // string�̾�� JSON �Ľ� �� �� �ȴٰ� ��
        private bool isUsing;
        //public List<ItemEffect> efts;
        public Sprite itemImage;

        public bool Use()
        {
            isUsing = false;
            //foreach (ItemEffect eft in efts)
            //{
            //    isUsing = eft.ExecuteRole();
            //}
            return isUsing;
        }
    }
}
    //public ItemData tabName;
    //public ItemType itemType;
    //public string itemName;
    //public Sprite itemImage;
    //public List<ItemEffect> efts;

    //public bool Use()
    //{
    //    //bool isUsed = false;
    //    //foreach (ItemEffect eft in efts)
    //    //{
    //    //    isUsed = eft.ExecuteRole();
    //    //}
    //    //return isUsed;
    //}