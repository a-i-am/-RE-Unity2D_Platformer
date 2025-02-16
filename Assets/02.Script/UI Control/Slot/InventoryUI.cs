using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    Inventory inven;

    public GameObject inventoryPanel;
    public TextMeshProUGUI itemSlotNumText;
    public TextMeshProUGUI characterSlotNumText;
    bool activeInventory = false;

    public ItemSlot[] itemSlots;
    public CharacterSlot[] characterSlots;
    
    public Transform itemSlotHolder;
    public Transform characterSlotHolder;

    void Start()
    {
        inven = Inventory.instance;
        itemSlots = itemSlotHolder.GetComponentsInChildren<ItemSlot>();
        characterSlots = characterSlotHolder.GetComponentsInChildren<CharacterSlot>();

        inven.onItemSlotCountChange += ItemSlotChange;
        inven.onCharacterSlotCountChange += CharacterSlotChange;

        inven.onChangeItem += RedrawItemSlotUI;
        inven.onChangeCharacter += RedrawCharacterSlotUI;


        inventoryPanel.SetActive(activeInventory);
    }

    void FixedUpdate()
    {
        itemSlotNumText.text = string.Format("{0} / {1}", inven.acquiredItems, itemSlots.Length);
        characterSlotNumText.text = string.Format("{0} / {1}", inven.acquiredCharacters, characterSlots.Length);
    }

    private void ItemSlotChange(int val)
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            itemSlots[i].itemSlotnum = i;

            if (i < inven.ItemSlotCnt)
                itemSlots[i].GetComponent<Button>().interactable = true;
            else
                itemSlots[i].GetComponent<Button>().interactable = false;
        }
    }

    private void CharacterSlotChange(int val)
    {
        for (int i = 0; i < characterSlots.Length; i++)
        {
            characterSlots[i].characterSlotnum = i;

            if (i < inven.CharacterSlotCnt)
                characterSlots[i].GetComponent<Button>().interactable = true;
            else
                characterSlots[i].GetComponent<Button>().interactable = false;
        }
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            activeInventory = !activeInventory;
            inventoryPanel.SetActive(activeInventory);
        }
    }

    public void AddItemSlot()
    {
        inven.ItemSlotCnt++;
    }
    public void AddCharacterSlot()
    {
        inven.CharacterSlotCnt++;
    }

    void RedrawItemSlotUI()
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            itemSlots[i].RemoveItemSlot();
        }


        for (int i = 0; i < inven.inventory_items.Count; i++) 
        {
            itemSlots[i].itemData = inven.inventory_items[i];

            itemSlots[i].UpdateItemSlotUI();
        }
    }

    void RedrawCharacterSlotUI()
    {
        for (int i = 0; i < characterSlots.Length; i++)
        {
            characterSlots[i].RemoveCharacterSlot();
        }


        for (int i = 0; i < inven.inventory_characters.Count; i++)
        {
            characterSlots[i].characterData = inven.inventory_characters[i];

            characterSlots[i].UpdateCharacterSlotUI();
        }
    }

}