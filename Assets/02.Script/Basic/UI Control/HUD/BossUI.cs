using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossUI : MonoBehaviour
{
    [Header("HP")]
    [SerializeField] private Image bossHP;
    private float bossHPFillAmount;
    [SerializeField] private float lerpSpeed;

    // Update is called once per frame
    void Update()
    {
        HandleBossHpBar();
    }

    // Boss HP Value
    public float BossMaxHP { get; set; }

    public float BossValue
    {
        set
        {
            bossHPFillAmount = Map(value, 0, BossMaxHP, 0, 1);
        }
    }

    void HandleBossHpBar()
    {
        if (bossHPFillAmount != bossHP.fillAmount)
        {
            bossHP.fillAmount = Mathf.Lerp(bossHP.fillAmount, bossHPFillAmount, Time.deltaTime * lerpSpeed);
        }

        //if (lerpColors)
        //{
        //    bossHP.color = Color.Lerp(lowColor, fullColor, bossHPFillAmount);
        //}
    }

    private float Map(float value, float inMin, float inMax, float outMin, float outMax)
    {
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }
}
