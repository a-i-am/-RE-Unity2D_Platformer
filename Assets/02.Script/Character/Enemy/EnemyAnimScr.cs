using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyAnimScr : MonoBehaviour
{
    private Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    //void OnCollisionEnter2D(Collision2D other)
    //{
    //    if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
    //    {
    //        EnemyScr enemy = other.gameObject.GetComponent<EnemyScr>();
    //        dashHitVFX.transform.position = enemy.transform.position;
    //        if (enemy != null)
    //        {
    //            enemy.TakeDamage();
    //            dashHitVFX.Play();
    //        }
    //    }
    //}

    // Update is called once per frame
    void Update()
    {
    }
    public void WalkAnimation(int walkSpeed)
    {
        anim.SetInteger("WalkSpeed", walkSpeed);
    }

    public void HurtAnimation()
    {
        anim.SetTrigger("Hurt");
    }

    public void FaintAnimation(bool isFaint)
    {
        anim.SetBool("IsFaint", isFaint);
    }

    public void RespawnAnimation()
    {
        anim.SetTrigger("IsRespawn");
    }

}
