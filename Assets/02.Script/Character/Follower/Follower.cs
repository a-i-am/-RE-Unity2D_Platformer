using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    // watch & follow
    //public Vector3 followPos;
    //public int followDelay;
    //public Transform front;
    //public Queue<Vector3> frontPos;

    // follow to player
    //[SerializeField] float moveDistance;
    //[SerializeField] float moveSpeed;

    // Dash & Return
    [SerializeField] float detectionRange;
    [SerializeField] float dashDuration;
    private MobGroupMoving mobGroupMoving;
    private bool isDashing = false;
    public Transform returnPos; // 인스펙터에서 각 Follower의 returnPos 할당
    private Dictionary<GameObject, Vector3> originalPositions; // Follower들의 원래 위치 저장

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
    void Awake()
    {
        //frontPos = new Queue<Vector3>();
    }

    void Update()
    {
        NearestNeighborFinder(); 
    }
    void DashAndReturn(GameObject follower, Vector3 targetPosition)
    {
        Vector3 originalPosition = originalPositions[follower];
        originalPosition = gameObject.GetComponent<Follower>().returnPos.position;
        #region domove 창고
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
        #endregion
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

        foreach (var obj in selfObjects)
        {
            Vector2? nearest = FindNearestTargetInRange(obj.transform.position);

            if (nearest.HasValue)
            {
                var nearestObj = targetObjects.Find(o => o.transform.position == (Vector3)nearest.Value);

                if (nearestObj != null && !targetedObjects.Contains(nearestObj))
                {
                    targetedObjects.Add(nearestObj);  // 이 개체를 타겟팅된 개체로 추가

                    // Get the assigned color for this object
                    Color lineColor = objectColorMap[obj];

                    // 선으로 표시 (개체마다 다른 색상 사용)
                    Debug.DrawLine(obj.transform.position, nearest.Value, lineColor);

                    //// Dash towards the nearest target and return
                    DashAndReturn(obj, nearestObj.transform.position);
                }
            }
        }
    }


    Vector2? FindNearestTargetInRange(Vector2 position)
    {
        Vector2? bestTarget = null;
        float bestDistance = float.MaxValue;

        foreach (var target in targetObjects)
        {
            if (target == null) continue; // target이 null인지 확인

            float distance = Vector2.Distance(position, target.transform.position);

            // Check if the target is within the detection range
            if (distance <= detectionRange && !targetedObjects.Contains(target))
            {
                if (distance < bestDistance)
                {
                    bestDistance = distance;
                    bestTarget = target.transform.position;
                }
            }
        }

        return bestTarget;
    }


    #region 창고 
    // Follow To Player
    //void FollowPlayer()
    //{
    //    if (Mathf.Abs(transform.position.x - front.position.x) > moveDistance)
    //        transform.Translate(new Vector2(-1, 0) * Time.deltaTime * moveSpeed);
    //    //DirectionFollower();
    //}
    //void DirectionFollower()
    //{
    //    //  몹 그룹이 플레이어 왼쪽에 있을 때
    //    // 오브젝트 위치 좌우반전
    //    if (transform.position.x - front.position.x < 0)
    //    {
    //        transform.eulerAngles = new Vector3(0, 180, 0);
    //    }
    //    else
    //    {
    //        transform.eulerAngles = new Vector3(0, 0, 0);
    //    }
    //}


    //void Watch()
    //{
    //    //위치 입력 // FIFO
    //    //if (!parentPos.Contains(front.position))   //같은 위치값이면 큐에 저장하지 않음
    //    frontPos.Enqueue(front.position);

    //    //위치 출력
    //    if (frontPos.Count > followDelay)
    //        followPos = frontPos.Dequeue();
    //    else if (frontPos.Count < followDelay)
    //        followPos = front.position;
    //}

    //void Follow()
    //{
    //    transform.position = followPos;
    //}
    #endregion
}