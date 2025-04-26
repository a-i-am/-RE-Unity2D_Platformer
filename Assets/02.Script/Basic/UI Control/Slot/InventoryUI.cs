using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    private List<Character.CharacterData> filteredCharacterList = new List<Character.CharacterData>();
    private List<Item.ItemData> filteredItemList = new List<Item.ItemData>();

    private InventoryDatabase invenDB;
    private Inventory inven;
    [SerializeField] private GameObject playerUI;
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private TextMeshProUGUI itemSlotNumText;
    [SerializeField] private TextMeshProUGUI characterSlotNumText;
    private bool activeInventory = false;

    public ItemSlot[] itemSlots;
    public CharacterSlot[] characterSlots;
    
    [SerializeField] private Transform itemSlotHolder;
    [SerializeField] private Transform characterSlotHolder;

    void Start()
    {
        inven = Inventory.Instance;
        invenDB = InventoryDatabase.Instance; 
        itemSlots = itemSlotHolder.GetComponentsInChildren<ItemSlot>();
        characterSlots = characterSlotHolder.GetComponentsInChildren<CharacterSlot>();

        inven.onItemSlotCountChange += ItemSlotChange;
        inven.onCharacterSlotCountChange += CharacterSlotChange;

        inven.onChangeItem += RedrawItemSlotUI;
        inven.onChangeCharacter  += RedrawCharacterSlotUI;
        invenDB.onCharacterSubTab += RedrawCharacterSlotUI;
        invenDB.onItemSubTab += RedrawItemSlotUI;

        inventoryPanel.SetActive(activeInventory);
    }

    void FixedUpdate()
    {
        //itemSlotNumText.text = string.Format("{0} / {1}", inven.acquiredItems, itemSlots.Length);
        itemSlotNumText.text = string.Format("{0} / {1}", inven.acquiredItems, inven.ItemSlotCnt);

        //characterSlotNumText.text = string.Format("{0} / {1}", inven.acquiredCharacters, characterSlots.Length);
        characterSlotNumText.text = string.Format("{0} / {1}", inven.acquiredCharacters, inven.CharacterSlotCnt);

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
            playerUI.SetActive(activeInventory); // 기본 false
            activeInventory = !activeInventory; // 기본 false -> true
            inventoryPanel.SetActive(activeInventory);
        }
    }

    public void AddCharacterSlot()
    {
        if(inven.CharacterSlotCnt < characterSlots.Length)
        inven.CharacterSlotCnt++;
    }

    public void AddItemSlot()
    {
        if(inven.ItemSlotCnt < itemSlots.Length)
        inven.ItemSlotCnt++;
    }

    void RedrawItemSlotUI()
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            itemSlots[i].RemoveItemSlot();
        }

        filteredItemList = inven.items.FindAll(item => item.type == invenDB.itemCurSubType);

        for (int i = 0; i < filteredItemList.Count && i < itemSlots.Length; i++)
        {
            itemSlots[i].itemData = filteredItemList[i];
            itemSlots[i].UpdateItemSlotUI();
        }


    }

    void RedrawCharacterSlotUI()
    {
        for (int i = 0; i < characterSlots.Length; i++)
        {
            characterSlots[i].RemoveCharacterSlot();
        }

        filteredCharacterList = inven.characters.FindAll(character => character.type == invenDB.characterCurSubType);

        for (int i = 0; i < filteredCharacterList.Count && i < characterSlots.Length; i++)
        {
            characterSlots[i].characterData = filteredCharacterList[i];
            characterSlots[i].UpdateCharacterSlotUI();
        }
    }

}