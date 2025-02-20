using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ItemEft/FollowerSpawner")]
public abstract class FollowerEffect : ScriptableObject
{
    public abstract bool ExecuteRole();
}
