using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScr : MonoBehaviour
{
    [SerializeField] private float followInterval = 0.1f; // 따라가는 간격
    [SerializeField] private float chaseDistance = 15f;
    [SerializeField] private float moveSpeed; // 몬스터의 이동 속도
    [SerializeField] private float chaseSpeed; // 몬스터가 플레이어를 빨리 쫓아갈 때 속도
    [SerializeField] private Transform player; // 플레이어의 Transform을 저장하는 변수
    [SerializeField] private LayerMask groundLayer; // 땅을 나타내는 레이어

    private bool enemyIsGrounded;
    private EnemyAnimScr enemyAnimScr;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        //rb = GetComponent<Rigidbody2D>();
        enemyAnimScr = GetComponent<EnemyAnimScr>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        // 충돌한 GameObject의 Rigidbody2D 컴포넌트를 가져옴

        //player = GameObject.FindWithTag("Player").GetComponent<Transform>();

        // StartCoroutine을 사용하여 코루틴 시작
        StartCoroutine(FollowPlayer());
    }

    // Update is called once per frame
    void Update()
    { }

    void FixedUpdate()
    {
        EnemyRay();
        OffCollider();
    }

    void OffCollider()
    {
        if (enemyIsGrounded == false) {
            gameObject.GetComponent<Collider2D>().enabled = false;
        }
    }

    void EnemyRay()
    {
            // 캐릭터의 아래에 있는 Collider의 절반 크기만큼의 레이를 쏘아서 땅과 충돌하는지 여부를 검사
            Vector2 raycastStart = new Vector2(transform.position.x, GetComponent<Collider2D>().bounds.center.y - 1f);
            RaycastHit2D hit = Physics2D.Raycast(raycastStart, Vector2.down, 3f, LayerMask.GetMask("groundLayer"));
            Debug.DrawRay(raycastStart, Vector2.down * 3f, Color.red); // 레이를 시각적으로 표시
            //RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 50f, groundLayer);

            if (hit.collider != null)
            {
                // 충돌이 발생한 경우
                //Debug.Log("**Enemy** Hit Collider: " + hit.collider.name);
                //Debug.Log("**Enemy** Hit Distance: " + hit.distance);
                enemyIsGrounded = true;
            }
            else
            {   // 충돌이 발생하지 않은 경우
                enemyIsGrounded = false;
                //Debug.Log("**Enemy** No Ground Detected");
            }
    }
    // 코루틴
    // 0.1초 마다 플레이어 방향으로 X축 이동
    IEnumerator FollowPlayer()
    {
        while (true)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            Vector2 targetPosition = new Vector2(player.position.x, transform.position.y);

            // 플레이어가 적의 오른쪽에 있는지 여부를 확인 후, 스프라이트 뒤집기
            spriteRenderer.flipX = (player.position.x > transform.position.x);

            Vector2 movement = (targetPosition - (Vector2)transform.position).normalized * moveSpeed * Time.deltaTime;
            transform.Translate(movement);
            enemyAnimScr.WalkAnimation(true); // 달리기 애니메이션 실행(bool)

            yield return new WaitForSeconds(followInterval);

            // 일정 거리 이하로 가까워지면 플레이어에게 달리기
            if (distanceToPlayer < chaseDistance)
            {
                Vector2 chaseMovement = (targetPosition - (Vector2)transform.position).normalized * chaseSpeed * Time.deltaTime;
                transform.Translate(chaseMovement);
                enemyAnimScr.WalkAnimation(true); // 달리기 애니메이션 실행(bool)
            }
            else
            {
                enemyAnimScr.WalkAnimation(false); // 달리기 애니메이션 종료
            }

            spriteRenderer.flipX = true;
        }
    }
}