using System.Collections.Generic;
using UnityEngine;

public class FieldItems : MonoBehaviour
{
    public Item.ItemData field_item;
    public SpriteRenderer image;

    public void SetItem(Item.ItemData _item)
    {
        field_item = _item;
        image.sprite = _item.itemImage;
    }

    public Item.ItemData GetItem()
    {
        return field_item;
    }

    public void DestroyItem()
    {
        Destroy(gameObject);
    }

    //public void SetRandomItem()
    //{
    //    List<Item.ItemData> itemDB = InventoryDatabase.Instance.allItemList;
    //    if (itemDB.Count > 0)
    //    {
    //        SetItem(itemDB[UnityEngine.Random.Range(0, itemDB.Count)]);
    //    }
    //}
}

