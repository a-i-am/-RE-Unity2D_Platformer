using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Side_Crawl : StateMachineBehaviour
{
    public float speed;
    public float attackRange = 3f;

    Transform player;
    Rigidbody2D rb;
    Boss boss;

    // OnStateEnter는 전환이 시작되고 상태 시스템이 이 상태를 평가하기 시작할 때 호출됩니다.
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.transform.GetComponent<Rigidbody2D>();
        boss = animator.GetComponent<Boss>();
    }

    // OnStateUpdate는 OnStateEnter와 OnStateExit 콜백 사이의 각 업데이트 프레임에서 호출됩니다
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss.LookAtPlayer();

        Vector2 target = new Vector2(player.position.x, rb.position.y);
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.deltaTime);
        rb.MovePosition(newPos);

        if (Vector3.Distance(player.position, rb.position) <= attackRange)
        {
            //Debug.Log("Boss mm Atack!");
            //animator.SetTrigger("Chomp");
        }

    }

    // OnStateExit는 전환이 끝나고 상태 머신이 이 상태 평가를 마치면 호출됩니다.
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //animator.ResetTrigger("Chomp");
    }


}
