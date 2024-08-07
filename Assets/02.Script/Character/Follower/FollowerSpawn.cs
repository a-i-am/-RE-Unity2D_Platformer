using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerSpawn : MonoBehaviour
{
    [SerializeField] List<GameObject> spawnObjectList;
    public CharacterSlot slot;
    public Transform front;
    public Queue<Vector2> frontPos;
    public Vector2 followerPos;
    public int followDelay = 12;
    Transform playerTransform;
    Vector3 spawnPos;

    void Awake()
    {
        frontPos = new Queue<Vector2>();
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

        Debug.Log("Spawn 밑작업");
        print(spawnObjectList.Count);

        for (int i = 0; i < spawnObjectList.Count; i++)
        {
            if (spawnObjectList[i] == null)
            {
                Debug.LogWarning($"spawnObjectList[{i}] is null");
                continue;
            }
            Debug.Log($"Checking spawnObjectList[{i}] with child count: {spawnObjectList[i].transform.childCount}");

            if (spawnObjectList[i].transform.childCount == 0)
            {
                Debug.Log("Spawn 조건 통과!");
                GameObject follower = Instantiate(characterData.characterPrefab, spawnObjectList[i].transform);
                follower.name = "Follower " + i;
                Debug.Log("팔로워 소환!!!");
                break;
            }
        }
    }
}
