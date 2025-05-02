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
        inven.onChangeCharacter += RedrawCharacterSlotUI;
        inven.onChangeItem += RedrawItemSlotUI;


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
    void RedrawCharacterSlotUI()
    {
        // 이전 슬롯 필터링 데이터 초기화
        //for (int i = 0; i < characterSlots.Length; i++)
        //{
        //    characterSlots[i].RemoveCharacterSlot();
        //}

        // 슬롯 데이터 필터링
        filteredCharacterList = inven.characters.FindAll(character => character.type == invenDB.characterCurSubType);
        for (int i = 0; i < filteredCharacterList.Count && i < characterSlots.Length; i++)
        {
            characterSlots[i].characterData = filteredCharacterList[i];
            characterSlots[i].UpdateCharacterSlotUI();
        }
    }

    //public void RemoveCharacterSlotAt(int index)
    //{
    //    if (index >= 0 && index < characterSlots.Length)
    //    {
    //        // 공통 : 인덱스 지정
    //        characterSlots[index].RemoveCharacterSlot(); // Only 슬롯 데이터를 직접 지움 
    //        inven.RemoveCharacter(index); //  Inventory.characters 리스트 데이터를 Remove + RedrawSlotUI Action 호출
    //        inven.characters.RemoveAt(index);
    //        inven.acquiredCharacters--;
    //        inven.onChangeCharacter.Invoke();
    //    }
    //}


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




    void RedrawItemSlotUI()
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




}