using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    // Start is called before the first frame update
    public float ghostDelay;
    private float ghostDelaySeconds;
    public GameObject ghost;
    public bool makeGhost = false;
    // Use this for initialization
    private SpriteRenderer spriteRenderer;
    void Start()
    {
        ghostDelaySeconds = ghostDelay;
        spriteRenderer = GetComponent<SpriteRenderer>();  // SpriteRenderer 컴포넌트 참조
    }

    // Update is called once per frame
    void Update()
    {
        if (makeGhost)
        {
            if (ghostDelaySeconds > 0)
            {
                ghostDelaySeconds -= Time.deltaTime;
            }
            else
            {
                GameObject currentGhost = Instantiate(ghost, transform.position, transform.rotation);
                Sprite currentSprite = GetComponent<SpriteRenderer>().sprite;

                //currentGhost.transform.localScale = this.transform.localScale;
                currentGhost.GetComponent<SpriteRenderer>().flipX = spriteRenderer.flipX;

                currentGhost.GetComponent<SpriteRenderer>().sprite = currentSprite;
                ghostDelaySeconds = ghostDelay;
                Destroy(currentGhost, 1.5f);
                Invoke("IgnoreDamage", 0.5f);
            }
        }
    }

    void IgnoreDamage()
    {
        Physics2D.IgnoreLayerCollision(6, 7, false);
    }
}
