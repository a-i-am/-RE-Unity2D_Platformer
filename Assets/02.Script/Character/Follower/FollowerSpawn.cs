using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerSpawn : MonoBehaviour
{
    public List<GameObject> spawnObjectList;
    //public CharacterSlot slot;
    Transform playerTransform;

    void Awake()
    {
        //playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        //frontPos = new Queue<Vector2>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void Start()
    {
    }

    void Update()
    {
    }

    void FixedUpdate()
    {
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

                //FollowerAttack_Dash followerAttack = follower.GetComponent<FollowerAttack_Dash>();
                //if (followerAttack != null)
                //{
                //    followerAttack.followerSpawn = this;
                //    followerAttack.spawnIndex = i;
                //    Debug.Log($"팔로워 {i}가 스폰 위치 {i}에 소환되었습니다.");

                //}
                break;
            }
        }
    }

    //public Transform GetSpawnChildTransform(int spawnIndex)
    //{
    //    if (spawnIndex >= 0 && spawnIndex < spawnObjectList.Count)
    //    {
    //        GameObject parentObject = spawnObjectList[spawnIndex];
    //        if (parentObject.transform.childCount > 0)
    //        {
    //            return parentObject.transform;
    //            //return parentObject.transform.GetChild(childIndex);
    //        }
    //    }
    //    return null;
    //}
}
