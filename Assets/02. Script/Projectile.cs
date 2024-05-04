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
    private EnemyAnimScr enemyAnimScr;
    private PlayerScr player;
    private Vector2 direction = Vector2.right; // �߻�ü �⺻ ����(R)

    [SerializeField] private float speed = 4.5f;
    void Start()
    {
        player = FindObjectOfType<PlayerScr>(); // Player ��ü ã��
        
        // �߻�ü�� �ӵ� ����
        Rigidbody2D rbAmmo = GetComponent<Rigidbody2D>(); // ��ü ź(�߻�ü)
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
        if (collision.gameObject.CompareTag("Enemy"))
        {
            gameObject.SetActive(false);
            enemy.TakeDamage();
            //Destroy(collision.gameObject);
        }
        

    }

    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    EnemyScr enemy = other.GetComponent<EnemyScr>(); // �浹�� ������Ʈ�� �������� Ȯ��

    //    if (enemy != null)
    //    {
    //        enemy.TakeDamage(); // ������ TakeDamage() ȣ��
    //    }

    //    Destroy(gameObject); // �߻�ü ����
    //}

}