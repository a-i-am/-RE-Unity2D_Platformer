using Assets;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    Inventory inven;
    public PlayerScr playerScr;
    [SerializeField] TextMeshProUGUI pickupMobCountText;
    [SerializeField] private Slider hpBar;
    [SerializeField] private Image circularSpellGauge;
    [SerializeField] float currentValue = 0;
    [SerializeField] double canChargeMaxValue = 0.25;
    [SerializeField] float gaugeChargeSpeed = 25;
    private float maxHp = 100;
    private float curHp = 100;
    float imsi;

    void Start()
    {
        inven = Inventory.instance;
        playerScr = GameObject.FindWithTag("Player").GetComponent<PlayerScr>();
        hpBar.value = (float)curHp / (float)maxHp;
    }

    void Update()
    {
        ReduceHP();
        HandleHp();
        ChargeSpellGauge();
    }

    void FixedUpdate()
    {
        pickupMobCountText.text = string.Format("{0}", inven.pickupMobCount);
    }

    private void HandleHp()
    {
        imsi = (float)curHp / (float)maxHp;
        hpBar.value = Mathf.Lerp(hpBar.value, imsi, Time.deltaTime * 10);
        //hpBar.value = (float)curHp / (float)maxHp;
        //hpBar.value = Mathf.Lerp(hpBar.value, (float)curHp / (float)maxHp, Time.deltaTime * 10);
    }

    private void ReduceHP()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (curHp > 0) { curHp -= 10; }
            else { curHp = 0; }
        }
    }


    private void ChargeSpellGauge()
    {
        if (Input.GetKey(KeyCode.X) && circularSpellGauge.fillAmount <= canChargeMaxValue)
        {
            currentValue += gaugeChargeSpeed * Time.deltaTime;
            // (예정)
            // 획득한 MobCount가 30, 50, 100 이상이면 각각
            // canChargeMaxValue = 0.5, 0.75, 1 할당(Switch문)
        }
        else
        {
            currentValue -= gaugeChargeSpeed * Time.deltaTime;
        }

        circularSpellGauge.fillAmount = currentValue / 100;
        //spellGaugeLevel_1
        // 만약 GetKeyUp ,, 키 눌렀다가 뗌 &&  몹 카운트 조건 만족 시 스킬 발동
        // 
    }

}
