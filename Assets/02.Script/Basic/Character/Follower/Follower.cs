using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    // Dash & Return
    [SerializeField] float detectionRange;
    [SerializeField] float dashDuration; // dash 속도 조절

    private MobGroupMoving mobGroupMoving;
    public Transform returnPos; // 인스펙터에서 각 Follower의 returnPos 할당
    private Dictionary<GameObject, Vector3> originalPositions; // Follower들의 원래 위치 저장

    public bool isDashing = false;
    public float attackRange = 5f;
    Animator anim;
    LayerMask groundLayer;

    // Enemy Targeting
    private List<GameObject> selfObjects;  // 자신들에 해당하는 개체들 리스트
    private List<GameObject> targetObjects;  // B 태그에 해당하는 개체들 리스트

    private KDTree selfKDTree;
    private KDTree targetKDTree;
    private HashSet<GameObject> targetedObjects;  // 타겟팅된 개체들 기록

    // Color Array for different colors
    private Color[] colors;
    private Dictionary<GameObject, Color> objectColorMap;

    private void Start()
    {
        mobGroupMoving = gameObject.GetComponentInParent<MobGroupMoving>();
        selfObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag("Follower"));  // 자신들의 태그

        targetObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));  // Enemy 태그에 해당하는 개체들의 태그

        selfKDTree = new KDTree();
        targetKDTree = new KDTree();
        targetedObjects = new HashSet<GameObject>();
        originalPositions = new Dictionary<GameObject, Vector3>();

        // 자신들의 위치로 K-D 트리 초기화
        foreach (var obj in selfObjects)
        {
            selfKDTree.Insert(obj.transform.position);
        }

        // Enemy 태그 개체들 위치로 K-D 트리 초기화
        foreach (var obj in targetObjects)
        {
            targetKDTree.Insert(obj.transform.position);
        }

        // Define colors (You can add more colors or generate them dynamically)
        colors = new Color[] { Color.red, Color.green, Color.blue, Color.yellow, Color.magenta, Color.cyan, Color.white, Color.black };
        objectColorMap = new Dictionary<GameObject, Color>();

        int colorIndex = 0;
        foreach (var obj in selfObjects)
        {
            // Assign a color to each follower
            objectColorMap[obj] = colors[colorIndex % colors.Length];
            originalPositions[obj] = gameObject.GetComponent<Follower>().returnPos.position; // Follower의 원래 위치 저장
            colorIndex++;
        }
    }
    void Update()
    {
        NearestNeighborFinder();
    }
    void DashAndReturn(GameObject follower, Vector3 targetPosition)
    {
        Vector3 originalPosition = originalPositions[follower];
        originalPosition = gameObject.GetComponent<Follower>().returnPos.position;
        if (!isDashing)
        {
            isDashing = true;
            mobGroupMoving.isSineActive = false;

            Sequence seq = DOTween.Sequence();
            seq.Append(follower.transform.DOMove(targetPosition, dashDuration))
                               .AppendInterval(0.5f)
               .Append(follower.transform.DOMove(originalPosition, dashDuration))
               .OnComplete(() =>
               {
                   mobGroupMoving.isSineActive = true; // Reset the sine wave movement state
                   isDashing = false;  // Reset the dashing state
               })
               .Play();
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }

    public void NearestNeighborFinder()
    {
        targetedObjects.Clear();  // 매 프레임마다 타겟팅 정보를 초기화합니다.

        // 파괴된 오브젝트를 targetObjects 리스트에서 제거합니다.
        targetObjects.RemoveAll(item => item == null);

        if (targetObjects.Count == 0)
        {
            Debug.LogWarning("targetObjects 리스트가 비어 있음");
            return;
        }

        foreach (var obj in selfObjects)
        {
            Vector2? nearest = FindNearestTargetInRange(obj.transform.position);
            
            if (nearest.HasValue)
            {
                Debug.Log(obj.name + "의 가장 가까운 타겟: " + nearest.Value);
                var nearestObj = targetObjects.Find(o => Vector3.Distance(o.transform.position, (Vector3)nearest.Value) < 0.5f);


                if (nearestObj == null)
                {
                    Debug.LogWarning($"{obj.name} -> nearestObj가 null임!");
                }
                else
                {
                    Debug.Log($"{obj.name} -> nearestObj: {nearestObj.name}");
                }


                if (nearestObj != null && !targetedObjects.Contains(nearestObj))
                {

                    targetedObjects.Add(nearestObj);  // 이 개체를 타겟팅된 개체로 추가

                    Debug.Log($"Follower {obj.name} -> Target {nearestObj.name} 선택됨");

                    // 선으로 표시 (개체마다 다른 색상 사용)
                    Debug.DrawLine(obj.transform.position, nearest.Value, Color.red);

                    // Dash towards the nearest target and return
                    DashAndReturn(obj, nearestObj.transform.position);
                }
                else
                {
                    Debug.LogWarning($"{obj.name} -> nearestObj가 null임! (Find() 실패 가능성)");
                }

            }
            else
            {
                Debug.Log(obj.name + "의 가장 가까운 타겟이 없음");
            }
        }
    }



    Vector2? FindNearestTargetInRange(Vector2 position)
    {
        Vector2? bestTarget = null;
        float bestDistance = float.MaxValue;

        if (targetObjects.Count == 0)
        {
            Debug.LogWarning("🚨 targetObjects 리스트가 비어 있음!");
            return null;
        }

        foreach (var target in targetObjects)
        {
            if (target == null)
            {
                Debug.LogWarning("⚠️ targetObjects 리스트에서 null 오브젝트 발견");
                continue;
            }

            // Enemy의 Fainted 상태를 확인
            EnemyScr enemyScript = target.GetComponent<EnemyScr>();
            if (enemyScript != null && enemyScript.enemyIsFainted)
            {
                Debug.Log($"⛔ {target.name}은 Fainted 상태이므로 제외됨");
                continue;
            }

            float distance = Vector2.Distance(position, target.transform.position);
            Debug.Log($"🔍 {target.name}과의 거리: {distance} (detectionRange: {detectionRange})");

            // detectionRange 내에 있는지 확인
            if (distance > detectionRange)
            {
                Debug.Log($"🚫 {target.name}이 detectionRange 밖임 ({distance} > {detectionRange})");
                continue;
            }

            // 이미 타겟팅된 개체인지 확인
            if (targetedObjects.Contains(target))
            {
                Debug.Log($"⚠️ {target.name}은 이미 타겟팅됨");
                continue;
            }

            // 가장 가까운 타겟을 찾기 위한 조건 확인
            if (distance < bestDistance)
            {
                Debug.Log($"✅ {target.name}이 현재 가장 가까운 적임 ({distance} < {bestDistance})");
                bestDistance = distance;
                bestTarget = target.transform.position;
            }
        }

        if (!bestTarget.HasValue)
        {
            Debug.LogWarning("❌ 적절한 타겟을 찾지 못함 (모든 적이 제외됨)");
        }

        return bestTarget;
    }

}