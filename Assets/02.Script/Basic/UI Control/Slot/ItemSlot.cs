using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IPointerUpHandler
{
    
    public int itemSlotnum;
    public Item.ItemData itemData;
    public Image itemIcon;

    public void UpdateItemSlotUI()
    {
        itemIcon.sprite = itemData.itemImage;
        itemIcon.gameObject.SetActive(true);
    }

    public void RemoveItemSlot()
    {
        itemData = null;
        itemIcon.gameObject.SetActive(false);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        bool itemIsUse = itemData.UseItem();

        if (itemIsUse)
        {
            Inventory.Instance.RemoveItem(itemSlotnum);
            Inventory.Instance.acquiredItems--;
        }
    }
}

