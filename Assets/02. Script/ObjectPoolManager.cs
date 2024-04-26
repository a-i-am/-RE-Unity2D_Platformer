using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab; // ������Ʈ Ǯ���� ������
    [SerializeField] private int initialPoolSize = 10; // �ʱ� Ǯ ũ��
    [SerializeField] private float despawnTime = 5f; // ���� ��� �ð�

    private List<GameObject> pooledProjectiles;
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

        // �ʱ� Ǯ ũ�⸸ŭ �߻�ü�� �����Ͽ� ����Ʈ�� �߰��ϰ� ��Ȱ��ȭ ���·� ����
        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject pooledProjectile = Instantiate(projectilePrefab, Vector3.zero, Quaternion.identity);
            pooledProjectile.SetActive(false);
            pooledProjectiles.Add(pooledProjectile);
        }

        isInitialized = true; // �ʱ�ȭ �Ϸ�
    }

    // ��Ȱ��ȭ�� �߻�ü�� �������� �Լ�
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


    // Update is called once per frame
    void Update()
    {

    }
}
