//using Assets;
//using DG.Tweening;
//using System.Collections;
//using System.Collections.Generic;
//using System.Runtime.InteropServices.WindowsRuntime;
//using System.Xml.Serialization;
//using Unity.Burst.CompilerServices;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
//using UnityEngine.SocialPlatforms;
//using UnityEngine.TextCore.Text;
//using static UnityEditor.PlayerSettings;
//using static UnityEngine.UI.Image;

public class MobGroupMoving : MonoBehaviour
{
    // Follow
    [SerializeField] float moveDistance;
    [SerializeField] float moveSpeed;
    Transform player;

    // Sine
    [SerializeField] float amplitude = 2f; // sine 파동의 높이
    [SerializeField] float frequency = 1.0f; // sine 파동의 주기
    //[SerializeField] float sineSpeed = 3.2f;
    Collider2D lastGroundCollider; // 마지막으로 닿은 땅의 Collider 정보를 저장할 변수
    public bool isSineActive = false; // Sine 애니메이션 활성화 여부
    float sineY;
    float startY;

    // Dash & Return
    //[SerializeField] float detectionRange;
    //[SerializeField] float dashSpeed;
    //private bool isDashing = false;
    //Vector2 dashDir;
    ////public List<Transform> returnPosList;
    //public Transform returnPos;

    //private Dictionary<GameObject, Vector3> originalPositions; // Follower들의 원래 위치 저장

    //public float attackRange = 5f;

    Animator anim;
    LayerMask groundLayer;

    // Enemy Targeting
    //private List<GameObject> selfObjects;  // 자신들에 해당하는 개체들 리스트
    //private List<GameObject> targetObjects;  // B 태그에 해당하는 개체들 리스트

    //private KDTree selfKDTree;
    //private KDTree targetKDTree;
    //private HashSet<GameObject> targetedObjects;  // 타겟팅된 개체들 기록

    //// Color Array for different colors
    //private Color[] colors;
    //private Dictionary<GameObject, Color> objectColorMap;

    void Start()
    {
        startY = transform.position.y;
        #region 창고
        //selfObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag("Follower"));  // 자신들의 태그
        //targetObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));  // Enemy 태그에 해당하는 개체들의 태그
        //selfKDTree = new KDTree();
        //targetKDTree = new KDTree();
        //targetedObjects = new HashSet<GameObject>();
        //originalPositions = new Dictionary<GameObject, Vector3>();

        //// 자신들의 위치로 K-D 트리 초기화
        //foreach (var obj in selfObjects)
        //{
        //    selfKDTree.Insert(obj.transform.position);
        //}

        //// Enemy 태그 개체들 위치로 K-D 트리 초기화
        //foreach (var obj in targetObjects)
        //{
        //    targetKDTree.Insert(obj.transform.position);
        //}


        //// Define colors (You can add more colors or generate them dynamically)
        //colors = new Color[] { Color.red, Color.green, Color.blue, Color.yellow, Color.magenta, Color.cyan, Color.white, Color.black };
        //objectColorMap = new Dictionary<GameObject, Color>();

        //int colorIndex = 0;
        //foreach (var obj in selfObjects)
        //{
        //    // Assign a color to each follower
        //    objectColorMap[obj] = colors[colorIndex % colors.Length];
        //    originalPositions[obj] = obj.transform.position; // Follower의 원래 위치 저장
        //    colorIndex++;
        //}
        #endregion
    }
    void Awake()
    {
        anim = GetComponent<Animator>();
        groundLayer = LayerMask.GetMask("groundLayer");

        player = GameObject.FindGameObjectWithTag("Player").transform;
        Physics2D.IgnoreLayerCollision(7, 9); // Mob(9)과 Player(7) 충돌 무시
    }
    void Update()
    {
        Sine();
        FollowPlayer();
        //NearestNeighborFinder();
    }
    private void FixedUpdate()
    {
        ResetStartY();
    }

    // detect Enemy target
    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.black;
    //    Gizmos.DrawWireSphere(transform.position, detectionRange);
    //}

    //public void NearestNeighborFinder()
    //{
    //    targetedObjects.Clear();  // 매 프레임마다 타겟팅 정보를 초기화합니다.

    //    foreach (var obj in selfObjects)
    //    {
    //        Vector2? nearest = FindNearestTargetInRange(obj.transform.position);

