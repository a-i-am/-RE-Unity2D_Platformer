using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEft/MobSpawner/CharacterAbility")]
public abstract class CharacterAbility : ItemEffect
{
    public override bool ExecuteRole()
    {

        return true;
    }
}
