using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Followers // 네임스페이스를 추가하여 충돌 방지
{
    public class Follower : MonoBehaviour
    {
        [SerializeField] float telDistance = 20f;
        [SerializeField] float teleportDelay = 3f;
        [SerializeField] float sineSpeed = 3.2f;
        public Transform parent;

        float startingPos;
        float endPos;
        float direction;
        Rigidbody2D rb;
        SpriteRenderer spriteRenderer;
        Transform player;
        Animator anim;
        LayerMask groundLayer;

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            anim = GetComponent<Animator>();

            groundLayer = LayerMask.GetMask("groundLayer");
            player = GameObject.FindGameObjectWithTag("Player").transform;
            Physics2D.IgnoreLayerCollision(7, 9);

            endPos = transform.position.y + 0.8f;
            startingPos = transform.position.y + 0.3f;
            StartCoroutine(EnableTeleportAfterDelay());
        }

        void Start()
        {
        }

        void Update()
        {
            spriteRenderer.flipX = (transform.position.x < player.position.x);
            direction = transform.position.x < player.position.x ? 1 : -1;
        }

        void FixedUpdate()
        {
            Sine();
        }

        void Sine()
        {
            float y = Mathf.Lerp(startingPos, endPos, (Mathf.Sin(Time.time * sineSpeed) + 1) / 2);
            Vector2 targetPosition = new Vector2(rb.position.x, y);
            rb.MovePosition(targetPosition);
        }

        void TeleportToPlayer()
        {
            if (Vector2.Distance(player.position, transform.position) > telDistance)
                transform.position = player.position;
        }

        IEnumerator EnableTeleportAfterDelay()
        {
            yield return new WaitForSeconds(teleportDelay);
        }
    }
}
