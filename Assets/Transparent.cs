using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transparent : MonoBehaviour
{
    private Vector3 originalPosition; // 원래 위치를 저장할 변수
    bool isTransparent;
    void Start()
    {
        // 시작할 때 현재 위치를 원래 위치로 저장
        originalPosition = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Transparent"))
        {
            // 현재 개체의 Position Z값을 -16으로 바꾼다.
            Vector3 newPosition = new Vector3(transform.position.x, transform.position.y, -16);
            transform.position = newPosition;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Transparent"))
        {
            // 충돌이 유지되는 동안 Z값을 -16으로 계속 유지
            Vector3 newPosition = new Vector3(transform.position.x, transform.position.y, -16);
            transform.position = newPosition;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Transparent"))
        {
            // 충돌이 종료되면 원래의 Z값으로 되돌림
            transform.position = originalPosition;
        }
    }

    void Update()
    {

    }
}
