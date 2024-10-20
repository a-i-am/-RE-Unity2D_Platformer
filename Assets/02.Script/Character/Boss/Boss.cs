using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public bool isFainted = false;  // Faint 상태를 나타내는 플래그
    public ParticleSystem gushOutEffect;
    public float speed;
    public float followDistance;
    public float gushoutDistance;
    public float gushOutTimer;
    public float chompTimer;
    public float spinTimer;
    public float turnTimer;
    //private float spinCoolDown = 1.5f;
    public float spinSpeed;
    public GameObject gushOutEffectObj;
    public bool isFlipped = false;
    private bool isSpinning = false;
    private bool isSpinDirectionSet = false; // 스핀 방향 설정여부 확인
    float followDirection;
    float spinDirection;
    Animator anim;
    Transform player;
    Rigidbody2D rbBoss;

    public void SetFaint(bool faintState)
    {
        isFainted = faintState;  // Faint 상태를 설정

        if (isFainted)
        {
            // 모든 애니메이션 중지
            anim.ResetTrigger("Crawl");
            anim.SetBool("GushOut", false);
            anim.SetBool("Chomp", false);
            anim.SetBool("Spin", false);

            // 보스의 속도를 0으로 설정해서 움직임 멈춤
            rbBoss.velocity = Vector2.zero;

            // 필요하다면 파티클 이펙트도 중지
            gushOutEffect.Stop();
            gushOutEffectObj.SetActive(false);
        }

    }

    // Faint & Sleep 애니메이션 이벤트 함수
    void ChangePositionToSleep()
    {
        // Sleep Position // 현재 y축 위치에서 2.2만큼 뺀 위치로 이동 
        transform.position = new Vector3(transform.position.x, transform.position.y - 2.2f, transform.position.z);
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rbBoss = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        gushOutEffect = transform.GetChild(0).GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isFainted)
        {
            // Faint 상태일 때 로직을 멈추거나 제어
            return;
        }

        if (!isSpinning)
        {
            gushOutTimer += Time.deltaTime;
            chompTimer += Time.deltaTime;
        }
        else if (gushOutTimer >= 15f && chompTimer >= 15f)
        {
            spinTimer += Time.deltaTime;
        }

    }

    private void FixedUpdate()
    {
        if (isFainted)
        {
            // Faint 상태일 때 로직을 멈추거나 제어
            return;
        }

        LookAtPlayer();
        followDirection = player.position.x < transform.position.x ? -1f : 1f;

        if (gushOutTimer >= 15f && chompTimer >= 15f)
        {
            gushOutEffect.Stop();
            gushOutEffectObj.gameObject.SetActive(false);
            if (!isFainted) Spin();
        }

        if (!isFainted && !isSpinning && Vector3.Distance(player.position, rbBoss.position) >= followDistance)
        {
            speed = 8f;
            anim.SetBool("GushOut", false);
            Chomp();
            if (!isSpinning)
                Follow();
        }
        else if (!isSpinning && Vector3.Distance(player.position, rbBoss.position) <= followDistance &&
            Vector3.Distance(player.position, rbBoss.position) >= gushoutDistance)
        {
            anim.SetBool("Chomp", false);
            if (!isFainted) GushOut();
        }


    }

    void Spin()
    {
        if (isFainted) return;


        isSpinning = true;
        anim.SetBool("Spin", true);
        if (!isSpinDirectionSet)
        {
            spinDirection = player.position.x < transform.position.x ? -1f : 1f;
            isSpinDirectionSet = true;
        }

        rbBoss.velocity = new Vector2(spinDirection * spinSpeed, rbBoss.position.y);

        // 속도 제한
        if (rbBoss.velocity.magnitude > spinSpeed)
            rbBoss.velocity = rbBoss.velocity.normalized * spinSpeed;

        if (spinTimer >= 10f) // Spin 5초 이상 지나면
        {
            anim.SetBool("Spin", false);
            isSpinning = false;

            // 타이머를 다시 0으로 리셋
            gushOutTimer = 0f;
            chompTimer = 0f;
            spinTimer = 0f;

            // 방향을 다시 설정할 수 있도록 초기화
            isSpinDirectionSet = false;
        }
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "SpinDirectionReset")
        {
            Debug.Log("SpinDirectionReset");
            spinDirection *= -1;
        }
    }

    void Follow()
    {
        if (isFainted) return;

        rbBoss.velocity = new Vector2(followDirection * speed, rbBoss.velocity.y);
        anim.SetTrigger("Crawl");
    }
    void GushOut()
    {
        if (isFainted) return;

        if (!gushOutEffect.isPlaying && gushOutTimer < 15f)
        {
            // 플레이어의 위치와 보스의 위치를 비교하여 힘을 가할 방향을 계산
            rbBoss.AddForce(new Vector2(followDirection * 200f, 0f), ForceMode2D.Impulse);

            if (rbBoss.velocity.magnitude > 30f)
            {
                Debug.Log("Boss GushOut!");
                rbBoss.velocity = rbBoss.velocity.normalized * 30f;
            }

            anim.SetBool("GushOut", true);
            gushOutEffectObj.gameObject.SetActive(true);
            gushOutEffect.Play();
        }

    }

    void Chomp()
    {
        if (isFainted) return;

        if (chompTimer < 15f)
        {
            gushOutEffect.Stop();
            gushOutEffectObj.gameObject.SetActive(false);
            anim.SetBool("Chomp", true);
        }
    }

    void LookAtPlayer()
    {
        if (isFainted) return;

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




}
