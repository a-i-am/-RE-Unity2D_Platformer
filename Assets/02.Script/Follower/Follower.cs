using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Transform player;

    // Start is called before the first frame update
    void Start()
    {
    }

    void Awake()
    {
        Physics2D.IgnoreLayerCollision(7, 9); // Player(7)와 Mob(9) 충돌 무시
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

    }
    // Update is called once per frame
    void Update()
    {
        // 스프라이트 좌우 전환
        spriteRenderer.flipX = (transform.position.x < player.position.x);
    }


}
