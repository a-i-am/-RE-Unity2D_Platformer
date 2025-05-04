using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static Inventory;
using System.Reflection;
using UnityEngine.TextCore.Text;

public class InventoryUI : MonoBehaviour
{
    // 외부 참조
    private InventoryDatabase invenDB;
    private Inventory inven;

    [Header("패널")]
    [SerializeField] private GameObject playerUI;
    [SerializeField] private GameObject inventoryPanel;
    [Header("텍스트")]
    [SerializeField] private TextMeshProUGUI itemSlotNumText;
    [SerializeField] private TextMeshProUGUI characterSlotNumText;
    [Header("슬롯")]
    public ItemSlot[] itemSlots;
    public CharacterSlot[] characterSlots;
    [SerializeField] private Transform itemSlotHolder;
    [SerializeField] private Transform characterSlotHolder;

    // 리스트    
    private List<Character.CharacterData> filteredCharacterList = new List<Character.CharacterData>();
    private List<Item.ItemData> filteredItemList = new List<Item.ItemData>();

    // 인벤토리 ON/OFF
    private bool activeInventory = false;

    private void Awake()
    {
        itemSlots = itemSlotHolder.GetComponentsInChildren<ItemSlot>();
        characterSlots = characterSlotHolder.GetComponentsInChildren<CharacterSlot>();
    }
    private void Start()
    {
        inven = Inventory.Instance;
        invenDB = InventoryDatabase.Instance;

        inven.onItemSlotCountChange += ItemSlotChange;
        inven.onCharacterSlotCountChange += CharacterSlotChange;
        inven.onChangeCharacter += () => RedrawAllCharacterSlotsUI();
        invenDB.onCharacterSubTab += () => RedrawAllCharacterSlotsUI();
        inven.onChangeItem += RedrawItemSlotUI;
        invenDB.onItemSubTab += RedrawItemSlotUI;

        inventoryPanel.SetActive(activeInventory);

        //itemSlotNumText.text = string.Format("{0} / {1}", inven.acquiredItems, itemSlots.Length);
        itemSlotNumText.text = string.Format("{0} / {1}", inven.acquiredItems, inven.ItemSlotCnt);

        //characterSlotNumText.text = string.Format("{0} / {1}", inven.acquiredCharacters, characterSlots.Length);
        characterSlotNumText.text = string.Format("{0} / {1}", inven.acquiredCharacters, inven.CharacterSlotCnt);

    }

    void FixedUpdate()
    {
        ////itemSlotNumText.text = string.Format("{0} / {1}", inven.acquiredItems, itemSlots.Length);
        //itemSlotNumText.text = string.Format("{0} / {1}", inven.acquiredItems, inven.ItemSlotCnt);

        ////characterSlotNumText.text = string.Format("{0} / {1}", inven.acquiredCharacters, characterSlots.Length);
        //characterSlotNumText.text = string.Format("{0} / {1}", inven.acquiredCharacters, inven.CharacterSlotCnt);
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

    public void RedrawAllCharacterSlotsUI()
    {
        for (int i = 0; i < characterSlots.Length; i++)
        {
            characterSlots[i].RemoveCharacterSlot();
        }

        filteredCharacterList = inven.characters.FindAll(c => c.type == invenDB.characterCurSubType);

        for (int i = 0; i < filteredCharacterList.Count && i < characterSlots.Length; i++)
        {
            characterSlots[i].characterData = filteredCharacterList[i];
            characterSlots[i].UpdateCharacterSlotUI();
        }

        characterSlotNumText.text = $"{inven.acquiredCharacters} / {inven.CharacterSlotCnt}";
    }

    // 특정 슬롯만 비움
    public void RemoveCharacterSlotAt(int index)
    {
        if (index >= 0 && index < characterSlots.Length)
        {
            characterSlots[index].RemoveCharacterSlot();
        }

        characterSlotNumText.text = $"{inven.acquiredCharacters} / {inven.CharacterSlotCnt}";
    }

    public void RedrawItemSlotUI()
    {
        // 이전 슬롯 필터링 데이터 초기화
        for (int i = 0; i < itemSlots.Length; i++)
        {
            itemSlots[i].RemoveItemSlot();
        }


        // 슬롯 데이터 필터링
        filteredItemList = inven.items.FindAll(item => item.type == invenDB.itemCurSubType);
        for (int i = 0; i < filteredItemList.Count && i < itemSlots.Length; i++)
        {
            itemSlots[i].itemData = filteredItemList[i];
            itemSlots[i].UpdateItemSlotUI();
        }


    }

    public void RemoveItemSlotAt(int index)
    {
        if (index >= 0 && index < characterSlots.Length)
        {
            itemSlots[index].RemoveItemSlot();
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

    // [+] 슬롯 추가 버튼 이벤트
    public void AddCharacterSlot()
    {
        if (inven.CharacterSlotCnt < characterSlots.Length)
            inven.CharacterSlotCnt++;
    }

    public void AddItemSlot()
    {
        if (inven.ItemSlotCnt < itemSlots.Length)
            inven.ItemSlotCnt++;
    }
}