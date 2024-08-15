using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerAttack_Dash : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float dashSpeed = 15f;
    [SerializeField] float dashDuration = 0.5f;
    [SerializeField] float returnDelay = 0.1f;
    [SerializeField] float detectionRange = 10f;

    float attackTime;
    float maxAttackTime;
    bool isDashing;
    private Vector2 dashStartPosition;
    private Vector2 targetDirection;
    private Vector2 currentVelocity;

    public int spawnIndex; // 현재 사용할 spawnObjectList의 인덱스
    public FollowerSpawn followerSpawn;
    public Follower follower; // Follower 스크립트 참조
    public MobGroupMoving mobGroupMoving; // MobGroupMoving 스크립트 참조

    void Start()
    {
    }

    private void Awake()
    {
        maxAttackTime = Random.Range(3, 6);
    }

    void Update()
    {
        if (!isDashing)
        {
            DetectTarget();

            if (targetDirection != Vector2.zero)
            {
                currentVelocity = targetDirection.normalized * moveSpeed;
                transform.Translate(currentVelocity * Time.deltaTime, Space.World);
            }

            attackTime += Time.deltaTime;

            if (attackTime >= maxAttackTime && targetDirection != Vector2.zero)
            {
                StartCoroutine(DashAttackRoutine());
            }
        }
    }

    void DetectTarget()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, detectionRange);
        foreach (Collider2D collider in hitColliders)
        {
            if (collider.CompareTag("Enemy") || collider.CompareTag("Boss"))
            {
                targetDirection = collider.transform.position - transform.position;
                return;
            }
        }
        targetDirection = Vector2.zero;
    }



    IEnumerator DashAttackRoutine()
    {
        //isDashing = true;

        Transform spawnTransform = followerSpawn.GetSpawnChildTransform(spawnIndex, 0);
        if (spawnTransform != null)
        {
            dashStartPosition = spawnTransform.position;
        }
        else
        {
            dashStartPosition = transform.position; // 유효한 자식 트랜스폼이 없으면 현재 위치 사용
        }

        currentVelocity = targetDirection.normalized * dashSpeed;

        // Sine 애니메이션 멈추기
        if (follower != null) follower.SetSineActive(false);
        if (mobGroupMoving != null) mobGroupMoving.SetSineActive(false);

        // 대시 시작
        float elapsedTime = 0f;
        while (elapsedTime < dashDuration)
        {
            transform.Translate(currentVelocity * Time.deltaTime, Space.World);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(returnDelay);

        // 원래 위치로 부드럽게 돌아가기
        if (spawnTransform != null)
        {
            //transform.position = spawnTransform.position;
            while ((transform.position - spawnTransform.position).sqrMagnitude > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, spawnTransform.position, moveSpeed * Time.deltaTime);
                yield return null;
            }
        }
        //else
        //{
        //    transform.position = dashStartPosition; // 유효한 자식 트랜스폼이 없으면 대시 시작 위치로
        //}

        // Sine 애니메이션 다시 활성화
        if (follower != null) follower.SetSineActive(true);
        if (mobGroupMoving != null) mobGroupMoving.SetSineActive(true);

        maxAttackTime = Random.Range(3, 6);
        isDashing = false;
        attackTime = 0;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }


    //void DashAttack()
    //{
    //    // 현재 스폰 위치의 자식 트랜스폼을 가져오기
    //    Transform spawnTransform = followerSpawn.GetSpawnChildTransform(spawnIndex, 0);

    //    if (spawnTransform != null)
    //    {
    //        dashStartPosition = spawnTransform.position;
    //    }
    //    else
    //    {
    //        dashStartPosition = transform.position; // 유효한 자식 트랜스폼이 없으면 현재 위치 사용
    //    }

    //    currentVelocity = targetDirection.normalized * dashSpeed;

    //    // Sine 애니메이션 멈추기
    //    if (follower != null) follower.SetSineActive(false);
    //    if (mobGroupMoving != null) mobGroupMoving.SetSineActive(false);
    //}

    void ResetMove()
    {
        currentVelocity = targetDirection.normalized * moveSpeed;
    }
}
