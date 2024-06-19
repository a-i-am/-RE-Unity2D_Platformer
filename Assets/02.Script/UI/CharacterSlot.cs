using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterSlot : MonoBehaviour, IPointerUpHandler, IPointerClickHandler
{
    public int characterSlotnum;
    public Character.CharacterData characterData;
    public Image characterIcon;
    public Transform player;

    FollowerSpawn followerSpawns;
    //List<FollowerSpawn> sortedSpawns;
    //public int spawnsOrder;

    // 소환된 몹들의 위치를 저장할 리스트
    //public List<Vector3> List_spawnPositions = new List<Vector3>();
    //private int lastSpawnedIndex = 0; // 마지막으로 Spawn()이 호출된 오브젝트의 번호를 저장

    private void Start()
    {
        // FollowerSpawn 스크립트를 포함한 모든 오브젝트를 찾습니다.
        followerSpawns = FindObjectOfType<FollowerSpawn>();
        // 오브젝트의 이름과 매핑되는 인덱스를 정렬된 리스트로 만듭니다.
        //sortedSpawns = new List<FollowerSpawn>();
        //spawnsOrder = sortedSpawns.Count + 1;

        player = GameObject.FindGameObjectWithTag("Player").transform;

        //firstSpawnPosition = Inventory.instance.playerScr.transform.position + Vector3.left * 5f;
        //lastSpawnPosition = firstSpawnPosition;
        //Physics2D.IgnoreLayerCollision(9, 9); // Mob(9) 끼리의 충돌 무시
    }

    public void UpdateCharacterSlotUI()
    {
        if (characterData != null)
        {
            characterIcon.sprite = characterData.characterImage;
            characterIcon.gameObject.SetActive(true);
        }
    }

    public void RemoveCharacterSlot()
    {
        characterData = null;
        characterIcon.gameObject.SetActive(false);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        bool characterIsUse = characterData.UseCharacter();

        if (characterIsUse)
        {
            Inventory.instance.RemoveItem(characterSlotnum);
            Inventory.instance.acquiredCharacters--;
            
        }

    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (followerSpawns == null)
        {
            Debug.LogError("followerSpawns is null");
            return;
        }
        
        CharacterSlot characterSlot = GetComponent<CharacterSlot>();
        if (characterSlot != null)
        {
            // CharacterSlot 컴포넌트의 characterData에 접근하여 처리
            Character.CharacterData characterData = characterSlot.characterData;
            followerSpawns.Spawn(characterData);
            Inventory.instance.RemoveCharacter(characterSlotnum);
        }
        
        //    foreach (FollowerSpawn followerSpawn in followerSpawns)
        //if (sortedSpawns.Count > 5) return;


        //// followerSpawns
        //// sortedSpawns
        //{
        //    if (followerSpawn.gameObject.name == "MobSpawnPosition_"+spawnsOrder)
        //    {
        //        Debug.Log("MobSpawnPosition_검색");       
        //        sortedSpawns.Add(followerSpawn);
        //        // 다음 오브젝트의 Spawn() 메서드를 호출합니다.
        //        if (sortedSpawns.Count > 0)
        //        {
        //            sortedSpawns[lastSpawnedIndex].Spawn();
        //            lastSpawnedIndex++;
        //            Debug.Log("Spawn() 호출");
        //            //lastSpawnedIndex = (lastSpawnedIndex + 1) % sortedSpawns.Count;
        //        }
        //    }
        //}


    }

    //void RemoveSpawnSlot()
    //{
    //    //lastSpawnedIndex--;
    //} 

}