using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformScr : MonoBehaviour
{
    [SerializeField] private Transform startPos; // ���� ��������
    [SerializeField] private Transform endPos; // ���� �� ����
    [SerializeField] private Transform desPos; // ���� ��ȯ(����)����
    [SerializeField] private float speed; // ���� �̵��ӵ�

    void Start()
    {
        transform.position = startPos.position;
        desPos = endPos;
    }
    void FixedUpdate()
    {
        transform.position = Vector2.MoveTowards(transform.position, desPos.position, Time.deltaTime * speed);

        if (Vector2.Distance(transform.position, desPos.position) <= 0.05f)
        {
            if (desPos == endPos) desPos = startPos;
            else desPos = endPos;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }



}
