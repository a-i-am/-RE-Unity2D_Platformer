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
    public bool isSineActive = true; // Sine 애니메이션 활성화 여부
    float amplitude = 0.1f; // sine 파동의 높이
    float frequency = 1.0f; // sine 파동의 주기
    float startY;
    float inputHorizontal;
    [SerializeField] float distance = 5f;
    [SerializeField] float moveSpeed = 15f;
    [SerializeField] float telDistance;
    [SerializeField] float teleportDelay = 3f; // 텔레포트 지연 시간 변수 추가
    [SerializeField] float sineSpeed = 3.2f;
    [SerializeField] Transform veryFront;
    [SerializeField] Transform veryBack;
    float sineY;
    bool canTeleport;
    Transform player;
    Animator anim;
    LayerMask groundLayer;

    void Start()
    {
        //startY = rb.position.y;
        startY = transform.position.y;

    }
    void Awake()
    {
        //rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        groundLayer = LayerMask.GetMask("groundLayer");
        player = GameObject.FindGameObjectWithTag("Player").transform;

        StartCoroutine(EnableTeleportAfterDelay()); // 지연 후 텔레포트 활성화 코루틴 시작
    }

    void Update()
    {
        inputHorizontal = Input.GetAxis("Horizontal");
        Sine();
        MoveHorizontally();

    }

    private void FixedUpdate()
    {
        if (canTeleport)
            TeleportToPlayer();
    }

    public void SetSineActive(bool active)
    {
        isSineActive = active;
    }

    void MoveHorizontally()
    {
        // 현재 개체와 플레이어 간의 거리 계산
        float distanceToPlayer = Mathf.Abs(transform.position.x - player.position.x);

        // 좌우 입력에 따라 이동
        float moveX = inputHorizontal * moveSpeed * Time.deltaTime;

        // 이동 후 개체와 플레이어 간의 거리 계산
        float newDistanceToPlayer = Mathf.Abs((transform.position.x + moveX) - player.position.x);

        // 이동 후 거리가 허용 범위를 초과하면 이동을 제한
        if (newDistanceToPlayer <= distance)
        {
            transform.position = new Vector2(transform.position.x + moveX, transform.position.y);
        }
    }

    void Sine()
    {
        if(isSineActive)
        {
            sineY = startY + Mathf.Sin(Time.time * frequency) * amplitude;
            transform.position = new Vector2(transform.position.x, sineY); // Sine()에서 계산된 Y축 위치 사용
        }
    }

    void TeleportToPlayer()
    {
        if (Vector2.Distance(player.position, transform.position) > telDistance)
        {
            transform.position = player.position;
        }
    }

    IEnumerator EnableTeleportAfterDelay()
    {
        yield return new WaitForSeconds(teleportDelay);
        canTeleport = true;
    }
}