    //        if (nearest.HasValue)
    //        {
    //            var nearestObj = targetObjects.Find(o => o.transform.position == (Vector3)nearest.Value);

    //            if (nearestObj != null && !targetedObjects.Contains(nearestObj))
    //            {
    //                targetedObjects.Add(nearestObj);  // 이 개체를 타겟팅된 개체로 추가

    //                // Get the assigned color for this object
    //                Color lineColor = objectColorMap[obj];

    //                // 선으로 표시 (개체마다 다른 색상 사용)
    //                Debug.DrawLine(obj.transform.position, nearest.Value, lineColor);

    //                //// Dash towards the nearest target and return
    //                //DashAndReturn(obj, nearestObj.transform.position);
    //            }
    //        }
    //    }
    //}


    //void DashAndReturn(GameObject follower, Vector3 targetPosition)
    //{
    //    Vector3 originalPosition = originalPositions[follower];

    //    // Check if the follower is not already dashing
    //    if (!isDashing)
    //    {
    //        isDashing = true;

    //        Sequence seq = DOTween.Sequence();
    //        seq.Append(follower.transform.DOMove(targetPosition, dashSpeed))
    //           .AppendInterval(0.5f)
    //           .Append(follower.transform.DOMove(originalPosition, dashSpeed))
    //           .OnComplete(() => isDashing = false) // Reset the dashing state
    //           .Play();
    //    }
    //}


    //Vector2? FindNearestTargetInRange(Vector2 position)
    //{
    //    Vector2? bestTarget = null;
    //    float bestDistance = float.MaxValue;

    //    foreach (var target in targetObjects)
    //    {
    //        float distance = Vector2.Distance(position, target.transform.position);

    //        // Check if the target is within the detection range
    //        if (distance <= detectionRange && !targetedObjects.Contains(target))
    //        {
    //            if (distance < bestDistance)
    //            {
    //                bestDistance = distance;
    //                bestTarget = target.transform.position;
    //            }
    //        }
    //    }

    //    return bestTarget;
    //}

    // Follow To Player
    void FollowPlayer()
    {
        if (Mathf.Abs(transform.position.x - player.position.x) > moveDistance)
            transform.Translate(new Vector2(-1, 0) * Time.deltaTime * moveSpeed);
        DirectionFollower();
    }
    void DirectionFollower()
    {
        //  몹 그룹이 플레이어 왼쪽에 있을 때
        // 오브젝트 위치 좌우반전
        if (transform.position.x - player.position.x < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }

    // y core Idle Moving
    void Sine()
    {
        if (isSineActive)
        {
            sineY = startY + Mathf.Sin(Time.time * frequency) * amplitude;
            //rb.MovePosition(new Vector2(rb.position.x, sineY)); // Sine()에서 계산된 Y축 위치 사용
            transform.position = new Vector2(transform.position.x, sineY); // Sine()에서 계산된 Y축 위치 사용
        }
    }

    //void TeleportToPlayer()
    //{
    //    if (Vector2.Distance(player.position, transform.position) > telDistance)
    //    {
    //        sineY = startY + Mathf.Sin(Time.time * frequency) * amplitude;
    //        transform.position = new Vector2(transform.position.x, sineY);
    //    }
    //}
    void ResetStartY()
    {
        // 캐릭터의 아래에 있는 Collider의 절반 크기만큼의 레이를 쏘아서 땅과 충돌하는지 여부를 검사
        Vector2 raycastStart = new Vector2(player.transform.position.x, player.transform.position.y - 2f);
        RaycastHit2D hit = Physics2D.Raycast(raycastStart, Vector2.down, 0.2f, LayerMask.GetMask("groundLayer"));
        Debug.DrawRay(raycastStart, Vector2.down * 0.2f, Color.magenta); // 레이를 시각적으로 표시

        if (hit.collider != null) // && 닿은 오브젝트의 태그가 movingPlatform이 아닌 경우에만!
        {
            if (hit.collider != lastGroundCollider) // 현재 닿아있는 땅이 이전 땅과 다를 경우에만 실행
            {
                lastGroundCollider = hit.collider; // 현재 닿아있는 땅을 업데이트
                startY = hit.point.y + 5f;
            }
        }
    }
}