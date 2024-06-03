using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.TextCore.Text;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager instance;

    [SerializeField] private GameObject projectilePrefab; // ������Ʈ Ǯ���� ������
    [SerializeField] private int initialPoolSize = 10; // �ʱ� Ǯ ũ��
    [SerializeField] private float despawnTime = 5f; // ���� ��� �ð�

    private List<GameObject> pooledProjectiles;
    private Dictionary<string, List<GameObject>> pooledMob;
    
    private bool isInitialized = false; // �ʱ�ȭ ���θ� ��Ÿ���� ����

    // Start is called before the first frame update
    void Start()
    {
        if (!isInitialized) // �ʱ�ȭ���� �ʾ��� ���� �ʱ�ȭ ����
        {
            InitializePool();
        }
    }

    // Ǯ �ʱ�ȭ �Լ�
    private void InitializePool()
    {
        // �ʱ� Ǯ ����
        pooledProjectiles = new List<GameObject>();
        pooledMob = new Dictionary<string, List<GameObject>>();

        // �ʱ� Ǯ ũ�⸸ŭ �߻�ü�� �����Ͽ� ����Ʈ�� �߰��ϰ� ��Ȱ��ȭ ���·� ����
        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject pooledProjectile = Instantiate(projectilePrefab, Vector3.zero, Quaternion.identity);
            pooledProjectile.SetActive(false);
            pooledProjectiles.Add(pooledProjectile);
        }
        #region �� Ǯ��1 

        //// �ʱ� Ǯ ũ�⸸ŭ ĳ���͸� �����Ͽ� ����Ʈ�� �߰��ϰ� ��Ȱ��ȭ ���·� ����
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
        isInitialized = true; // �ʱ�ȭ �Ϸ�
    }
    #region �� Ǯ��2 
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


    // ��Ȱ��ȭ�� �߻�ü�� �������� �Լ�

    #endregion
    public GameObject GetProjectile()
    {
        foreach (GameObject pooledProjectile in pooledProjectiles)
        {
            if (!pooledProjectile.activeInHierarchy)
            {
                Debug.Log("��Ȱ��ȭ �߻�ü ������");
                pooledProjectile.SetActive(true);
                Invoke("DespawnProjectile", despawnTime); // ���� �ð� �Ŀ� �߻�ü ���� ����
                return pooledProjectile;
            }
        }
        // ���� ��Ȱ��ȭ�� �߻�ü�� ������ ��� ǥ���ϰ� null�� ��ȯ
        Debug.LogWarning("Ǯ�� ��Ȱ��ȭ�� �߻�ü�� �����ϴ�. �ʱ� Ǯ ũ�⸦ �÷��ּ���.");
        return null;
    }

    // �߻�ü�� Ǯ�� ��ȯ�ϴ� �Լ�
    public void ReturnProjectile(GameObject projectile)
    {
        if (pooledProjectiles.Contains(projectile))
        {
            projectile.SetActive(false); // �߻�ü�� ��Ȱ��ȭ�մϴ�.
        }
        else
        {
            Debug.LogWarning("�� Ǯ���� �ش� �߻�ü�� �����ϴ�.");
        }
    }

    // �߻�ü ���� �Լ�
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
}
