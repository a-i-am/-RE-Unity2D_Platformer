using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    [Header("아이템 정보")]
    public int itemSlotnum;
    public Item.ItemData itemData;
    public Image itemIcon;

    [Header("외부 참조")]
    private Pointable pointable;
    private InventoryUI inventoryUI;

    private void Awake()
    {
        inventoryUI = GetComponentInParent<InventoryUI>();
        pointable = GetComponent<Pointable>();
        if (pointable != null)
        {
            pointable.OnClick = OnClick;
            pointable.OnPointerUpAction = OnPointerUp;
        }
    }

    public void OnPointerUp()
    {
        // 드래그 & 드롭
        if (itemData == null) return;
    }

    public void OnClick()
    {
        // 슬롯 클릭 트리거
        if (itemData == null) return;
        
        if (inventoryUI != null)
        {
            //inventoryUI.RemoveItemSlotAt(itemSlotnum);
            //Inventory.Instance.RemoveItem(itemSlotnum);
            //Inventory.Instance.acquiredItems--;
        }
    }


    public void UpdateItemSlotUI()
    {
        if (itemData != null)
        {
            itemIcon.sprite = itemData.itemImage;
            itemIcon.gameObject.SetActive(true);
        }
    }

    public void RemoveItemSlot()
    {
        if (itemData != null)
        {
            itemData = null;
            itemIcon.gameObject.SetActive(false);
        }
    }


}

