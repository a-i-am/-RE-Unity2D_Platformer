using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // 인터페이스 참조
    private IEnemyNumberCheck enemyNumberChecker;

    // 프리팹
    //private TargetingAI targetingAI;

    // 몬스터 데이터
    public Character.CharacterData characterData;
    public void SetCharacter(Character.CharacterData _character)
    {
        characterData = _character;
        //image.sprite = _character.characterImage;
    }
    public Character.CharacterData GetCharacter()
    {
        #region null 체크
        if (characterData == null)
        {
            Debug.LogError("GetCharacter returned null");
        }
        else if (characterData.characterPrefab == null)
        {
            Debug.LogError("GetCharacter returned a character with a null prefab");
        }
        #endregion
        return characterData;
    }
    public Follower GetCharacterPrefab()
    {
        return characterData.characterPrefab;
    }

    private void Awake()
    {
        enemyNumberChecker = TargetingAI.Instance;
    }

    private void OnEnable()
    {
        enemyNumberChecker?.AddActiveEnemy(this);
    }

    private void OnDisable()
    {
        enemyNumberChecker?.RemoveActiveEnemy(this);
    }

}
