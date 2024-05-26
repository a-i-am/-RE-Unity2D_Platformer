using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static Inventory;

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

    public delegate void OnItemSlotCountChange(int val);
    public delegate void OnCharacterSlotCountChange(int val);
    public OnItemSlotCountChange onItemSlotCountChange;
    public OnCharacterSlotCountChange onCharacterSlotCountChange;
    
    public delegate void OnChangeItem();
    public OnChangeItem onChangeItem;
    public delegate void OnChangeCharacter();
    public OnChangeCharacter onChangeCharacter;
    public List<Character.CharacterData> inventory_characters = new List<Character.CharacterData>();
    public List<Item.ItemData> inventory_items = new List<Item.ItemData>();
    
    public int acquiredItems = 0;
    public InventoryUI invenUI;

    private int itemSlotCnt;
    private int characterSlotCnt;
    public int ItemSlotCnt
    {
        get => itemSlotCnt;
        set
        {
            itemSlotCnt = value;
            onItemSlotCountChange.Invoke(itemSlotCnt);
        }
    }

    public int CharacterSlotCnt
    {
        get => characterSlotCnt;
        set
        {
            characterSlotCnt = value;
            onCharacterSlotCountChange.Invoke(characterSlotCnt);
        }
    }

    void Start()
    {
        //ItemSlotCnt = 8;
        //CharacterSlotCnt = 4;
        ItemSlotCnt = invenUI.itemSlots.Length;
        CharacterSlotCnt = invenUI.characterSlots.Length;

    }

    public bool AddItem(Item.ItemData _item) // ItemData _item
    {
        if (inventory_items.Count < ItemSlotCnt)
        {
            inventory_items.Add(_item);
            if (onChangeItem != null)
                onChangeItem.Invoke();
            return true;
        }
        return false;

    }

    public bool AddCharacter(Character.CharacterData _character) // ItemData _item
    {
        if (inventory_characters.Count < ItemSlotCnt)
        {
            inventory_characters.Add(_character);
            if (onChangeCharacter != null)
                onChangeCharacter.Invoke();
            return true;
        }
        return false;

    }

    public void RemoveItem(int _index)
    {
        inventory_items.RemoveAt(_index);
        onChangeItem.Invoke();
    }

    public void RemoveCharacter(int _index)
    {
        inventory_characters.RemoveAt(_index);
        onChangeCharacter.Invoke();
    }




    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("FieldItem"))
        {
            FieldItems fieldItems = collision.GetComponent<FieldItems>();
            if (AddItem(fieldItems.GetItem()))
            {
                acquiredItems++;
                fieldItems.DestroyItem();
            }
        }
    }
}
