using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    Animator[] animators;

    void Start()
    {
        // 현 개체와 모든 자식들의 Animator 컴포넌트를 가져옴
        animators = GetComponentsInChildren<Animator>();
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            // 모든 Animator에 대해 IsActing 값을 true로 설정
            foreach (Animator animator in animators)
            {
                animator.SetBool("IsActing", true);
            }

        }
    }


}
