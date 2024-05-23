using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Inventory : MonoBehaviour
{
    #region Singleton
    public static Inventory instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one Inventory instance found!");
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    #endregion

    public delegate void OnSlotCountChange(int val);
    public OnSlotCountChange onSlotCountChange;

    public delegate void OnChangeItem();
    public OnChangeItem onChangeItem;

    //public List<ItemData> inventory_items = new List<ItemData>();
    //public List<CharacterData> characters = new List<CharacterData>();
    //public List<Item> items = new List<Item>();
    public List<Item.ItemData> inventory_items = new List<Item.ItemData>();

    private int slotCnt;
    public int SlotCnt
    {
        get => slotCnt;
        set
        {
            slotCnt = value;
            onSlotCountChange.Invoke(slotCnt);
        }
    }
    void Start()
    {
        SlotCnt = 8;
    }

    public bool AddItem(Item.ItemData _item) // ItemData _item
    {
        if (inventory_items.Count < SlotCnt)
        {
            inventory_items.Add(_item);
            if (onChangeItem != null)
                onChangeItem.Invoke();
            return true;
        }
        return false;
        //if (items.Count < SlotCnt)
        //{
        //    items.Add(_item);
        //    if (onChangeItem != null)
        //        onChangeItem.Invoke();
        //    return true;
        //}
        //return false;

    }

    public void RemoveItem(int _index)
    {
        inventory_items.RemoveAt(_index);
        onChangeItem.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("FieldItem"))
        {
            FieldItems fieldItems = collision.GetComponent<FieldItems>();
            if (AddItem(fieldItems.GetItem()))
            {
                fieldItems.DestroyItem();
            }
        }
    }
}
