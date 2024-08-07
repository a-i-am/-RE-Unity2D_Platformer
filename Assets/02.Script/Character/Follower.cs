using Assets;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static UnityEditor.PlayerSettings;
using static UnityEngine.UI.Image;

public class Follower : MonoBehaviour
{
    //[SerializeField] float distance = 5f;
    //[SerializeField] float moveSpeed = 2f;
    [SerializeField] float telDistance = 20f;
    [SerializeField] float teleportDelay = 3f; // 텔레포트 지연 시간 변수 추가
    [SerializeField] float sineSpeed = 3.2f;
    public Transform parent;

    float startingPos;
    float endPos;
    float direction;
    //bool canSine;
    //bool canTeleport;
    Rigidbody2D rb;
    SpriteRenderer spriteRenderer_player;
    Transform playerTransform;
    Animator anim;
    LayerMask groundLayer;

    void Start()
    {

}
void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer_player = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        
        groundLayer = LayerMask.GetMask("groundLayer");
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        Physics2D.IgnoreLayerCollision(7, 9); // Player와 Mob 충돌 무시

        endPos = transform.position.y + 0.8f;
        startingPos = transform.position.y + 0.3f;
        //canTeleport = false; // 초기값을 false로 설정
        StartCoroutine(EnableTeleportAfterDelay()); // 지연 후 텔레포트 활성화 코루틴 시작
    }

    void Update()
    {
        // 스프라이트 좌우 전환
        spriteRenderer_player.flipX = (transform.position.x < playerTransform.position.x);
        direction = transform.position.x < playerTransform.position.x ? 1 : -1;

        // 몹은 플레이어와 일정 거리 떨어져 있어야 플레이어의 움직임을 따라갈 수 있음
        //if (Mathf.Abs(transform.position.x - player.position.x) > distance)
        //{
        //    MobMove(direction);
        //}
    }

    private void FixedUpdate()
    {
        Sine();

        //if (canTeleport)
        //{
        //    TeleportToPlayer();
        //}

    }
    //void MobMove(float direction)
    //{
    //    //transform.position = followPos;
    //    //transform.Translate(new Vector2(direction, 0) * Time.deltaTime * moveSpeed);
    //}

    void Sine()
    {
        // y축으로 Sin 운동의 범위를 설정하고 Sin 함수의 결과를 계산
        float y = Mathf.Lerp(startingPos, endPos, (Mathf.Sin(Time.time * sineSpeed) + 1) / 2);
        Vector2 targetPosition = new Vector2(rb.position.x, y); // 기존 위치를 유지하면서 y값만 업데이트
        rb.MovePosition(targetPosition); // Rigidbody를 사용하여 위치 업데이트
    }

    void TeleportToPlayer()
    {
        if (Vector2.Distance(playerTransform.position, transform.position) > telDistance)
            transform.position = playerTransform.position;
    }
    IEnumerator EnableTeleportAfterDelay()
    {
        yield return new WaitForSeconds(teleportDelay);
        //canTeleport = true;
    }

    //IEnumerator DisableSineForSeconds(float seconds)
    //{
    //    canSine = false; // Sine 함수 비활성화
    //    yield return new WaitForSeconds(seconds); // 지정한 시간 동안 대기
    //    canSine = true; // Sine 함수 다시 활성화
    //}




}
