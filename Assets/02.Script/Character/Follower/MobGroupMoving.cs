using Assets;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Serialization;
using Unity.Burst.CompilerServices;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.TextCore.Text;
using static UnityEditor.PlayerSettings;
using static UnityEngine.UI.Image;

public class MobGroupMoving : MonoBehaviour
{

    float amplitude = 1.0f; // sine 파동의 높이
    float frequency = 1.0f; // sine 파동의 주기
    float startY;
    float inputHorizontal;
    [SerializeField] float distance = 5f;
    [SerializeField] float moveSpeed = 15f;
    [SerializeField] float telDistance = 30f;
    [SerializeField] float teleportDelay = 3f; // 텔레포트 지연 시간 변수 추가
    [SerializeField] float sineSpeed = 3.2f;
    [SerializeField] Transform veryFront;
    [SerializeField] Transform veryBack;
    //public Vector2 followPos;
    //public int followDelay = 12;
    //public Transform frontMob;
    //public Queue<Vector2> frontPos;

    // 자식 개체의 Transform을 저장할 변수
    //Transform childTransform;
    float startingPos;
    float endPos;
    float direction;
    float sineY;
    //bool canSine;
    bool canTeleport;
    bool isDetectWall;
    Rigidbody2D rb;
    Transform player;

    
    Animator anim;
    LayerMask groundLayer;


    void Start()
    {
        startY = rb.position.y;
    }
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        groundLayer = LayerMask.GetMask("groundLayer");
        player = GameObject.FindGameObjectWithTag("Player").transform;

        //frontPos = new Queue<Vector2>();
        endPos = transform.position.y + 0.8f;
        startingPos = transform.position.y + 0.3f;
        //canTeleport = false; // 초기값을 false로 설정
        StartCoroutine(EnableTeleportAfterDelay()); // 지연 후 텔레포트 활성화 코루틴 시작
    }

    void Update()
    {
        inputHorizontal = Input.GetAxis("Horizontal");

        //Watch();
        //Follow();
        // y축으로 Sin 운동의 범위를 설정하고 Sin 함수의 결과를 계산

        //direction = transform.position.x < player.position.x ? 1 : -1;

        //if (Mathf.Abs(transform.position.x - player.position.x) > distance)
        //{
        //    MobMove(direction);
        //}

        //DetectWall();
        Sine();
        MobMove(inputHorizontal);
    }

    private void FixedUpdate()
    {
        ResetStartY();

        //if (canTeleport)
        //    TeleportToPlayer();

    }

    //void Watch()
    //{
    //    // #.Input Pos
    //    if (!frontPos.Contains(frontMob.position))
    //        frontPos.Enqueue(frontMob.position);
    //    // #.Output Pos
    //    if (frontPos.Count > followDelay)
    //    {
    //        followPos = frontPos.Dequeue();
    //    }
    //    else if(frontPos.Count < followDelay)
    //    {
    //        followPos = frontMob.position;
    //    }
    //}

    //void Follow()
    //{
    //    if (transform.childCount > 0)
    //    {
    //        //Vector3 childLocalPosition = childTransform.localPosition;
    //        //transform.position = followPos;
    //        childTransform = transform.GetChild(0);
    //        childTransform.position = followPos;
    //    }
    //}
    void MobMove(float direction)
    {
        //// 성공, 속도 높으면 떨림은 발생
        //float newX = rb.position.x + direction * Time.deltaTime * moveSpeed;
        //Vector2 targetPosition = new Vector2(newX, sineY);
        //rb.MovePosition(targetPosition);

        //transform.Translate(new Vector2(direction, sineY) * Time.deltaTime * moveSpeed);
        // X축으로만 이동
        //if(Mathf.Abs(transform.position.x - player.position.x) > distance)
        if (inputHorizontal > 0 && transform.position.x + 5f < player.position.x
            || inputHorizontal < 0 && transform.position.x - 30f > player.position.x - direction)
            transform.Translate(new Vector2(direction * Time.deltaTime * moveSpeed, 0));

        rb.MovePosition(new Vector2(rb.position.x, sineY)); // Sine()에서 계산된 Y축 위치 사용

        // 앞에 벽 있으면 movespeed = 0;

        // Y축 이동을 반영하여 Translate
        //transform.Translate(new Vector2(direction, 0) * Time.deltaTime * moveSpeed);

        // 움직임 끊김
        //rb.MovePosition(new Vector2(direction, sineY)); // Sine()에서 계산된 Y축 위치 사용

    }

    void Sine()
    {
        sineY = startY + Mathf.Sin(Time.time * frequency) * amplitude;
        //float newY = startY + Mathf.Sin(Time.time * frequency) * amplitude;
        //rb.MovePosition(new Vector2(rb.position.x, newY));
        //sineY = Mathf.Lerp(startingPos, endPos, (Mathf.Sin(Time.time * sineSpeed) + 1) / 2);
        ////Vector2 targetPosition = new Vector2(rb.position.x, sineY); // 기존 위치를 유지하면서 y값만 업데이트
        //Vector2 targetPosition = new Vector2(transform.position.x, sineY); // 기존 X 위치를 유지하면서 Y 값만 업데이트
        //rb.MovePosition(targetPosition); // Rigidbody를 사용하여 위치 업데이트
    }

    void ResetStartY()
    {
        // 캐릭터의 아래에 있는 Collider의 절반 크기만큼의 레이를 쏘아서 땅과 충돌하는지 여부를 검사
        //Vector2 raycastStart = new Vector2(player.transform.position.x, GetComponent<Collider2D>().bounds.center.y - 1f);
        Vector2 raycastStart = new Vector2(player.transform.position.x, player.transform.position.y - 2f);
        RaycastHit2D hit = Physics2D.Raycast(raycastStart, Vector2.down, 0.2f, LayerMask.GetMask("groundLayer"));
        Debug.DrawRay(raycastStart, Vector2.down * 0.2f, Color.magenta); // 레이를 시각적으로 표시
        if (hit.collider != null) // && 닿은 오브젝트의 태그가 movingPlatform이 아닌 경우에만!
        {
            startY = hit.point.y + 5f;
        }
    }

    void DetectWall()
    {
        Vector2 raycastStart = new Vector2(transform.position.x, transform.position.y);
        RaycastHit2D hitLeft = Physics2D.Raycast(raycastStart, Vector2.left, 30f, LayerMask.GetMask("groundLayer"));
        RaycastHit2D hitRight = Physics2D.Raycast(raycastStart, Vector2.right, 30f, LayerMask.GetMask("groundLayer"));
        Debug.DrawRay(raycastStart, Vector2.left * 30f, Color.magenta); // 레이를 시각적으로 표시
        Debug.DrawRay(raycastStart, Vector2.right * 30f, Color.magenta); // 레이를 시각적으로 표시

        if (hitLeft.collider != null || hitRight.collider != null)
        {
            moveSpeed = 0;
            isDetectWall = true;
        }
        else
        {
            moveSpeed = 5f;
            isDetectWall = false;
        }

    }

    void TeleportToPlayer()
    {
        if (Vector2.Distance(player.position, transform.position) > telDistance && !isDetectWall)
            transform.position = player.position;
    }
    IEnumerator EnableTeleportAfterDelay()
    {
        yield return new WaitForSeconds(teleportDelay);
        canTeleport = true;
    }

    //IEnumerator DisableSineForSeconds(float seconds)
    //{
    //    canSine = false; // Sine 함수 비활성화
    //    yield return new WaitForSeconds(seconds); // 지정한 시간 동안 대기
    //    canSine = true; // Sine 함수 다시 활성화
    //}




}
