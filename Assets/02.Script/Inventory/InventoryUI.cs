using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    //Inventory inven;

    public GameObject inventoryPanel;
    bool activeInventory = false;

    //public Slot[] itemSlots;
    //public Slot[] chSlots;

    public Transform itemSlotHolder;
    public Transform chSlotHolder;

    void Start()
    {
        // ��ư ������Ʈ�� ã�Ƽ� Ŭ�� �̺�Ʈ �Լ��� ����
        //Button tabButton = GameObject.Find("TabButton").GetComponent<Button>();
        //tabButton.onClick.AddListener(delegate { TabClick("ChInvenButton"); });
        //tabButton.onClick.AddListener(delegate { TabClick("ItemInvenButton"); });


        //inven = Inventory.instance;

        //itemSlots = itemSlotHolder.GetComponentsInChildren<Slot>();
        //chSlots = chSlotHolder.GetComponentsInChildren<Slot>();

        //inven.onSlotCountChange += SlotChange;
        //inven.onChangeItem += RedrawSlotUI;
        inventoryPanel.SetActive(activeInventory);
    }

    //private void SlotChange(int val)
    //{
    //    for (int i = 0; i < itemSlots.Length; i++)
    //    {
    //        itemSlots[i].slotnum = i;

    //        if (i < inven.SlotCnt)
    //            itemSlots[i].GetComponent<Button>().interactable = true;
    //        else
    //            itemSlots[i].GetComponent<Button>().interactable = false;
    //    }
    //}
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            activeInventory = !activeInventory;
            inventoryPanel.SetActive(activeInventory);
        }
    }

    public void TabClick(string tabName)
    {

    }
    
    //public void AddSlot()
    //{
    //    inven.SlotCnt++;
    //}

    //void RedrawSlotUI()
    //{
    //    for (int i = 0; i < itemSlots.Length; i++)
    //    {
    //        itemSlots[i].RemoveSlot();
    //    }
    //    for (int i = 0; i < inven.items.Count; i++)
    //    {
    //        itemSlots[i].item = inven.items[i];
    //        itemSlots[i].UpdateSlotUI();
    //    }
    //}

}