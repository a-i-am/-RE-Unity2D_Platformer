using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerUpHandler
{
    public int slotnum;

    public Item.ItemData itemData;
    public Character.CharacterData characterData;
    public Image itemIcon;
    public Image characterIcon;

    public void UpdateItemSlotUI()
    {
        itemIcon.sprite = itemData.itemImage;
        itemIcon.gameObject.SetActive(true);

    }

    public void UpdateCharacterSlotUI()
    {
        characterIcon.sprite = characterData.characterImage;
        characterIcon.gameObject.SetActive(true);

    }

    public void RemoveItemSlot()
    {
        //slot_item = null;
        itemData = null;
        itemIcon.gameObject.SetActive(false);
    }


    public void RemoveCharacterSlot()
    {
        //slot_item = null;
        characterData = null;
        characterIcon.gameObject.SetActive(false);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        bool itemIsUse = itemData.UseItem();
        //bool characterIsUse = characterData.UseCharacter();

        if (itemIsUse)
        {
            Inventory.instance.RemoveItem(slotnum);
        }
        //if (characterIsUse)
        //{
        //    Inventory.instance.RemoveCharacter(slotnum);
        //}

    }
}

