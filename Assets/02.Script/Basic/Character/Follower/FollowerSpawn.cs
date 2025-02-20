using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerSpawn : MonoBehaviour
{
    public List<GameObject> spawnObjectList;
    Transform playerTransform;

    void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }
    public void Spawn(Character.CharacterData characterData)
    {
        if (spawnObjectList == null)
        {
            Debug.LogWarning("spawnObjectList is null");
            return;
        }

        if (characterData == null || characterData.characterPrefab == null)
        {
            Debug.LogWarning("Character data or prefab is missing");
            return;
        }

        for (int i = 0; i < spawnObjectList.Count; i++)
        {
            if (spawnObjectList[i] == null)
            {
                Debug.LogWarning($"spawnObjectList[{i}] is null");
                continue;
            }

            if (spawnObjectList[i].transform.childCount == 0)
            {
                GameObject follower = Instantiate(characterData.characterPrefab, spawnObjectList[i].transform);
                follower.name = "Follower " + i;
                break;
            }
        }
    }

}
