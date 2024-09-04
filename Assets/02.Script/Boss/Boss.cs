using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public ParticleSystem gushOutEffect;
    public float speed;
    public float stopDistance;
    public float gushOutTimer = 0f;
    public float chompTimer = 0f;
    public float spinTimer = 0f;
    public float turnTimer = 0f;
    //private float spinCoolDown = 1.5f;
    public float spinSpeed;

    public Transform player;
    public bool isFlipped = false;

    [SerializeField] private int health = 100; // 몬스터의 체력
    private bool bossIsHurted = false;
    private bool bossIsFainted;

    private bool isSpinning = false;
    private bool isSpinDirectionSet = false; // 스핀 방향 설정여부 확인
    //Vector2 followDirection;
    float followDirection;
    float spinDirection;


    SpriteRenderer spriteRenderer;
    Animator anim;
    Rigidbody2D rbBoss;
    RaycastHit2D rayHit;
    bool isTurn = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rbBoss = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        gushOutEffect = transform.GetChild(0).GetComponent<ParticleSystem>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isSpinning)
        {
            gushOutTimer += Time.deltaTime;
            chompTimer += Time.deltaTime;
        }
        else if (gushOutTimer >= 10f && chompTimer >= 10f )
        {
            spinTimer += Time.deltaTime;
        }

        //StopSpin();
    }

    private void FixedUpdate()
    {
        GroundCheckRay();
        LookAtPlayer();
        //followDirection = (player.position - transform.position).normalized;\
        followDirection = player.position.x < transform.position.x ? -1f : 1f;

        if (!isSpinning && Vector3.Distance(player.position, rbBoss.position) >= stopDistance)
        {
            speed = 8f;
            anim.SetBool("GushOut", false);
            Chomp();
            if (!isSpinning)
                Follow();
        }
        else if (!isSpinning && Vector3.Distance(player.position, rbBoss.position) <= stopDistance)
        {
            anim.SetBool("Chomp", false);
            GushOut();
        }

        if (gushOutTimer >= 15f && chompTimer >= 15f)
        {
            gushOutEffect.Stop();
            Spin();
        }
    }


    void Spin()
    {
        //isTurn = false;
        isSpinning = true;
        anim.SetBool("Spin", true);
        if (!isSpinDirectionSet)
        {
            //followDirection = (player.position - transform.position).normalized;
            //followDirection = player.position.x < transform.position.x ? Vector2.left : Vector2.right;
            spinDirection = player.position.x < transform.position.x ? -1f : 1f;
            isSpinDirectionSet = true;
        }
        
        rbBoss.velocity = new Vector2(spinDirection * spinSpeed, rbBoss.position.y);

        // 속도 제한
        if (rbBoss.velocity.magnitude > spinSpeed)
            rbBoss.velocity = rbBoss.velocity.normalized * spinSpeed;

        if (spinTimer >= 5f) // Spin 5초 이상 지나면
        {
            anim.SetBool("Spin", false);
            isSpinning = false;
            //isTurn = false;

            // 타이머를 다시 0으로 리셋
            gushOutTimer = 0f;
            chompTimer = 0f;
            spinTimer = 0f;

            // 방향을 다시 설정할 수 있도록 초기화
            isSpinDirectionSet = false;
        }
    }
    void GroundCheckRay()
    {
        // 앞에 땅이 있는지 체크
        Vector2 frontCheck = new Vector2(rbBoss.position.x + 3f * spinDirection, rbBoss.position.y);
        Debug.DrawRay(frontCheck, Vector2.down * 4f, Color.blue); // 레이를 시각적으로 표시
        rayHit = Physics2D.Raycast(frontCheck, Vector2.down, 4f, LayerMask.GetMask("groundLayer"));

        //// 앞에 땅 없으면 방향 전환
        if (rayHit.collider == null)
        {
            spinDirection *= -1;
            //Spin();
            //isTurn = true;
            //StopSpin();
        }
    }

    //void StopSpin()
    //{
    //    if (isTurn && turnTimer < 1.5f)
    //    {
    //        rbBoss.velocity = Vector2.zero;
    //        Debug.Log("StopSpin!");
    //        turnTimer += Time.deltaTime;
    //    }
    //    else Spin(); 
    //}

    void Follow()
    {
        rbBoss.velocity = new Vector2(followDirection * speed, rbBoss.velocity.y);

        //Vector2 followDirection = new Vector2(player.position.x, rbBoss.position.y);
        // newPos =  Vector2.MoveTowards(rbBoss.position, target, speed * Time.deltaTime);
        //rbBoss.MovePositi//(newPos);
    }
    void GushOut()
    {
        if (!gushOutEffect.isPlaying && gushOutTimer < 10f)
        {
            // 플레이어의 위치와 보스의 위치를 비교하여 힘을 가할 방향을 계산
            //rbBoss.AddForce(followDirection * 500f, ForceMode2D.Impulse);
            rbBoss.AddForce(new Vector2(followDirection * 500f, 0f), ForceMode2D.Impulse);

            if (rbBoss.velocity.magnitude > 30f)
            {
                Debug.Log("Boss GushOut!");
                rbBoss.velocity = rbBoss.velocity.normalized * 30f;
            }
            anim.SetBool("GushOut", true);
            gushOutEffect.Play();
        }
    }

    void Chomp()
    {
        if (chompTimer < 10f)
        {
            gushOutEffect.Stop();
            anim.SetBool("Chomp", true);
        }
    }

    void LookAtPlayer()
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;

        if (transform.position.x > player.position.x && isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = false;
        }
        else if (transform.position.x < player.position.x && !isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
        }
    }

    public void TakeDamage()
    {
        //bossIsHurted = true;
        //anim.SetBool("Hurt", true);
        //health--;
        //if (health <= 0)
        //{
        //    Faint(); // 체력이 0 이하일 경우 몬스터 삭제
        //}
        //else
        //{
        //    rbBoss.velocity = Vector2.zero;
        //    if (transform.position.x > player.transform.position.x)
        //    {
        //        rbBoss.velocity = new Vector2(10f, 0);
        //    }
        //    else
        //    {
        //        rbBoss.velocity = new Vector2(-10f, 0);
        //    }
        //}
    }

    // Enemy 녹다운
    void Faint()
    {
        // 레이어보단 is trigger 로 온오프 시키는게 나을듯. 추후 변경 예정
        // 현재 오브젝트의 레이어를 9번(Mob)으로 변경합니다.
        gameObject.layer = 9;
        Physics2D.IgnoreLayerCollision(9, 7); // Fainted(9)과 Player(7) 충돌 무시
        Physics2D.IgnoreLayerCollision(9, 8); // Fainted(9)과 Attack(8) 충돌 무시 
        Physics2D.IgnoreLayerCollision(9, 6); // Fainted(9)과 Enemy(6)  충돌 무시
        bossIsFainted = true;

        anim.SetBool("Faint", true);
        Debug.Log("Enemy Knock Down-!!");
    }
}
