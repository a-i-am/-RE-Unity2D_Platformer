using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public class FollowerSpawn : MonoBehaviour
{
    //[SerializeField] float spawnDistance = 5f;
    [SerializeField] List<GameObject> spawnObjectList;
    Transform player;

    void Start()
    {
    }

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        //Watch();
        //Follow();
    }

    void FixedUpdate()
    {

    }


    public void Spawn(Character.CharacterData characterData)
    {
        // spawnObjectList가 null인지 확인
        if (spawnObjectList == null)
        {
            Debug.LogWarning("spawnObjectList is null");
            return;
        }

        // characterData가 null인지 확인
        if (characterData == null || characterData.characterPrefab == null)
        {
            Debug.LogWarning("Character data or prefab is missing");
            return;
        }

        Debug.Log("Spawn 밑작업");
        print(spawnObjectList.Count);

        // spawnObjectList의 각 오브젝트에 자식이 있는지 확인
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
                //follower.transform.localPosition = Vector3.zero;
                break;
            }
        }
    }
}
