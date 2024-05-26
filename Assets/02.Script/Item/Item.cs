using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable] // ������ ����ȭ(������� Inspector�� ǥ������)
public class Item
{
    [System.Serializable] // ������ ����ȭ(����Ʈ ������� Inspector�� ǥ������)
    public class ItemData
    {
        //public SpriteRenderer image;
        public ItemData(string _type, string _name, string _explain, string _number, bool _isUsing, string _tabName = "Item")
        { tabName = _tabName; type = _type; name = _name; explain = _explain; number = _number; isUsing = _isUsing; }

        public string tabName, type, name, explain, number; // string�̾�� JSON �Ľ� �� �� �ȴٰ� ��
        private bool isUsing;
        public List<ItemEffect> efts;
        public Sprite itemImage;

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