using Assets;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using static UnityEngine.UI.Image;

public class MobMoving : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float distance = 5f;
    [SerializeField] private float jumpPower = 10f;
    private SpriteRenderer spriteRenderer;
    Transform player;
    Animator anim;
    Rigidbody2D rb;
    LayerMask groundLayer;

    void Start()
    {
    }
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        groundLayer = LayerMask.GetMask("groundLayer");
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        Physics2D.IgnoreLayerCollision(7, 9); // Player와 Mob 충돌 무시
        //Physics2D.IgnoreLayerCollision(6, 3); // Enemy 죽으면 Player와 Enemy 충돌 무시, 닿으면 플레이어 HP 감소
    }
    void Update()
    {
        // 스프라이트 좌우 전환
        // M < P : TRUE 시 몹이 오른쪽 보게 함  
        spriteRenderer.flipX = (transform.position.x < player.position.x);
        // 몹 이동 방향(몹이 플레이어 따라가야함(작은 값의 방향(-)이 큰 값의 방향(+) 따라간다고 생각하기)
        float direction = transform.position.x < player.position.x ? 1 : -1;
        
        // 나중에 몹 이동 조건을 프리펩 배열 생성 여부로 바꿀 것임.
        if (Mathf.Abs(transform.position.x - player.position.x) > distance)
        {
            transform.Translate(new Vector2(direction, 0) * Time.deltaTime * speed);
            anim.SetBool("IsWalk", true);

            // DrawRay 추가
            Debug.DrawRay(transform.position, new Vector2(direction, 0) * 5f, Color.magenta);
            Debug.DrawRay(transform.position, new Vector2(1 * direction, 1) * 5f, Color.blue);

            RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(direction, 0), 5f, groundLayer);
            RaycastHit2D hitdia = Physics2D.Raycast(transform.position, new Vector2(1 * direction, 1), 5f, groundLayer);

            // 플레이어가 몹보다 아래에 있을 때 레이캐스트 반환 값 비워줌
            // 몹 혼자 발판 위로 올라가는 경우를 막기 위함
            if (player.position.y - transform.position.y <= 0)
                hitdia = new RaycastHit2D();

            // 몹의 레이캐스트에 맞으면 점프
            if (hit || hitdia)
            {
                Debug.Log("몹 점프한다");
                rb.velocity = Vector2.up * jumpPower;
            }
        }   
        else 
        {
            anim.SetBool("IsWalk", false);
        }
    }
}
