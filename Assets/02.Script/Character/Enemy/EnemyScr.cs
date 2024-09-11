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
    private int nextMove;
    [SerializeField] private float moveSpeed = 5f; // 몬스터의 이동 속도
    [SerializeField] private float chaseDistance = 8f;
    [SerializeField] private float stopDistance = 2f;
    [SerializeField] private float chaseSpeed = 15f; // 몬스터가 플레이어를 빨리 쫓아갈 때 속도
    [SerializeField] private int health = 3; // 몬스터의 체력

    [SerializeField] private Transform player; // 플레이어의 Transform을 저장하는 변수
    [SerializeField] private LayerMask groundLayer; // 땅을 나타내는 레이어

    //private bool enemyIsGrounded;
    private bool enemyIsHurted = false;
    public bool enemyIsFainted;
    private EnemyAnimScr enemyAnimScr;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rbEnemy;
    RaycastHit2D rayHit;
    void Start()
    {
        // Player 오브젝트를 찾아서 player에 할당
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rbEnemy = GetComponent<Rigidbody2D>();
        enemyAnimScr = GetComponent<EnemyAnimScr>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Physics2D.IgnoreLayerCollision(6, 6); // Enemy 끼리 충돌 방지
        Physics2D.IgnoreLayerCollision(6, 7); // Enemy 와 Player 충돌 방지
        Invoke("Think", 5);
    }

    void FixedUpdate()
    {
        // Move
        if(!enemyIsHurted && !enemyIsFainted)
        {
            rbEnemy.velocity = new Vector2(moveSpeed * nextMove, rbEnemy.velocity.y);
            GroundCheckRay();
            SpeedUpForChasePlayer();
  
        }
    }


    void Think()
    {
        // Set Next Active
        nextMove = Random.Range(-1, 2);
        
        enemyAnimScr.WalkAnimation(nextMove);

        // Flip Sprite
        if (nextMove != 0)
            spriteRenderer.flipX = nextMove == 1;

        // Recursive
        float nextThinkTime = Random.Range(2f, 5f);
        Invoke("Think", nextThinkTime);
    }

    void SpeedUpForChasePlayer()
    {
        if (Vector2.Distance(rbEnemy.position, player.position) < chaseDistance)
        {
            moveSpeed = chaseSpeed;
            
            if (Vector2.Distance(rbEnemy.position, player.position) < stopDistance)
            {
                moveSpeed = 0;
            }
        }
    }

    void GroundCheckRay()
    {
        // 앞에 땅이 있는지 체크
        Vector2 frontCheck = new Vector2(rbEnemy.position.x + nextMove, rbEnemy.position.y);
        Debug.DrawRay(frontCheck, Vector2.down, Color.red); // 레이를 시각적으로 표시
        rayHit = Physics2D.Raycast(frontCheck, Vector2.down, 1, LayerMask.GetMask("groundLayer"));

        // 앞에 땅 없으면 방향 전환
        if (rayHit.collider == null)
        {
            Turn();
        }
    }
    void Turn()
    {
        nextMove *= -1;
        spriteRenderer.flipX = nextMove == 1;
        CancelInvoke();
        Invoke("Think", 2);
    }

    public void TakeDamage()
    {
        enemyIsHurted = true;
        enemyAnimScr.HurtAnimation();


        health--;
        if (health <= 0)
        {
            Faint(); // 체력이 0 이하일 경우 몬스터 삭제
        }
        else
        {
            rbEnemy.velocity = Vector2.zero;
            if (transform.position.x > player.transform.position.x)
            {
                rbEnemy.velocity = new Vector2(10f, 0);
            }
            else
            {
                rbEnemy.velocity = new Vector2(-10f, 0);
            }
        }
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
        enemyIsFainted = true;
        enemyAnimScr.FaintAnimation(true);
        //enemyIsHurted = false;
        Debug.Log("Enemy Knock Down-!!");
    }
}
