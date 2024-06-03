using Assets;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using static UnityEngine.UI.Image;

public class MobMoving : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float distance = 5f;
    [SerializeField] private float jumpPower = 10f;
    private SpriteRenderer spriteRenderer;
    Transform player;
    Animator anim;
    Rigidbody2D rb;
    LayerMask groundLayer;

    void Start()
    {
    }
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        groundLayer = LayerMask.GetMask("groundLayer");
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        Physics2D.IgnoreLayerCollision(7, 9); // Player�� Mob �浹 ����
        //Physics2D.IgnoreLayerCollision(6, 3); // Enemy ������ Player�� Enemy �浹 ����, ������ �÷��̾� HP ����
    }
    void Update()
    {
        // ��������Ʈ �¿� ��ȯ
        // M < P : TRUE �� ���� ������ ���� ��  
        spriteRenderer.flipX = (transform.position.x < player.position.x);
        // �� �̵� ����(���� �÷��̾� ���󰡾���(���� ���� ����(-)�� ū ���� ����(+) ���󰣴ٰ� �����ϱ�)
        float direction = transform.position.x < player.position.x ? 1 : -1;
        
        // ���߿� �� �̵� ������ ������ �迭 ���� ���η� �ٲ� ����.
        if (Mathf.Abs(transform.position.x - player.position.x) > distance)
        {
            transform.Translate(new Vector2(direction, 0) * Time.deltaTime * speed);
            anim.SetBool("IsWalk", true);

            // DrawRay �߰�
            Debug.DrawRay(transform.position, new Vector2(direction, 0) * 5f, Color.magenta);
            Debug.DrawRay(transform.position, new Vector2(1 * direction, 1) * 5f, Color.blue);

            RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(direction, 0), 5f, groundLayer);
            RaycastHit2D hitdia = Physics2D.Raycast(transform.position, new Vector2(1 * direction, 1), 5f, groundLayer);

            // �÷��̾ ������ �Ʒ��� ���� �� ����ĳ��Ʈ ��ȯ �� �����
            // �� ȥ�� ���� ���� �ö󰡴� ��츦 ���� ����
            if (player.position.y - transform.position.y <= 0)
                hitdia = new RaycastHit2D();

            // ���� ����ĳ��Ʈ�� ������ ����
            if (hit || hitdia)
            {
                Debug.Log("�� �����Ѵ�");
                rb.velocity = Vector2.up * jumpPower;
            }
        }   
        else 
        {
            anim.SetBool("IsWalk", false);
        }
    }
}
