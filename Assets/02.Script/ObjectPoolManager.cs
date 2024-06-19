using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.TextCore.Text;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager instance;
    [SerializeField] private GameObject projectilePrefab; // 오브젝트 풀링할 프리팹
    [SerializeField] private int initialPoolSize = 10; // 초기 풀 크기
    [SerializeField] private float despawnTime = 5f; // 삭제 대기 시간
    private GameObject pooledProjectileParent;

    private List<GameObject> pooledProjectiles;
    //private Dictionary<string, List<GameObject>> pooledMob;

    private bool isInitialized = false; // 초기화 여부를 나타내는 변수

    // Start is called before the first frame update
    void Start()
    {

        if (!isInitialized) // 초기화되지 않았을 때만 초기화 진행
        {
            InitializePool();
        }
    }

    // 풀 초기화 함수
    private void InitializePool()
    {
        // 초기 풀 설정
        pooledProjectiles = new List<GameObject>();
        GameObject pooledProjectileParent = new GameObject("Pooled_Projectile");
        //pooledMob = new Dictionary<string, List<GameObject>>();

        // 초기 풀 크기만큼 발사체를 생성하여 리스트에 추가하고 비활성화 상태로 설정
        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject pooledProjectile = Instantiate(projectilePrefab, Vector3.zero, Quaternion.identity);
            pooledProjectile.SetActive(false);
            pooledProjectile.transform.parent = pooledProjectileParent.transform; // 풀링 오브젝트 그룹화
            pooledProjectiles.Add(pooledProjectile);

            Destroy(pooledProjectileParent, 3.0f);
        }
        #region 몹 풀링1 

        //// 초기 풀 크기만큼 캐릭터를 생성하여 리스트에 추가하고 비활성화 상태로 설정
        //foreach (Character.CharacterData characterData in ItemDatabase.instance.characterDB)
        //{
        //    List<GameObject> mobPool = new List<GameObject>();

        //    for (int i = 0; i < initialPoolSize; i++)
        //    {
        //        GameObject mob = Instantiate(characterData.characterPrefab);
        //        mob.SetActive(false);
        //        mobPool.Add(mob);
        //    }
        //    pooledMob[characterData.name] = mobPool;
        //}
        #endregion
        isInitialized = true; // 초기화 완료
    }
    #region 몹 풀링2 
    //public GameObject GetMob(string characterName)
    //{
    //    if (pooledMob.ContainsKey(characterName))
    //    {
    //        foreach (GameObject mob in pooledMob[characterName]) 
    //        {
    //            if(!mob.activeInHierarchy)
    //            {
    //                mob.SetActive(true);
    //                return mob;
    //            }
    //        }

    //        GameObject newMob = Instantiate(ItemDatabase.instance.characterDB.Find(m => m.name == characterName).characterPrefab);
    //        pooledMob[characterName].Add(newMob);
    //        newMob.SetActive(true);
    //        return newMob;
    //    }
    //    Debug.LogWarning("No such monster found in the pool.");
    //    return null;
    //}
    //public void ReturnMob(GameObject mob)
    //{
    //    mob.SetActive(false);
    //}


    // 비활성화된 발사체를 가져오는 함수

    #endregion
    public GameObject GetProjectile()
    {
        foreach (GameObject pooledProjectile in pooledProjectiles)
        {
            if (!pooledProjectile.activeInHierarchy)
            {
                Debug.Log("비활성화 발사체 가져옴");
                pooledProjectile.SetActive(true);
                Invoke("DespawnProjectile", despawnTime); // 일정 시간 후에 발사체 삭제 예약
                return pooledProjectile;
            }
        }
        // 만약 비활성화된 발사체가 없으면 경고를 표시하고 null을 반환
        Debug.LogWarning("풀에 비활성화된 발사체가 없습니다. 초기 풀 크기를 늘려주세요.");
        return null;
    }

    // 발사체를 풀에 반환하는 함수
    public void ReturnProjectile(GameObject projectile)
    {
        if (pooledProjectiles.Contains(projectile))
        {
            projectile.SetActive(false); // 발사체를 비활성화합니다.
        }
        else
        {
            Debug.LogWarning("이 풀에는 해당 발사체가 없습니다.");
        }
    }

    // 발사체 삭제 함수
    private IEnumerator DespawnProjectile(GameObject projectile)
    {
        yield return new WaitForSeconds(despawnTime);
        ReturnProjectile(projectile);

        #region 이미 ReturnProjectile에서 처리중임
        //GameObject projectileToDespawn = null;
        //foreach (GameObject projectile in pooledProjectiles)
        //{
        //    if (projectile.activeInHierarchy)
        //    {
        //        projectileToDespawn = projectile;
        //        break;
        //    }
        //}
        //if (projectileToDespawn != null)
        //{
        //    pooledProjectiles.Remove(projectileToDespawn);
        //    Destroy(projectileToDespawn);
        #endregion
    }
}
