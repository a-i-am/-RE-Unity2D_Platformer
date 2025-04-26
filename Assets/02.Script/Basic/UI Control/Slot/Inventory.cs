using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.TextCore.Text;
using static Inventory;
public class Inventory : Singleton<Inventory>
{
    EnemyState enemyState;
    public delegate void OnItemSlotCountChange(int val);
    public delegate void OnCharacterSlotCountChange(int val);

    public OnItemSlotCountChange onItemSlotCountChange;
    public OnCharacterSlotCountChange onCharacterSlotCountChange;

    public delegate void OnChangeItem();
    public OnChangeItem onChangeItem;
    public delegate void OnChangeCharacter();
    public OnChangeCharacter onChangeCharacter;
    public List<Character.CharacterData> characters = new List<Character.CharacterData>();
    public List<Item.ItemData> items = new List<Item.ItemData>();

    // 인벤토리 캐릭터(몹), 아이템 보유(획득)수량 표시
    public int acquiredCharacters = 0;
    public int acquiredItems = 0;
    public int pickupMobCount = 0;

    public InventoryUI invenUI;
    private PlayerScr playerScr;
    private Enemy enemy;

    public List<FollowerController> activeFollowers;

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

    // 팔로워 로직
    private bool isNotHaveFollower;

    private void Awake()
    {
        playerScr = GetComponent<PlayerScr>();
    }
    void Start()
    {
        //ItemSlotCnt = invenUI.itemSlots.Length;
        //CharacterSlotCnt = invenUI.characterSlots.Length;
        ItemSlotCnt = 4;
        CharacterSlotCnt = 5;
    }
    private void Update()
    {
        DetectMob();
    }

    public bool AddItem(Item.ItemData _item) // ItemData _item
    {
        if (items.Count < ItemSlotCnt)
        {
            items.Add(_item);
            if (onChangeItem != null)
                onChangeItem.Invoke();
            return true;
        }
        return false;

    }

    public bool AddCharacter(Character.CharacterData _character)
    {
        if (characters.Count < CharacterSlotCnt)
        {
            characters.Add(_character);
            
            if (onChangeCharacter != null)
                onChangeCharacter.Invoke();

            return true;
        }
        return false;

    }

    public void RemoveItem(int _index)
    {
        items.RemoveAt(_index);
        onChangeItem.Invoke();
        acquiredItems--;
    }

    public void RemoveCharacter(int _index)
    {
        if (_index >= 0 && _index < characters.Count)
        {
            characters.RemoveAt(_index);
            onChangeCharacter.Invoke();
            acquiredCharacters--;
            Debug.Log("RemoveCharacter");
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
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, transform.right, 5f, LayerMask.GetMask("Fainted"));
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, -transform.right, 5f, LayerMask.GetMask("Fainted"));

        RaycastHit2D hit = hitRight.collider != null ? hitRight : hitLeft.collider != null ? hitLeft : new RaycastHit2D();

        if (hit.collider != null)
        {
            enemy = hit.collider.GetComponent<Enemy>();
            if (enemy != null && enemyState != EnemyState.Fainted)
            {
                if (Input.GetKeyDown(KeyCode.V)) // Collect
                {
                    AddCharacter(enemy.GetCharacter());
                    pickupMobCount += 1;

                    if (enemy != null)
                    {
                        Destroy(enemy.gameObject);
                        acquiredCharacters++;
                        Debug.Log("enemy 획득!");
                    }
                }
            }
        }
    }
}
