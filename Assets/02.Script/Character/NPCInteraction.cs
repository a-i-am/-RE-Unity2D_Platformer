using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class NPCInteraction : MonoBehaviour
{
    Animator anim;
    void Start()
    {
        anim.SetBool("IsActing", true);
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        
    }
}
