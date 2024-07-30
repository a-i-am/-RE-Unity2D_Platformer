using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.TextCore.Text;
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

    // 인벤토리 캐릭터(몹), 아이템 보유(획득)수량 표시
    public int acquiredCharacters = 0;
    public int acquiredItems = 0;
    public int pickupMobCount = 0;

    public InventoryUI invenUI;
    public EnemyScr enemy;
    public PlayerScr playerScr;

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
        playerScr = GameObject.FindWithTag("Player").GetComponent<PlayerScr>();
        ItemSlotCnt = invenUI.itemSlots.Length;
        CharacterSlotCnt = invenUI.characterSlots.Length;
    }
    private void Update()
    {
        DetectMob();
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

    public bool AddCharacter(Character.CharacterData _character)
    {
        if (inventory_characters.Count < CharacterSlotCnt)
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
        if (_index >= 0 && _index < inventory_characters.Count)
        {
            inventory_characters.RemoveAt(_index);
            onChangeCharacter.Invoke();
        }
        else Debug.LogError("Index out of range: " + _index);

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

    private void DetectMob()
    {
        // 플레이어의 앞 방향으로 레이캐스트를 발사하여 적을 감지합니다.
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, transform.right, 5f, LayerMask.GetMask("Mob"));
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, -transform.right, 5f, LayerMask.GetMask("Mob"));

        RaycastHit2D hit = hitRight.collider != null ? hitRight : hitLeft.collider != null ? hitLeft : new RaycastHit2D();

        if (hit.collider != null)
        {
            enemy = hit.collider.GetComponent<EnemyScr>();
            if (enemy != null && enemy.enemyIsFainted)
            {
                if (Input.GetKeyDown(KeyCode.C)) // Collect
                {
                    AddCharacter(enemy.GetCharacter());
                    pickupMobCount += 1;
                    Debug.Log("캐릭터 획득!");
                    enemy.DestroyCharacter();
                    acquiredCharacters++;
                    // 몹 오브젝트를 풀에 반환 후 필드의 Enemy는 삭제
                    //ObjectPoolManager.instance.ReturnMob(gameObject);
                }
            }
        }
    }





}
