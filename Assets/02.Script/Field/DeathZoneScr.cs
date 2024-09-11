using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DeathZoneScr : MonoBehaviour
{
    void Start(){ 
      // 이벤트를 보낼 스크립트의 인스턴스를 찾아서 이벤트 핸들러에 등록
      //GameManager gameManager = FindObjectOfType<GameManager>();
    }

    void Update(){
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")){
            // 플레이어가 사망 지역에 진입하면 GameManager의 DeadJump 메소드 호출
            GameManager.Instance.OnDeath();
            Debug.Log("데스존 게임오버!");
            #region GameManager.Instance.OnDeath();

            // 플레이어가 사망 지역에 진입하면 GameManager의 OnDeath 메소드 호출
            //GameManager.Instance.OnDeath();
            #endregion
        }

        //GameObject[] gameObjects;
        //gameObjects = GameObject.FindGameObjectsWithTag("Enemy");

        if (collision.gameObject.CompareTag("Enemy")) {
            Destroy(collision.gameObject);
            Debug.Log("적이 삭제되었습니다!");
        }
    }
}


