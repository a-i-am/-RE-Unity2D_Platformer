using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField][Range(-1.0f, 1.0f)]
    private float moveSpeed = 0.1f;
    private Material material;

    void Awake()
    {
        material = GetComponent<Renderer>().material;
    }

    void Update()
    {
        material.SetTextureOffset("_MainTex", Vector2.right * moveSpeed * Time.time);
    }
}
