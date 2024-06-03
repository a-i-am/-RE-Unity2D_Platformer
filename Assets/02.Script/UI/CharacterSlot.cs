using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterSlot : MonoBehaviour, IPointerClickHandler
{
    public int characterSlotnum;
    public Character.CharacterData characterData;
    public Image characterIcon;

    public void UpdateCharacterSlotUI()
    {
        characterIcon.sprite = characterData.characterImage;
        characterIcon.gameObject.SetActive(true);

    }
    public void RemoveCharacterSlot()
    {
        characterData = null;
        characterIcon.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //bool characterIsUse = characterData.UseCharacter();

        Debug.Log("캐릭터 사용!!!");
        SpawnCharacterInField();
        Inventory.instance.RemoveCharacter(characterSlotnum);
        //if (characterIsUse) { }
    }

    private void SpawnCharacterInField()
    {
        // 플레이어의 위치를 기준으로 일정 거리 앞에 캐릭터를 소환합니다.
        Vector3 spawnPosition = Inventory.instance.playerScr.transform.position + Vector3.left * 5f;
        Instantiate(characterData.characterPrefab, spawnPosition, Quaternion.identity);
        // 가장 최근에 소환한 몹의 위치 = spawnPosition
        // 소환한 몹의 수를 변수로 반복문 돌려서 pawnPosition보다 -Nf 만큼 왼쪽에 소환 
    }

}
