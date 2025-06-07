//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class InteractionCollections
//{
//    // Enemies, Followers     // 삽입 삭제가 빈번한 자료들
//    public Dictionary<int, Follower> Followers { get; private set; }
//    public Dictionary<int, EnemyScr> WantedEnemies { get; private set; }
//    public Dictionary<int, EnemyScr> EnteredEnemies { get; private set; }
//    public Dictionary<GameObject, Color> FollowerRayMap { get; private set; }

//    public InteractionCollections()
//    {
//        Followers = new Dictionary<int, Follower>();
//        EnteredEnemies = new Dictionary<int, EnemyScr>();
//        WantedEnemies = new Dictionary<int, EnemyScr>();
//        FollowerRayMap = new Dictionary<GameObject, Color>();
//    }

//    public void AddFollower(int id, Follower follower) { Followers.Add(id, follower); }
//    public void RemoveFollower(int id, Follower follower) { Followers.Remove(id, follower); }
//    public void AddWantedEnemy(int id, EnemyScr wantedEnemy) { WantedEnemies.Add(id, wantedEnemy); }
//    public void RemoveWantedEnemy(int id, EnemyScr wantedEnemy) { WantedEnemies.Remove(id, out wantedEnemy); }
//    public void AddEnteredEnemy(int id, EnemyScr enteredEnemy) { EnteredEnemies.Add(id, enteredEnemy); }
//    public void RemoveEnteredEnemy(int id, EnemyScr enteredEnemy) { EnteredEnemies.Remove(id, enteredEnemy); }

//}
