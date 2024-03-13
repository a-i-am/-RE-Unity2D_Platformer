using JetBrains.Annotations;
using System.Collections;
using UnityEngine;

namespace Assets
{
    public class PlayerAnimScr : MonoBehaviour
    {
        private Animator anim;
        private float inputHorizontal;
        
        void Awake()
        {
            anim = GetComponentInChildren<Animator>();
        }
        #region MoveSpeedComment
        //public float MoveSpeed
        //{
        //    set => anim.SetFloat("movementSpeed", value);
        //    get => anim.GetFloat("movementSpeed");
        //}
        #endregion
        void Update()
        {
            var isGroundClass = GetComponent<PlayerScr>();

            // Walk Anim
            inputHorizontal = Input.GetAxisRaw("Horizontal");
            if (inputHorizontal != 0 && isGroundClass.isGrounded)
            {
                anim.SetTrigger("Walking");
            }
            else { anim.ResetTrigger("Walking"); }

            // Jump Anim
            //if (Input.GetKeyDown(KeyCode.Space) && isGroundClass.isGrounded)
            //{
            //    anim.SetBool("Jump", true);
            //}
            //else if (Input.GetKeyUp(KeyCode.Space))
            //{ anim.SetBool("Jump", false); }

            if (Input.GetButton("Jump") && isGroundClass.isGrounded)
            {
                anim.SetBool("Jump", true);
            }
            else if (Input.GetButtonUp("Jump"))
            { anim.SetBool("Jump", false); }

            #region isJumpingComment
            //private bool isJumping = false; // 점프 모션이 실행 중인지 여부

            // SetBool("New Bool", false);
            //isJumping = true; // Jump 입력 시 점프 모션이 실행 중임을 설정
            //if (!isJumping) // 점프 모션이 실행 중이 아닐 때에만 Idle 모션으로 전환
            //{
            //    animator.SetBool("IdleAnimation", true);
            //}
            #endregion
        }

        // DeadJump Anim(GameOver action 1)
        public void DeadJumpAnimation(bool fallDead)
        {
            anim.SetBool("DeadJump", fallDead);
        }

    }
}