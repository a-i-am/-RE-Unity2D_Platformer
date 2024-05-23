using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerUpHandler
{
    //public ItemData slot_item;
    public int slotnum;
    public Item.ItemData item;
    public Image itemIcon;

    public void UpdateSlotUI()
    {
        //itemIcon.sprite = slot_item.itemImage;
        itemIcon.sprite = item.itemImage;
        itemIcon.gameObject.SetActive(true);

    }
    public void RemoveSlot()
    {
        //slot_item = null;
        item = null;
        itemIcon.gameObject.SetActive(false);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //bool isUse = slot_item.Use();
        //bool isUse = item.Use();
        //if (isUse)
        //{
        //    Inventory.instance.RemoveItem(slotnum);
        //}
    }
}

