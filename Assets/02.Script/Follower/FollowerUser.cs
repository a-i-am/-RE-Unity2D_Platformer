using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ItemEft/FollowerSpawner")]
public abstract class FollowerUser : ScriptableObject
{
    public abstract bool ExecuteRole();
}
