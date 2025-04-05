using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFollowerNumberCheck
{
    public void AddFollower(Follower follower);
    public void RemoveFollower(Follower follower);

    public bool IsFollowerRegistered(Follower follower);
}
