using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllPlayerAction : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;
    bool deadWait; // 사망 시 다음 동작 지연
    bool respawnOrDead;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }
    private void Reset()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        GameManager.Instance.gameOverDele += OnDeath;
    }
    void Update()
    {

    }
    public void OnDeath() // OnDeath로 플레이어 죽음 하나로 묶음 => gameOverDele & OnDeath() 불러오기  
    {
        // 1)데스존에 빠짐 2)체력 쓰러짐
        StartCoroutine(DeadJump());
        rb.constraints = RigidbodyConstraints2D.FreezePositionX;
        // 게임오버 화면 전환
        //Invoke("OnDeath", 1.5f); // 해당 메소드 지연 후 실행
    }
    IEnumerator DeadJump()
    {
        respawnOrDead = true;
        //GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

        Debug.Log("DeadJumpStart");
        //playerAnimScr.DeadJumpAnimation(true);
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
    //모든 플레이어는
    // 지면 판정이 존재하고,

    // 애니메이션이 존재하고 // 애니메이션 유형화

    // 죽고(죽지 않고, 키입력이 있으면) // 죽음/데드점프/리스폰 유형화
    // 죽을 때, 접속 이상일 때
    // 데드존에 걸리면 데드점프 되고 
    // 리스폰 되고

    // 공격/피격 판정이 있고 // 공격, 피격 유형화
    // 피격 시 데미지를 입어야 하고

    // 기를 모아야 하고 // 기 유형화
    // 4단필(궁) 사용 판정이 있고 // 궁 유형화
    // 이동, 점프&코루틴타임점프, 대시&대시쿨다운하고







}
