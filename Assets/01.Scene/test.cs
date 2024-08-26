using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public Transform target; // 이동할 목표가 될 게임 오브젝트
    private Vector3 originalPosition; // 원래 위치
    public float moveSpeed = 5f; // 이동 속도

    void Start()
    {
        originalPosition = transform.position; // 시작 시 원래 위치 저장
    }
    void Update()
    {
        // 입력 받기
        float moveX = Input.GetAxisRaw("Horizontal"); // 좌우 입력 (A/D 또는 화살표 키)
        float moveY = Input.GetAxisRaw("Vertical");   // 위아래 입력 (W/S 또는 화살표 키)

        // 이동 처리
        Vector3 movement = new Vector3(moveX, moveY, 0f);
        transform.Translate(movement * moveSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space)) // 스페이스 키를 눌렀을 때 이동 시작
        {
            MoveToTarget();
        }

        // 움직일 때마다 새로운 위치를 원래 위치로 설정
        if (movement != Vector3.zero)
        {
            originalPosition = transform.position;
        }

    }
    public void MoveToTarget()
    {
        // 목표 위치를 동적으로 가져와 이동
        if (target != null)
        {
            Vector3 targetPosition = target.position; // 현재 목표 위치
            transform.DOMove(targetPosition, 1f).OnComplete(ReturnToOriginalPosition);
        }
    }

    private void ReturnToOriginalPosition()
    {
        // 원래 위치로 돌아옴
        transform.DOMove(originalPosition, 1f);
    }




}
