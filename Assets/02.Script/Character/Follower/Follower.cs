using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Follower : MonoBehaviour
{
    public bool isSineActive = true; // Sine 애니메이션 활성화 여부
    //[SerializeField] float telDistance = 20f;
    //[SerializeField] float teleportDelay = 3f;
    [SerializeField] float moveSpeed = 1f;

    public Transform parent;

    float inputHorizontal;
    float amplitude = 1.0f; // sine 파동의 높이
    float frequency = 1.0f; // sine 파동의 주기
    float startY;
    float startingPos;
    float endPos;
    float direction;
    float sineY;
    SpriteRenderer spriteRenderer;
    Transform player;
    Animator anim;
    LayerMask groundLayer;
    Vector3 pos;
    Collider2D lastGroundCollider; // 마지막으로 닿은 땅의 Collider 정보를 저장할 변수



    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        groundLayer = LayerMask.GetMask("groundLayer");
        player = GameObject.FindGameObjectWithTag("Player").transform;
        Physics2D.IgnoreLayerCollision(7, 9);
    }

    void Start()
    {
        startY = transform.position.y;

    }

    void Update()
    {
        inputHorizontal = Input.GetAxis("Horizontal");
        
        if (inputHorizontal < 0)
            spriteRenderer.flipX = false;
        else
            spriteRenderer.flipX = true;

            Sine();
    }

    void FixedUpdate()
    {
        ResetStartY();
    }

    public void SetSineActive(bool active)
    {
        isSineActive = active;
    }

    void Sine()
    {
        if(isSineActive) 
        {
        sineY = startY + Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = new Vector2(transform.position.x, sineY); // Sine()에서 계산된 Y축 위치 사용
        }
    }

    void ResetStartY()
    {
        // 캐릭터의 아래에 있는 Collider의 절반 크기만큼의 레이를 쏘아서 땅과 충돌하는지 여부를 검사
        Vector2 raycastStart = new Vector2(player.transform.position.x, player.transform.position.y - 2f);
        RaycastHit2D hit = Physics2D.Raycast(raycastStart, Vector2.down, 0.2f, LayerMask.GetMask("groundLayer"));
        Debug.DrawRay(raycastStart, Vector2.down * 0.2f, Color.magenta); // 레이를 시각적으로 표시

        if (hit.collider != null) // && 닿은 오브젝트의 태그가 movingPlatform이 아닌 경우에만!
        {
            if(hit.collider != lastGroundCollider) // 현재 닿아있는 땅이 이전 땅과 다를 경우에만 실행
            {
                lastGroundCollider = hit.collider; // 현재 닿아있는 땅을 업데이트
                startY = hit.point.y + 5f;
            }
            
        }
    }



}
