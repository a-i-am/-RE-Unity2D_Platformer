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
        //if (transform.position != null)
        //{
        //    anim.SetBool("Walking", true);
        //}
    }
    public void WalkAnimation(bool shouldWalk)
    {
        anim.SetBool("Walking", shouldWalk);
    }
}
