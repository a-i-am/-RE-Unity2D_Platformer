using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using TMPro;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private int followerLayer = 9;    // "Follower" 레이어의 번호
    private PlayerScr player;
    private Vector2 direction = Vector2.right; // 발사체 기본 방향(R)

    [SerializeField] private float speed = 4.5f;
    void Start()
    {
        Physics2D.IgnoreLayerCollision(7, 8); // Player(7)과 Attack(8) 충돌 무시 
        Physics2D.IgnoreLayerCollision(9, 8); // Follower(9)과 Attack(8) 충돌 무시
        player = FindObjectOfType<PlayerScr>(); // Player 객체 찾기

        // 발사체에 속도 적용
        Rigidbody2D rbAmmo = GetComponent<Rigidbody2D>(); // 강체 탄(발사체)
        rbAmmo.velocity = direction * speed;
        
        if (player.GetComponent<SpriteRenderer>().flipX)
        {
            rbAmmo.velocity = Vector2.left * speed;
        }

    }

    // Update is called once per frame
    void Update()
    {
        //Normal();
    }

    private void FixedUpdate()
    {

    }

    //private void Normal()
    //{
    //    rbAmmo.velocity = Vector2.right * speed;
    //}



    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collidedObject = collision.gameObject;
        // 적과 충돌했을 때
        if (collision.gameObject.tag == "Enemy")
        {
            EnemyScr enemy = collidedObject.GetComponent<EnemyScr>();
            if (enemy != null)
            {
                enemy.TakeDamage();
            }
        }
        Destroy(gameObject);
    }
}
