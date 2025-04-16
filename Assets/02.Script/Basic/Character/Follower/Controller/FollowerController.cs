using System;
using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.UIElements;
using System.Buffers.Text;

public class FollowerController : MonoBehaviour, IFollowerTargetReceivable, IFollowerAttackable
{
    [Header("외부 참조")]
    [SerializeField] private FollowerGroupMoving followerGroupMoving;
    [HideInInspector]public int followerIndex; 
    private float spacing = 3f;
    private FollowerState followerState;

    [Header("Transform 데이터")]
    private Transform player;
    [SerializeField] private Transform follower;
    [Header("대시와 리턴")]
    private Vector3 originalPos;
    private Vector3 groupPos;
    private int dashCount = 0;
    private float baseY;
    private float baseX;
    private bool isDashing = false;
    [SerializeField] private float detectionRange;
    [SerializeField] private float dashDuration; // dash 속도 조절
    
    // 타겟팅
    private Enemy _currentTarget;
    public Enemy CurrentTarget => _currentTarget;

    private Vector2 _targetPos;
    public Vector2 TargetPosition => _currentTarget != null ? _currentTarget.transform.position : Vector2.zero;
    public void SetTarget(Enemy target)
    {
        _currentTarget = target;
    }

    // 렌더링
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        TryGetComponent(out spriteRenderer);
    }

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;

    }
    private void FixedUpdate()
    {
        spriteRenderer.flipX = (transform.position.x < player.position.x) ? true : false;
    }

    public bool IsDashCheck()
    {
        return !isDashing;
    }

    public void DashAndReturn()
    {
        if (isDashing && _currentTarget == null) return;
        #region 디버깅(null 체크)
        if (_currentTarget == null)
        {
            Debug.LogWarning("DashAndReturn - target이 null!");
            return;
        }
        if (follower == null)
        {
            Debug.LogWarning("DashAndReturn - follower가 null!");
            return;
        }
        if (followerGroupMoving == null)
        {
            Debug.LogWarning("DashAndReturn - followerGroupMoving이 null!");
            return;
        }
        #endregion
        isDashing = true; // 동작 종료 전 재실행 불가

        // 타겟팅 상태 설정
        followerState = FollowerState.Targeting;
        _targetPos = TargetPosition;

        followerGroupMoving.isSineActive = false;

        // 팔로워 위치 & 움직임 조정
        groupPos = transform.parent.position;
        baseY = followerGroupMoving.transform.localPosition.y;
        baseX = groupPos.x + followerIndex * -spacing;
        originalPos = new Vector3(baseX, baseY, groupPos.z);

        Sequence seq = DOTween.Sequence();
        seq.Append(follower.DOMove(_targetPos, dashDuration))
           .AppendInterval(0.5f)
           .Append(follower.DOMove(originalPos, dashDuration))
           .AppendInterval(0.5f)
           .OnComplete(() =>
           {
               _currentTarget = null;
               followerGroupMoving.isSineActive = true;
               isDashing = false;
               TargetingAI.Instance.ClearTargetHashSet();
           })
           .Play();

        ++dashCount;
        Debug.Log(dashCount);

    }
}
