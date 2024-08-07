using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FollowUser/FollowSpawner")]
public class FollowerSpawner : FollowerUser
{
    public Character.CharacterData characterData;

    //public Character.CharacterData characterData;
    public override bool ExecuteRole()
    {
        //Inventory.instance.enemy.GetCharacter();
        Debug.Log("GetCharacter!!!");
        return true;
    }

    
}
