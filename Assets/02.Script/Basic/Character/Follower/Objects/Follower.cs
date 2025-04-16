using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Follower : MonoBehaviour
{
    [Header("외부 참조")]
    private FollowerState followerState;
    
    // 인터페이스 참조
    private IFollowerNumberCheck followerNumChecker;
    private IFollowerTargetReceivable followerTargetReceivable;
    private IFollowerAttackable followerAttackable;

    // 프리팹
    [SerializeField] private FollowerController followerController;
    //[SerializeField] private TargetingAI targetingAI;

    private void Awake()
    {
        // 의존성 주입: 내부에서 싱글톤 통해 참조 획득
        followerTargetReceivable = followerController;
        followerAttackable = followerController;
        followerNumChecker = TargetingAI.Instance;
    }

    private void OnEnable()
    {
        if (followerNumChecker == null)
        {
            return;
        }

        // **중복 등록 방지**
        if (!followerNumChecker.IsFollowerRegistered(this))
        {
            followerNumChecker.AddFollower(this);
        }
    }

    private void OnDisable()
    {
        followerNumChecker?.RemoveFollower(this);
    }

    public bool IsDashCheck()
    {
        return followerController.IsDashCheck();
    }


    public void SetTarget(Enemy target)
    {
        followerTargetReceivable?.SetTarget(target);
    }

    public void CallDashAttack()
    {
        followerAttackable?.DashAndReturn();
    }
}
