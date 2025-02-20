using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "FollowerEft/Attack")]
public class FollowerEffect_Spawn : FollowerEffect
{
    public override bool ExecuteRole()
    {
        Debug.Log("팔로워 소환 효과 테스트 - 버프");
        return true;
    }
}
