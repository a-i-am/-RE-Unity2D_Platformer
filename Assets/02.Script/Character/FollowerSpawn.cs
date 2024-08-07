using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FollowerSpawn : MonoBehaviour
{
    [SerializeField] List<GameObject> followerSpawnObjectList;
    public CharacterSlot slot;
    public Transform front;
    public Queue<Vector2> frontPos;
    public Vector2 followerPos;
    public int followDelay = 12;
    Transform playerTransform;
    Vector3 spawnPos;

    void Start()
    {
    }

    void Awake()
    {
        frontPos = new Queue<Vector2>();
        //spawnPosList = new List<Vector3>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
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
        //characterData = Inventory.instance.enemy.GetCharacter();
        //if (Inventory.instance == null)
        //{
        //    Debug.LogError("Inventory instance is null");
        //    return;
        //}

        //if (Inventory.instance.enemy == null)
        //{
        //    Debug.LogError("Inventory instance enemy is null");
        //    return;
        //}

        // followerSpawnObjectList가 null인지 확인
        if (followerSpawnObjectList == null)
        {
            Debug.LogWarning("followerSpawnObjectList is null");
            return;
        }

        // characterData가 null인지 확인
        if (characterData == null || characterData.characterPrefab == null)
        {
            Debug.LogWarning("Character data or prefab is missing");
            return;
        }

        Debug.Log("Spawn 밑작업");
        print(followerSpawnObjectList.Count);

        // followerSpawnObjectList의 각 오브젝트에 자식이 있는지 확인
        for (int i = 0; i < followerSpawnObjectList.Count; i++)
        {
            if (followerSpawnObjectList[i] == null)
            {
                Debug.LogWarning($"followerSpawnObjectList[{i}] is null");
                continue;
            }
            Debug.Log($"Checking followerSpawnObjectList[{i}] with child count: {followerSpawnObjectList[i].transform.childCount}");

            if (followerSpawnObjectList[i].transform.childCount == 0)
            {
                Debug.Log("Spawn 조건 통과!");
                GameObject follower = Instantiate(characterData.characterPrefab, followerSpawnObjectList[i].transform);
                follower.name = "Follower " + i;
                Debug.Log("팔로워 소환!!!");
                //follower.transform.localPosition = Vector3.zero;
                break;
            }
        }


        //if (spawnPosList.Count > 5) return;
        //int spawnPosOrder = spawnPosList.Count + 1;
        //spawnPos = player.transform.position + Vector3.left * spawnDistance * spawnPosOrder;
        ////characterData = Inventory.instance.enemy.GetCharacter();

        ////bool hasFollowerChild = false;

        //for (int i = 0; i < player.childCount; i++)
        //{
        //    Transform child = player.GetChild(i);
        //    Debug.Log("자식 오브젝트를 가져옵니다.");

        //    //if (child.CompareTag("Follower"))
        //    if (child.name == "MobSpawnPosition_" + spawnPosOrder)
        //    {
        //        Debug.Log("몹 소환!!!");
        //        GameObject FollowerPrefab = Instantiate(Inventory.instance.enemy.characterData.characterPrefab);
        //        //FollowerPrefab.transform.SetParent(parent.transform);
        //        FollowerPrefab.name = "Follower " + spawnPosOrder;
        //        spawnPosList.Add(spawnPos);
        //        //hasFollowerChild = true;
        //        //break;
        //    }
        //}
    }
    //if (!hasFollowerChild)
    //{
    //    Debug.Log("몹 소환!!!");
    //    GameObject FollowerPrefab = Instantiate(Inventory.instance.enemy.characterData.characterPrefab);
    //    FollowerPrefab.transform.SetParent(parent.transform);
    //    FollowerPrefab.name = "Follower " + spawnPosOrder;
    //    spawnPosList.Add(spawnPos);
    //    //if(transform.name == "MobSpawnPosition_" + spawnPosOrder)
    //    //GameObject FollowerPrefab = Instantiate(Inventory.instance.enemy.characterData.characterPrefab, spawnPos, Quaternion.identity);
    //}


    // frontPos = Instantiate(Inventory.instance.enemy.characterData.characterPrefab, spawnPos, Quaternion.identity);
    //if (Inventory.instance.enemy.characterData.characterPrefab != null && Inventory.instance.frontPos != null)
    //{
    //    GameObject instantiatedPrefab = Instantiate(Inventory.instance.enemy.characterData.characterPrefab);
    //    instantiatedPrefab.transform.SetParent(Inventory.instance.frontPos.transform);
    //}

    //void Watch()
    //{
    //    //#.Input Pos
    //    // 부모 위치가 가만히 있으면 저장하지 않도록 조건 추가
    //    if (frontPos.Contains(front.position))
    //        frontPos.Enqueue(front.position);

    //    //#.Output Pos
    //    // 큐에 일정 개수 데이터가 채워지면 그 때부터 반환
    //    if (frontPos.Count > followDelay)
    //        followerPos = frontPos.Dequeue();
    //    // 큐가 채워지기 전까진 부모 위치 적용
    //    else if (frontPos.Count < followDelay)
    //        followerPos = front.position;
    //}

    //void Follow()
    //{
    //    transform.position = followerPos;
    //}
}
