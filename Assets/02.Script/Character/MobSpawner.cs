using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobSpawner : MonoBehaviour
{
    public Character.CharacterData characterData;

    //public void SpawnMob(string characterName, Vector3 position)
    //{
    //    GameObject mob = ObjectPoolManager.instance.GetMob(characterName);
    //    if (mob != null)
    //    {
    //        mob.transform.position = position;
    //        mob.GetComponent<Character>().Initialize(ItemDatabase.instance.characterDB.Find(m => m.name == characterName));
    //    }

    //}
}