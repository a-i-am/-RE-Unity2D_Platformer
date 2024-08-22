using System.Collections.Generic;
using UnityEngine;

public class FollowerAttack_Dash : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float dashSpeed = 2f;
    [SerializeField] float returnDelay = 3f;
    public float attackRange = 10f;
    public float attackCooldown = 2f;

    public FollowerSpawn followerSpawn;
    public MobGroupMoving mobGroupMoving;

    public int spawnIndex;
    private GameObject currentTarget;
    private static List<GameObject> occupiedTargets = new List<GameObject>();

    public bool isDashing = false;
    Vector3 targetPosition;

    private void Awake()
    {
        mobGroupMoving = gameObject.GetComponentInParent<MobGroupMoving>();
        followerSpawn = gameObject.GetComponentInParent<FollowerSpawn>();
    }

    private void Update()
    {
        if (currentTarget == null || Vector3.Distance(transform.position, currentTarget.transform.position) > attackRange)
        {
            FindNewTarget();

            if (!isDashing && currentTarget != null)
            {
                Dash();
            }
        }
    }

    void FindNewTarget()
    {
        GameObject[] targetEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        float closestDistance = float.MaxValue;

        foreach (GameObject target in targetEnemies)
        {
            if (!occupiedTargets.Contains(target))
            {
                float distance = Vector3.Distance(transform.position, target.transform.position);
                if (distance <= attackRange && distance < closestDistance)
                {
                    closestDistance = distance;
                    currentTarget = target;
                }
            }
        }

        if (currentTarget != null && !occupiedTargets.Contains(currentTarget))
        {
            occupiedTargets.Add(currentTarget);
        }
    }

    void Dash()
    {
        if (currentTarget != null)
        {
            Debug.Log("dash");
            isDashing = true;
            targetPosition = currentTarget.transform.position;
            transform.position = targetPosition;
        }
    }
}
