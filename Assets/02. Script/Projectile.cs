using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
        if (collision.gameObject.CompareTag("Enemy"))
        {
            gameObject.SetActive(false);
            Destroy(collision.gameObject);
        }
    }

}