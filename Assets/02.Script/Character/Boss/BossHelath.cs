using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHelath : MonoBehaviour
{
    private Boss boss;
    [SerializeField] private Stat health;
    private bool bossIsHurted = false;
    private bool bossIsFainted;
    Transform player;
    Rigidbody2D rbBoss;

    Animator anim;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        boss = GetComponent<Boss>();
        rbBoss = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Awake()
    {
        health.BossHPInitialize();
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Attack")
        {
            TakeDamage();
        }
    }

    public void TakeDamage()
    {
        //bossIsHurted = true;
        anim.SetTrigger("Hurt");
        health.BossCurrentVal -= 50;
        if (health.BossCurrentVal <= 0)
        {
            Faint(); // 체력이 0 이하일 경우 몬스터 삭제
        }
        else
        {
            rbBoss.velocity = Vector2.zero;
            if (transform.position.x > player.transform.position.x)
            {
                rbBoss.velocity = new Vector2(10f, 0);
            }
            else
            {
                rbBoss.velocity = new Vector2(-10f, 0);
            }
        }
    }

    // Enemy 녹다운
    void Faint()
    {
        boss.SetFaint(true);  // 보스에게 Faint 상태 전달
        // 레이어보단 is trigger 로 온오프 시키는게 나을듯. 추후 변경 예정
        // 현재 오브젝트의 레이어를 9번(Mob)으로 변경합니다.
        gameObject.layer = 10;
        Physics2D.IgnoreLayerCollision(10, 7); // Fainted(9)과 Player(7) 충돌 무시
        Physics2D.IgnoreLayerCollision(10, 8); // Fainted(9)과 Attack(8) 충돌 무시 
        Physics2D.IgnoreLayerCollision(10, 6); // Fainted(9)과 Enemy(6)  충돌 무시
        
        //bossIsFainted = true;
        anim.SetTrigger("Faint");
        anim.SetTrigger("Sleep");

        Debug.Log("Enemy Knock Down-!!");
    }
}
