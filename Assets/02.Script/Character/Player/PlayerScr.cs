using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Assets;
using System;
using UnityEngine.WSA;
using System.Xml.Serialization;

namespace Assets
{
    public class PlayerScr : MonoBehaviour
    {
        public ParticleSystem CastingSpellEffect;
        public ObjectPoolManager projectilePool;
        private Rigidbody2D rb;
        private Vector2 currentVelocity;
        private PlayerAnimScr playerAnimScr;
        private LayerMask groundLayer; // Ground 레이어를 가진 오브젝트와의 충돌을 감지
        private SpriteRenderer spriteRenderer;
        private float inputHorizontal;
        private float keyHoldTime = 0f;
        private bool isDash;

        //[SerializeField] private Projectile projectilePrefab;
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private Transform launchOffsetL;
        [SerializeField] private Transform launchOffsetR;

         private float dashTime;
         private float defaultSpeed;
        [SerializeField] private float defaultTime;
        [SerializeField] private float walkSpeed = 13f;
        [SerializeField] private float dashSpeed;
        [SerializeField] private float jumpForce = 45f;

        [SerializeField] private float coyoteTime = 0.1f; // 코요태 점프 타임
        [SerializeField] private float coyoteTimer = 0f; // 코요태 점프 타이머
        [SerializeField] private float attackDistance;


        private bool deadWait; // 사망 시 다음 동작 지연
        private bool respawnOrDead; // 플레이어 사망 유형(리스폰 or 게임오버) 판정
        private bool canLaunch = true; // Launch 메서드 호출 가능 여부
        internal bool isCastingSpell;
        internal bool isGrounded; // 지면 판정
        internal bool isAttacking;
        
        bool canDash;
        public Ghost ghost;
        public float dashCooldown; // 대시 후 대시가 다시 가능해지는 시간 (초 단위)
        private float lastDashTime = 0.0f; // 마지막 대시 입력 시간을 기록하는 변수

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
            CastingSpellEffect = transform.GetChild(0).GetComponent<ParticleSystem>();
        }

        void Update()
        {
            inputHorizontal = Input.GetAxisRaw("Horizontal");
            Jump();
            Launch();
            ResetLaunch();
            CheckGrounded(); // 캐릭터의 땅과의 충돌 여부를 검사하는 메소드 호출

            if (Input.GetKeyDown(KeyCode.C) && Time.time >= lastDashTime + dashCooldown)
            {
                canDash = true;
                lastDashTime = Time.time; // 현재 시간을 기록
            }
            else 
            { 
                //canDash = false;
                ghost.makeGhost = false;
            }
                    
        }

        void FixedUpdate()
        {
            Walk();
            Dash();
            //KeepPlayerOnGround();
            UpdateCoyoteTimer();
            CastingSpell();
        }
        public SpriteRenderer SpriteRenderer
        {
            get { return spriteRenderer; }
        }

        void Walk()
        {
            currentVelocity = new Vector2(inputHorizontal * walkSpeed, rb.velocity.y);
            // 플레이어 스프라이트는 기본 오른쪽 방향
            // 뒤집어야될 순간은 왼쪽 방향으로 움직일 때 
            if (!isCastingSpell && !isAttacking && inputHorizontal < 0 && !respawnOrDead)
            {
                rb.velocity = currentVelocity;
                spriteRenderer.flipX = true;
                //projectileFlipX = true;
            }
            else if (!isCastingSpell && !isAttacking && inputHorizontal > 0 && !respawnOrDead)
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
            if (Input.GetButton("Jump") && isGrounded && !isCastingSpell)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
        }

        void Dash()
        {
            if (canDash)
            {
                isDash = true;
            }
            if (dashTime <= 0)
            {
                rb.velocity = currentVelocity;
                if (isDash)
                    dashTime = defaultTime;
            }
            else
            {
                dashTime -= Time.deltaTime;

                if (spriteRenderer.flipX)
                {
                    ghost.makeGhost = true;
                    rb.velocity = new Vector2(dashSpeed * -1, rb.velocity.y);
                }
                else {
                    ghost.makeGhost = true;
                    rb.velocity = new Vector2(dashSpeed * 1, rb.velocity.y);
                }
            }
            isDash = false;
            canDash = false;
        }
        void Launch()
        {
            // 플레이어는 자신이 공격 당한 상태 외엔 공격 가능
            // hurt() 판정으로 확인하기 
                if (Input.GetKeyDown(KeyCode.Z) && !Input.GetKey(KeyCode.X) && !isAttacking && canLaunch)
                {
                    if (!isGrounded)
                    {
                        playerAnimScr.AerialLaunchAnimation();
                        Invoke("InstantiateProjectile", 0.4f); // 0.4초 후에 발사체 생성
                    }
                    else
                    {
                        playerAnimScr.LaunchAnimation();
                        Invoke("InstantiateProjectile", 0.2f); // 0.2초 후에 발사체 생성
                    }

                    canLaunch = false; // Launch 메서드 일시적으로 호출 불가능하게 설정
                    isAttacking = true;

                    // 3초 후에 LaunchExit 메서드 호출
                    Invoke("LaunchExit", 0.7f);
                    // 1초 후에 Launch 메서드 다시 호출 가능하게 설정
                    Invoke("ResetLaunch", 1.0f);
                }
        }

        void InstantiateProjectile()
        {
            GameObject projectile;
            if (spriteRenderer.flipX)
                projectile = Instantiate(projectilePrefab, launchOffsetL.position, transform.rotation);
            else
                projectile = Instantiate(projectilePrefab, launchOffsetR.position, transform.rotation);

            // 3초 후에 발사체 삭제
            Destroy(projectile, 3.0f);
        }

        void LaunchExit()
        {
            isAttacking = false;
        }

        void ResetLaunch()
        {
            canLaunch = true; // Launch 메서드 호출 가능하게 설정
        }

        void CastingSpell()
        {
            if (Input.GetKey(KeyCode.X))
            {
                if (!CastingSpellEffect.isPlaying && isGrounded)
                {
                    playerAnimScr.CastingSpellAnimation(true);
                    CastingSpellEffect.Play();
                    isCastingSpell = true;
                }
            }
            else if (CastingSpellEffect.isPlaying)
            {
                playerAnimScr.CastingSpellAnimation(false);
                CastingSpellEffect.Stop();
                isCastingSpell = false;
            }
            //if (Input.GetKey(KeyCode.X) && !CastingSpellEffect.isPlaying)
            //{
            //    playerAnimScr.CastingSpellAnimation(true);
            //    CastingSpellEffect.Play();
            //}
            //else
            //{
            //    playerAnimScr.CastingSpellAnimation(false);
            //    CastingSpellEffect.Stop();
            //}
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

            Debug.DrawRay(raycastStart, Vector2.down * 0.2f, Color.green); // 레이를 시각적으로 표시

            //Debug.Log("isGrounded: " + isGrounded);
        }
        //void KeepPlayerOnGround()
        //{
        //    if (isGrounded)
        //    {
        //        // 땅과 충돌하고 있을 때만 높이 조절
        //        rb.position = new Vector2(rb.position.x, rb.position.y - 0.1f);
        //    }
        //}
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