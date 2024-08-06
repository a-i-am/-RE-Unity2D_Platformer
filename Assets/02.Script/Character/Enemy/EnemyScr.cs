using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.Experimental.GraphView;
using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class EnemyScr : MonoBehaviour
{

    public Character.CharacterData characterData; // 캐릭터(몹) 데이터
    //public SpriteRenderer image;

    public void SetCharacter(Character.CharacterData _character)
    {
        characterData = _character;
        //image.sprite = _character.characterImage;
    }

    public Character.CharacterData GetCharacter()
    {
        if (characterData == null)
        {
            Debug.LogError("GetCharacter returned null");
        }
        else if (characterData.characterPrefab == null)
        {
            Debug.LogError("GetCharacter returned a character with a null prefab");
        }
        return characterData;
    }

    public GameObject GetCharacterPrefab()
    {
        return characterData.characterPrefab;
    }

    public void DestroyCharacter()
    {
        Destroy(gameObject);
    }

    [SerializeField] private float followInterval = 0.1f; // 따라가는 간격
    [SerializeField] private float chaseDistance = 8f;
    [SerializeField] private float moveDistance = 15f;
    [SerializeField] private float moveSpeed = 5f; // 몬스터의 이동 속도
    [SerializeField] private float chaseSpeed; // 몬스터가 플레이어를 빨리 쫓아갈 때 속도
    [SerializeField] private int health = 3; // 몬스터의 체력

    [SerializeField] private Transform player; // 플레이어의 Transform을 저장하는 변수
    [SerializeField] private LayerMask groundLayer; // 땅을 나타내는 레이어

    private bool enemyIsGrounded;
    private bool enemyIsHurted = false;
    public bool enemyIsFainted;
    private EnemyAnimScr enemyAnimScr;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rbEnemy;
    void Start()
    {
        // Player 오브젝트를 찾아서 player에 할당
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rbEnemy = GetComponent<Rigidbody2D>();
        enemyAnimScr = GetComponent<EnemyAnimScr>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Physics2D.IgnoreLayerCollision(6, 6);
        // StartCoroutine을 사용하여 코루틴 시작
        StartCoroutine(FollowPlayer());
    }

    // Update is called once per frame
    void Update()
    { 

    }

    void FixedUpdate()
    {
        EnemyRay();
        OffCollider();
    }

    void OffCollider()
    {
        if (enemyIsGrounded == false)
        {
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
        while (!enemyIsHurted && !enemyIsFainted)
        {
            // 몬스터의 이동 방향 설정
            // 플레이어가 적의 오른쪽에 있는지 여부를 확인 후, 스프라이트 뒤집기
            spriteRenderer.flipX = (player.position.x > transform.position.x);

                
            if (Vector2.Distance(transform.position, player.position) < moveDistance)
            {
                rbEnemy.velocity = new Vector2((player.position.x < transform.position.x ? -1 : 1) * moveSpeed, rbEnemy.velocity.y);
                enemyAnimScr.WalkAnimation(true);
                
                if (Vector2.Distance(transform.position, player.position) < chaseDistance)
                {
                    moveSpeed = chaseSpeed; // 일정 거리 이하면 추적 속도로 변경
                    enemyAnimScr.WalkAnimation(true);
                }
            }
            else enemyAnimScr.WalkAnimation(false);
            yield return new WaitForSeconds(followInterval);

            // 걷기 애니메이션 실행 조건 설정
            //if (rbEnemy.velocity.magnitude > 0)
            //{

            //        //}
            //else
            //{

            //}
        }
    }

    public void TakeDamage()
    {
        enemyIsHurted = true;
        enemyAnimScr.HurtAnimation();
        StartCoroutine(ResetHurtedStateAfterDelay(0.5f)); // 0.5초 후에 enemyIsHurted를 false로 변경
        health--;
        if (health <= 0)
        {
            Faint(); // 체력이 0 이하일 경우 몬스터 삭제
        }
    }
    IEnumerator ResetHurtedStateAfterDelay(float delayTime)
    {
        yield return new WaitForSeconds(delayTime); // delayTime(초) 후까지 대기
        // delayTime(초) 후에 실행될 명령
        enemyIsHurted = false;
        StartCoroutine(FollowPlayer());
    }

    // Enemy 녹다운
    void Faint()
    {
        // 현재 오브젝트의 레이어를 9번(Mob)으로 변경합니다.
        gameObject.layer = 9;
        Physics2D.IgnoreLayerCollision(9, 7); // Mob(9)과 Player(7) 충돌 무시
        Physics2D.IgnoreLayerCollision(9, 8); // Mob(9)과 Attack(8) 충돌 무시 
        Physics2D.IgnoreLayerCollision(9, 6); // Mob(9)과 Enemy(6)  충돌 무시
        enemyIsFainted = true;
        enemyAnimScr.FaintAnimation(true);
        //enemyIsHurted = false;
        Debug.Log("Enemy Knock Down-!!");
    }
}
