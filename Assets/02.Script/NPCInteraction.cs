using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    Animator[] animators;
    [SerializeField] GameObject barrier;
    [SerializeField] GameObject barrierVFX;

    private bool barrierCreated;
    void Start()
    {
        // 현 개체와 모든 자식들의 Animator 컴포넌트를 가져옴
        animators = GetComponentsInChildren<Animator>();
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player" && !barrierCreated)
        {
            barrierCreated = true;
            Instantiate(barrierVFX, null); // 부모를 null로 설정하여 씬의 루트에 생성
            Invoke("InstantiateBarrier", 1.2f);
            Invoke("ActAnimation", 1.2f);
        }
    }

    void ActAnimation()
    {
        // 모든 Animator에 대해 IsActing 값을 true로 설정
        foreach (Animator animator in animators)
        {
            animator.SetBool("IsActing", true);
        }
    }

    void InstantiateBarrier()
    {
        Instantiate(barrier, null); // 부모를 null로 설정하여 씬의 루트에 생성
    }


}
