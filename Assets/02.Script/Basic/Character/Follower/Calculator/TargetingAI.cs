using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static UnityEngine.EventSystems.EventTrigger;
public class TargetingAI : Singleton<TargetingAI>, IFollowerNumberCheck, IEnemyNumberCheck
{
    /// <summary>
    // 살아있는 모든 followers와 Enemies = Active로 네이밍
    /// </summary>

    // Followers
    // 리스트: 순서 변경과 중간 삽입/삭제 O(n)으로 가능, Max값 낮게 설정할거라 영향 미미
    private List<Follower> _activeFollowers = new List<Follower>();   

    // Enemies 
    // 해시셋: 순서 변경 불가, 동적인 삽입/삭제
    private HashSet<Enemy> _activeEnemies = new HashSet<Enemy>();

    // 거리 기반 타겟팅된 타겟 후보들 
    private HashSet<Enemy> _detectedEnemies = new HashSet<Enemy>();
    private SortedSet<(float, Enemy)> _targetCandidates = new SortedSet<(float, Enemy)>(new TargetComparer());
    private HashSet<Enemy> _targetHashSet = new HashSet<Enemy>();

    private Dictionary<Collider2D, Enemy> _enemyCache = new Dictionary<Collider2D, Enemy>();

    public bool IsFollowerRegistered(Follower follower)
    {
        return _activeFollowers.Contains(follower);
    }

    public void AddFollower(Follower follower)
    {
        if (!IsFollowerRegistered(follower))
        {
            _activeFollowers.Add(follower);
        }
    }

    public void RemoveFollower(Follower follower)
    {
        if(follower == null) return;
        _activeFollowers.Remove(follower);
    }

    // 씬 로드 시에만 실행
    //private void ClearEnemiesCount()
    //{
    //    _activeEnemies.Clear();
    //}

    public void ClearTargetHashSet()
    {
        _targetHashSet.Clear();
    }

    public void AddActiveEnemy(Enemy enemy)
    {
        _activeEnemies.Add(enemy);
    }
    
    // 비활성화되거나 Faint()된 Enemy를 타겟팅 대상에서 제외
    public void RemoveActiveEnemy(Enemy enemy)
    {
        _activeEnemies.Remove(enemy);
        _targetHashSet.Remove(enemy);
    }

    public void EnterEnemy(Enemy enemy)
    {
        if (!_activeEnemies.Contains(enemy) || _detectedEnemies.Contains(enemy)) return;
        _detectedEnemies.Add(enemy);

        // 델리게이트 or 이벤트 실행해서, 갱신된 타겟 후보 데이터 전달
    }
    public void ExitEnemy(Enemy enemy)
    {
        if (!_activeEnemies.Contains(enemy) || !_detectedEnemies.Contains(enemy)) return;
        
        _detectedEnemies.Remove(enemy);
        _targetHashSet.Remove(enemy);
    }

    private Enemy GetEnemyFromCollider(Collider2D collider)
    {
        if(collider == null) return null;

        if(_enemyCache.TryGetValue(collider, out Enemy enemy))
        {
            return enemy;
        }

        if (collider.TryGetComponent(out enemy))
        {
            _enemyCache[collider] = enemy; // 캐싱
            return enemy;
        }

        return null;
    }

    private void PrintDebugState()
    {
        Debug.Log($"[TargetingAI] ActiveFollowers: {_activeFollowers.Count}, ActiveEnemies: {_activeEnemies.Count}, DetectedEnemies: {_detectedEnemies.Count}");
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        //PrintDebugState();

        if (_activeFollowers.Count < 1 || _activeEnemies.Count < 1) return;

        Enemy enemy = GetEnemyFromCollider(other);
        // enemy != null && _activeFollowers.Count > 0
        if (_activeEnemies.Count > 0 && enemy != null && !enemy.isFainted)
            EnterEnemy(enemy);

        if (_detectedEnemies.Count == 0) return;
            CalculateTargets();
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (_activeFollowers.Count < 1 || _activeEnemies.Count < 1) return;

        Enemy enemy = GetEnemyFromCollider(other);
        
        // enemy != null && !enemyController.isFainted && 
            Debug.Log("EnterEnemy 호출");
        if (_activeEnemies.Count > 0 && enemy != null && !enemy.isFainted)
            ExitEnemy(enemy);
    }
   



    // 실시간 거리 기반 타겟 계산
    public void CalculateTargets()
    {
        _targetCandidates.Clear();

        foreach (Enemy enemy in _detectedEnemies)
        {
            if (enemy == null) continue;
            foreach (Follower follower in _activeFollowers)
            {
                if (follower == null) continue;
                float dist = Vector2.Distance(follower.transform.position, enemy.transform.position);
                if (!_targetHashSet.Contains(enemy) && !enemy.isFainted)
                {
                    _targetCandidates.Add((dist, enemy));
                }
            }
        }
        AssignTargets();
    }

    private void AssignTargets()
    {
        foreach (Follower follower in _activeFollowers)
        {

            if (follower == null || follower.IsDashCheck())
                continue;

            foreach (var candidate in _targetCandidates)
            {
                if (!_targetHashSet.Contains(candidate.Item2))
                {
                    follower.SetTarget(candidate.Item2);
                    _targetHashSet.Add(candidate.Item2);
                    follower.CallDashAttack();
                    break;
                }
            }
        }
    }

    private class TargetComparer : IComparer<(float, Enemy)>
    {
        public int Compare((float, Enemy) x, (float, Enemy) y)
        {
            // 거리 비교 결과, x가 가까우면(값이 더 작으면) -1, y가 가까우면 +1
            int result = x.Item1.CompareTo(y.Item1);
            if (result == 0)
            {
                // 거리가 같을 경우, GetInstanceID() 값이 작은 순서로 우선 정렬
                // 인스턴스 ID는 런타임 중 생성된 객체마다 다르게 부여돼서, 서로 다른 오브젝트를 식별
                return x.Item2.GetInstanceID().CompareTo(y.Item2.GetInstanceID());
            }
            return result;
        }
    }
}
