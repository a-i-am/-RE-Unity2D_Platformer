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
    private PlayerScr player;
    private Vector2 direction = Vector2.right; // 발사체 기본 방향(R)

    [SerializeField] private float speed = 4.5f;
    void Start()
    {
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
        EnemyScr enemy = collision.gameObject.GetComponent<EnemyScr>();
        // "groundLayer" 레이어의 레이어 번호를 미리 가져옵니다.
        int groundLayer = LayerMask.NameToLayer("groundLayer");
        if (collision.gameObject.CompareTag("Enemy"))
        {
            enemy.TakeDamage();
            Destroy(gameObject);
            //Destroy(collision.gameObject);
        }

        if(collision.gameObject.layer == groundLayer)
        {
            //gameObject.SetActive(false);
            Destroy(gameObject);
        }

    }

    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    EnemyScr enemy = other.GetComponent<EnemyScr>(); // 충돌한 오브젝트가 몬스터인지 확인

    //    if (enemy != null)
    //    {
    //        enemy.TakeDamage(); // 몬스터의 TakeDamage() 호출
    //    }

    //    Destroy(gameObject); // 발사체 삭제
    //}

}