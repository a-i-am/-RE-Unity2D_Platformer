using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField]
    private Slider hpBar;
    private float maxHp = 100;
    private float curHp = 100;
    float imsi;

    void Start()
    {
        hpBar.value = (float)curHp / (float)maxHp;
    }

    void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (curHp > 0) { curHp -= 10;}
            else { curHp = 0; }
        }
        HandleHp();
    }

    private void HandleHp()
    {
        imsi = (float)curHp / (float)maxHp;
        hpBar.value = Mathf.Lerp(hpBar.value, imsi, Time.deltaTime * 10);
        //hpBar.value = (float)curHp / (float)maxHp;
        //hpBar.value = Mathf.Lerp(hpBar.value, (float)curHp / (float)maxHp, Time.deltaTime * 10);

    } 
}
