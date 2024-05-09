//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public enum ItemType
//{
//    Equipment,
//    Consumables,
//    Etc
//}
//[System.Serializable] // 데이터 직렬화(순서대로 Inspector에 표시해줌)
//public class Item
//{
//    //public ItemType itemType;
//    //public string itemName;
//    //public Sprite itemImage;
//    public List<ItemEffect> efts;



//    public bool Use()
//    {
//        bool isUsed = false;
//        foreach (ItemEffect eft in efts) 
//        {
//            isUsed = eft.ExecuteRole();
//        }
//        return isUsed;
//    }
//}

