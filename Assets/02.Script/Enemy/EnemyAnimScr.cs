using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyAnimScr : MonoBehaviour
{
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    { anim = GetComponent<Animator>(); }
    // Update is called once per frame
    void Update()
    {
    }
    public void WalkAnimation(bool isWalk)
    {
        anim.SetBool("IsWalk", isWalk);
    }

    public void HurtAnimation()
    {
        anim.SetTrigger("Hurt");
    }

    public void FaintAnimation(bool isFaint)
    {
        anim.SetBool("IsFaint", isFaint);
    }

}
