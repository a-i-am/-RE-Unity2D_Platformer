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

        Debug.Log("ĳ���� ���!!!");
        SpawnCharacterInField();
        Inventory.instance.RemoveCharacter(characterSlotnum);
        //if (characterIsUse) { }
    }

    private void SpawnCharacterInField()
    {
        // �÷��̾��� ��ġ�� �������� ���� �Ÿ� �տ� ĳ���͸� ��ȯ�մϴ�.
        Vector3 spawnPosition = Inventory.instance.playerScr.transform.position + Vector3.left * 5f;
        Instantiate(characterData.characterPrefab, spawnPosition, Quaternion.identity);
        // ���� �ֱٿ� ��ȯ�� ���� ��ġ = spawnPosition
        // ��ȯ�� ���� ���� ������ �ݺ��� ������ pawnPosition���� -Nf ��ŭ ���ʿ� ��ȯ 
    }

}
