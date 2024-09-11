using Assets;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    private PlayerScr playerScr;
    
    // Inventory
    Inventory inven;

    [Header("HP")]
    // HP
    [SerializeField]
    private Image hpContent;
    private float hpFillAmount;

    [SerializeField]
    private float lerpSpeed;

    [SerializeField]
    private Color fullColor;
    [SerializeField]
    private Color lowColor;
    [SerializeField]
    private bool lerpColors;

    public float MaxValue { get; set; }

    public float Value
    {
        set
        {
            hpFillAmount = Map(value, 0, MaxValue, 0, 1);
        }
    }

    [Header("Charge Casting Spell Gauage")]
    // Charge Casting Spell Gauage
    [SerializeField] TextMeshProUGUI pickupMobCountText;
    [SerializeField] private Image circularSpellGauge;
    [SerializeField] float currentChargeValue = 0;
    [SerializeField] double canChargeMaxValue = 0.25;
    [SerializeField] float gaugeChargeSpeed = 25;
    

    //[SerializeField] private Slider hpBar;
    //private float maxHp = 100;
    //private float curHp = 100;
    //float imsi;

    void Start()
    {
        inven = Inventory.instance;
        playerScr = GameObject.FindWithTag("Player").GetComponent<PlayerScr>();
        //hpBar.value = (float)curHp / (float)maxHp;

        if (lerpColors)
        {
            hpContent.color = fullColor;
        }
    }

    void Update()
    {
        //ReduceHP();
        //HandleHp();
        HandleHpBar();
        ChargeSpellGauge();
    }

    void FixedUpdate()
    {
        pickupMobCountText.text = string.Format("{0}", inven.pickupMobCount);
    }

    void HandleHpBar()
    {
        //content.fillAmount = hpFillAmount;
        //hpContent.fillAmount = Map(100, 0, 100, 0, 1);
        if(hpFillAmount != hpContent.fillAmount)
        {
            //hpContent.fillAmount = hpFillAmount;
            hpContent.fillAmount = Mathf.Lerp(hpContent.fillAmount, hpFillAmount, Time.deltaTime * lerpSpeed);

        }

        if(lerpColors)
        {
            hpContent.color = Color.Lerp(lowColor, fullColor, hpFillAmount);
        }
    }

    private float Map(float value, float inMin, float inMax, float outMin, float outMax)
    {
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }

    //private void HandleHp()
    //{
    //    imsi = (float)curHp / (float)maxHp;
    //    hpBar.value = Mathf.Lerp(hpBar.value, imsi, Time.deltaTime * 10);
    //    //hpBar.value = (float)curHp / (float)maxHp;
    //    //hpBar.value = Mathf.Lerp(hpBar.value, (float)curHp / (float)maxHp, Time.deltaTime * 10);
    //}

    //private void ReduceHP()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        if (curHp > 0) { curHp -= 10; }
    //        else { curHp = 0; }
    //    }
    //}


    private void ChargeSpellGauge()
    {
        if (Input.GetKey(KeyCode.X) && circularSpellGauge.fillAmount <= canChargeMaxValue)
        {
            currentChargeValue += gaugeChargeSpeed * Time.deltaTime;
            // (예정)
            // 획득한 MobCount가 30, 50, 100 이상이면 각각
            // canChargeMaxValue = 0.5, 0.75, 1 할당(Switch문)
        }
        else
        {
            currentChargeValue -= gaugeChargeSpeed * Time.deltaTime;
        }

        circularSpellGauge.fillAmount = currentChargeValue / 100;
        //spellGaugeLevel_1
        // 만약 GetKeyUp ,, 키 눌렀다가 뗌 &&  몹 카운트 조건 만족 시 스킬 발동
        // 
    }

}
