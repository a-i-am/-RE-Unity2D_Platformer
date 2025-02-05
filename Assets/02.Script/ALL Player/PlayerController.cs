using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;
#region 플레이어 로직 요약
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
#endregion

public class PlayerController : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    static Rigidbody2D rb;
    bool deadWait; // 사망 시 다음 동작 지연
    bool respawnOrDead;
    float inputHorizontal;
    [SerializeField]float walkSpeed = 2f;
    Vector2 currentVelocity;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        Managers.Init();  // Managers 초기화 보장
    }

    void Start()
    {
        //GameManager.Instance.gameOverDele += OnDeath;
        Managers.Input.keyAction -= OnKeyboard;
        Managers.Input.keyAction += OnKeyboard;

    }
   
    private void Reset()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    void OnKeyboard()
    {
        inputHorizontal = Input.GetAxis("Horizontal");
        if(inputHorizontal > 0 || inputHorizontal < 0)
        {
            Walk();
        }
    }

    void Walk()
    {
        //currentVelocity = new Vector2(inputHorizontal * walkSpeed, rb.velocity.y);
        //rb.velocity = currentVelocity;
        // 기존의 이동 속도와 방향을 계산하여 포지션을 변경
        Vector3 moveDirection = new Vector3(inputHorizontal * walkSpeed, 0f, 0f);
        transform.position += moveDirection * Time.deltaTime; // 이동

        spriteRenderer.flipX = inputHorizontal < 0;
    }


  


}
