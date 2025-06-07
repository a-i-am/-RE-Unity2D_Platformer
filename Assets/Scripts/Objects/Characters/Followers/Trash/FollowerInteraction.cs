//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.SceneManagement;

//public class FollowerInteraction : MonoBehaviour
//{
//    private InteractionCollections collections;
//    private Dictionary<int, EnemyScr> _wantedEnemies;
//    private Dictionary<int, EnemyScr> _enteredEnemies;
//    private Dictionary<int, Follower> _followers;

//    public Action<int, Follower> OnFollowerAdded;
//    public Action<int> OnFollowerRemoved;

//    private void OnEnable()
//    {
//        _wantedEnemies = collections.WantedEnemies;
//        _followers = collections.Followers;
//    }
//    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
//    {
//        _wantedEnemies.Clear();
//        _enteredEnemies.Clear();
//    }
//    void OnDisable()
//    {
//        SceneManager.sceneLoaded -= OnSceneLoaded;
//    }

//    private void Start()
//    {
//        SceneManager.sceneLoaded += OnSceneLoaded;
//    }



//    // 그룹 개체 콜라이더
//    private void OnTriggerEnter2D(Collider2D other)
//    {
//        int enemyId = other.GetInstanceID();

//        if (_wantedEnemies.ContainsKey(enemyId))
//        {
//            // _enteredEnemies에 적 추가
//            _enteredEnemies[enemyId] = _wantedEnemies[enemyId];

//            // KDTree를 갱신
//            kDTreeUtility.BuildTree(new Dictionary<int, EnemyScr>(_enteredEnemies));
//        }
//    }

//    private void OnTriggerExit2D(Collider2D other)
//    {
//        int enemyId = other.GetInstanceID();  // 적의 ID를 얻음

//        if (_enteredEnemies.ContainsKey(enemyId))
//        {
//            // _enteredEnemies에서 해당 적 제거
//            _enteredEnemies.Remove(enemyId);

//            // KDTree를 갱신
//            kDTreeUtility.BuildTree(new Dictionary<int, EnemyScr>(_enteredEnemies));
//        }
//    }

//}