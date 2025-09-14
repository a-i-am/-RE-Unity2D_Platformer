using System.Collections.Generic;
using UnityEngine;

public class FollowerGroupMoving : MonoBehaviour
{
    // Follow
    [SerializeField] private float moveDistance;
    [SerializeField] private float moveSpeed;
    private Transform player;
    public float startY; // sine 시작 위치

    private Animator anim;
    private LayerMask groundLayer;

    // Sine
    //[SerializeField] float sineSpeed = 3.2f;
    [SerializeField] private float amplitude = 2f; // sine 파동의 높이(움직임 범위)
    [SerializeField] private float frequency = 1.0f; // sine 파동의 주기(운동 간격)
    public bool isSineActive = true;
    private Collider2D lastGroundCollider; // sine 시작 높이 갱신용 콜라이더

    void Awake()
    {
        anim = GetComponent<Animator>();
        groundLayer = LayerMask.GetMask("groundLayer");
        Physics2D.IgnoreLayerCollision(7, 9); // Mob(9)과 Player(7) 충돌 무시
    }

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        FollowPlayer();
    }
    private void FixedUpdate()
    {
        Sine();
        ResetStartY();
    }

    private void FollowPlayer()
    {
        if (Mathf.Abs(transform.position.x - player.position.x) <= moveDistance) return;

        float direction = (player.position.x - transform.position.x) > 0 ? 1 : -1;
        transform.Translate(new Vector2(direction, 0) * Time.deltaTime * moveSpeed);
    }

    // 위아래로 움직임(둥둥 뜨는 연출)
    void Sine()
    {
        if (isSineActive)
        {
            float sineY = startY + Mathf.Sin(Time.time * frequency) * amplitude;
            transform.position = new Vector2(transform.position.x, sineY); // Sine()에서 계산된 Y축 위치 사용
        }
    }

    void ResetStartY()
    {
        if (!isSineActive) return;

        Vector2 raycastStart = new Vector2(player.transform.position.x, player.transform.position.y - 2f);
        RaycastHit2D hit = Physics2D.Raycast(raycastStart, Vector2.down, 0.2f, LayerMask.GetMask("groundLayer"));
        Debug.DrawRay(raycastStart, Vector2.down * 0.2f, Color.magenta);

        if (hit.collider != null) // && 닿은 오브젝트의 태그가 movingPlatform이 아닌 경우에만!
        {
            if (hit.collider != lastGroundCollider)
            {
                lastGroundCollider = hit.collider;
                startY = hit.point.y + 5f;
            }
        }
    }


}