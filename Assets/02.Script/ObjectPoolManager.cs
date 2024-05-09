using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab; // 오브젝트 풀링할 프리팹
    [SerializeField] private int initialPoolSize = 10; // 초기 풀 크기
    [SerializeField] private float despawnTime = 5f; // 삭제 대기 시간

    private List<GameObject> pooledProjectiles;
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

        // 초기 풀 크기만큼 발사체를 생성하여 리스트에 추가하고 비활성화 상태로 설정
        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject pooledProjectile = Instantiate(projectilePrefab, Vector3.zero, Quaternion.identity);
            pooledProjectile.SetActive(false);
            pooledProjectiles.Add(pooledProjectile);
        }

        isInitialized = true; // 초기화 완료
    }

    // 비활성화된 발사체를 가져오는 함수
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
    private void DespawnProjectile()
    {
        GameObject projectileToDespawn = null;
        foreach (GameObject projectile in pooledProjectiles)
        {
            if (projectile.activeInHierarchy)
            {
                projectileToDespawn = projectile;
                break;
            }
        }
        if (projectileToDespawn != null)
        {
            pooledProjectiles.Remove(projectileToDespawn);
            Destroy(projectileToDespawn);
        }
    }


    // Update is called once per frame
    void Update()
    {

    }
}
