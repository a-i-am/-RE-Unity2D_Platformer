using Assets;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Serialization;
using Unity.Burst.CompilerServices;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.TextCore.Text;
using static UnityEditor.PlayerSettings;
using static UnityEngine.UI.Image;

public class MobGroupMoving : MonoBehaviour
{
    public bool isSineActive = false; // Sine 애니메이션 활성화 여부
    float startY;
    
    [SerializeField] float moveDistance;
    [SerializeField] float moveSpeed;
    [SerializeField] float amplitude = 2f; // sine 파동의 높이
    [SerializeField] float frequency = 1.0f; // sine 파동의 주기
    [SerializeField] float sineSpeed = 3.2f;

    float sineY;
    bool canTeleport;
    Transform player;
    Animator anim;
    LayerMask groundLayer;
    Collider2D lastGroundCollider; // 마지막으로 닿은 땅의 Collider 정보를 저장할 변수
    void Start()
    {
        startY = transform.position.y;

    }
    void Awake()
    {
        anim = GetComponent<Animator>();
        groundLayer = LayerMask.GetMask("groundLayer");
        player = GameObject.FindGameObjectWithTag("Player").transform;
        Physics2D.IgnoreLayerCollision(7, 9); // Mob(9)과 Player(7) 충돌 무시
    }

    void Update()
    {
        Sine();
        FollowPlayer();
    }

    private void FixedUpdate()
    {
        ResetStartY();
    }

    public void SetSineActive(bool active)
    {
        isSineActive = active;
    }

    void FollowPlayer()
    {
        if (Mathf.Abs(transform.position.x - player.position.x) > moveDistance)
            transform.Translate(new Vector2(-1, 0) * Time.deltaTime * moveSpeed);
        DirectionFollower();
    }

    void DirectionFollower()
    {
        //  몹 그룹이 플레이어 왼쪽에 있을 때
        // 오브젝트 위치 좌우반전
        if(transform.position.x - player.position.x < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }

    void Sine()
    {
        if (isSineActive)
        {
            sineY = startY + Mathf.Sin(Time.time * frequency) * amplitude;
            transform.position = new Vector2(transform.position.x, sineY);
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
            if (hit.collider != lastGroundCollider) // 현재 닿아있는 땅이 이전 땅과 다를 경우에만 실행
            {
                lastGroundCollider = hit.collider; // 현재 닿아있는 땅을 업데이트
                startY = hit.point.y + 5f;
            }
        }
    }
}
