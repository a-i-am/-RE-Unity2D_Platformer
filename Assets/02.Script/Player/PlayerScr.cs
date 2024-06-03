using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Assets;
using System;

namespace Assets
{
    public class PlayerScr : MonoBehaviour
    {
        public ObjectPoolManager projectilePool;
        private Rigidbody2D rb;
        private Vector2 currentVelocity;
        private PlayerAnimScr playerAnimScr;

        private LayerMask groundLayer; // Ground 레이어를 가진 오브젝트와의 충돌을 감지
        private SpriteRenderer spriteRenderer;
        private float inputHorizontal;

        [SerializeField] private Projectile projectilePrefab;
        [SerializeField] private Transform launchOffsetL;
        [SerializeField] private Transform launchOffsetR;
        [SerializeField] private float moveSpeed = 13f;
        [SerializeField] private float jumpForce = 45f;
        [SerializeField] private float coyoteTime = 0.1f; // 코요태 점프 타임
        [SerializeField] private float coyoteTimer = 0f; // 코요태 점프 타이머
        [SerializeField] private float attackDistance;

        private bool deadWait; // 사망 시 다음 동작 지연
        private bool respawnOrDead; // 플레이어 사망 유형(리스폰 or 게임오버) 판정
        internal bool isGrounded; // 지면 판정


        //코요태타임점프(코루틴을 사용한 지연 점프)
        private bool isCoroutineActive = false;


        void Start()
        {
            // GameManager의 gameOver 델리게이트에 연결
            // 플레이어의 OnDeath 메서드를 델리게이트에 등록
            GameManager.Instance.gameOverDele += OnDeath;
            rb = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            playerAnimScr = GetComponent<PlayerAnimScr>();
        }

        void Update()
        {
            inputHorizontal = Input.GetAxisRaw("Horizontal");
            Jump();
            Launch();
        }

        void FixedUpdate()
        {
            Walk();
            CheckGrounded(); // 캐릭터의 땅과의 충돌 여부를 검사하는 메소드 호출
            KeepPlayerOnGround();
            UpdateCoyoteTimer();
        }
        public SpriteRenderer SpriteRenderer
        {
            get { return spriteRenderer; }
        }

        void Walk()
        {
            currentVelocity = new Vector2(inputHorizontal * moveSpeed, rb.velocity.y);
            // 플레이어 스프라이트는 기본 오른쪽 방향
            // 뒤집어야될 순간은 왼쪽 방향으로 움직일 때 
            if (inputHorizontal < 0 && !respawnOrDead)
            {
                rb.velocity = currentVelocity;
                spriteRenderer.flipX = true;
                //projectileFlipX = true;
            }
            else if (inputHorizontal > 0 && !respawnOrDead)
            {
                rb.velocity = currentVelocity;
                spriteRenderer.flipX = false;
                //projectileFlipX = false;
            }
            else
            {
                rb.velocity = new Vector2(0f, rb.velocity.y);
                //projectileFlipX = false;
            }
        }
        void Jump()
        {
            //if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            if (Input.GetButton("Jump") && isGrounded)
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        void Launch()
        {
            // 플레이어는 자신이 공격 당한 상태 외엔 공격 가능
            // hurt() 판정으로 확인하기 
            if (Input.GetKeyDown(KeyCode.Z))
            {
                //Debug.Log("Projectile Launch");
                if (spriteRenderer.flipX)
                {
                    Instantiate(projectilePrefab, launchOffsetL.position, transform.rotation);
                }
                else
                    Instantiate(projectilePrefab, launchOffsetR.position, transform.rotation);


            }
        }

        // 코요태타임점프 코루틴
        IEnumerator CoyoteTimeJump()
        {
            isCoroutineActive = true;
            yield return new WaitForSeconds(coyoteTime);
            Jump();
            isCoroutineActive = false;

            if (!isCoroutineActive && coyoteTimer > 0f)
            {
                StartCoroutine(CoyoteTimeJump());
            }
        }
        internal void CheckGrounded()
        {
            // 캐릭터의 아래에 있는 Collider의 절반 크기만큼의 레이를 쏘아서 땅과 충돌하는지 여부를 검사
            Vector2 raycastStart = new Vector2(transform.position.x, GetComponent<Collider2D>().bounds.center.y - 1f);
            RaycastHit2D hit = Physics2D.Raycast(raycastStart, Vector2.down, 0.2f, LayerMask.GetMask("groundLayer"));

            if (hit.collider != null)
            {
                // 충돌이 발생한 경우
                //Debug.Log("Hit Collider: " + hit.collider.name);
                //Debug.Log("Hit Distance: " + hit.distance);
                isGrounded = true;
            }
            else
            {
                // 충돌이 발생하지 않은 경우
                //Debug.Log("No Ground Detected");
                isGrounded = false;
            }

            Debug.DrawRay(raycastStart, Vector2.down * 3.0f, Color.green); // 레이를 시각적으로 표시

            //Debug.Log("isGrounded: " + isGrounded);
        }
        void KeepPlayerOnGround()
        {
            if (isGrounded)
            {
                // 땅과 충돌하고 있을 때만 높이 조절
                rb.position = new Vector2(rb.position.x, rb.position.y - 0.1f);
            }
        }
        void UpdateCoyoteTimer()
        {
            if (isGrounded)
            {
                coyoteTimer = coyoteTime;
            }
            else if (coyoteTimer > 0f)
            {
                coyoteTimer -= Time.deltaTime;
            }
        }

        // 플레이어의 죽음
        //GameManager.Instance.gameOverDele += OnDeath;
        public void OnDeath() // OnDeath로 플레이어 죽음 하나로 묶음 => gameOverDele & OnDeath() 불러오기  
        {
            // 1)데스존에 빠짐 2)체력 쓰러짐
            StartCoroutine(DeadJump());
            // 게임오버 화면 전환
            //Invoke("OnDeath", 1.5f); // 해당 메소드 지연 후 실행
        }
        // 데스존 게임 오버(마리오 사망 모션같은 것)
        IEnumerator DeadJump()
        {
            respawnOrDead = true;
            //GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

            Debug.Log("DeadJumpStart");
            playerAnimScr.DeadJumpAnimation(true);
            yield return new WaitForSeconds(1f);
            deadWait = true;
            Debug.Log("DeadJumpEnd");

            if (deadWait)
            {
                rb.AddForce(new Vector2(0, 1500f));
                rb.gravityScale = 8;
                gameObject.GetComponent<Collider2D>().enabled = false;
                // 만약 목숨이 0개라면(GameOverDeath)
            }
        }

    }
}