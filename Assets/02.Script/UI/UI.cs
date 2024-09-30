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
    [SerializeField] private Image hpContent;
    private float hpFillAmount;

    [SerializeField] private float lerpSpeed;
    [SerializeField] private Color fullColor;
    [SerializeField] private Color lowColor;
    [SerializeField] private bool lerpColors;

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
    [SerializeField] float gaugeChargeSpeed = 25;

    // 단계별 스킬 프리팹들
    private GameObject skillPrefab;
    [SerializeField] private GameObject skillPrefabLevel1;
    [SerializeField] private GameObject skillPrefabLevel2;
    [SerializeField] private GameObject skillPrefabLevel3;
    [SerializeField] private GameObject skillPrefabLevel4;

    private double canChargeMaxValue = 1.0; // 최대 게이지 값

    private GameObject originalProjectilePrefab; // 원래 projectilePrefab
    private GameObject currentProjectilePrefab; // 현재 사용 중인 projectilePrefab
    public float skillDuration = 5f; // 스킬 지속 시간



    void Start()
    {
        inven = Inventory.instance;
        playerScr = GameObject.FindWithTag("Player").GetComponent<PlayerScr>();

        if (lerpColors)
        {
            hpContent.color = fullColor;
        }

        // 초기화
        originalProjectilePrefab = playerScr.projectilePrefab; // 원래 projectilePrefab 초기화
    }

    void Update()
    {
        HandleHpBar();
        ChargeSpellGauge();


        // 스킬 사용 체크
        if (Input.GetKeyUp(KeyCode.X))
        {
            CastSpell();
        }
    }

    void FixedUpdate()
    {
        pickupMobCountText.text = string.Format("{0}", inven.pickupMobCount);
    }

    void HandleHpBar()
    {
        //content.fillAmount = hpFillAmount;
        //hpContent.fillAmount = Map(100, 0, 100, 0, 1);
        if (hpFillAmount != hpContent.fillAmount)
        {
            //hpContent.fillAmount = hpFillAmount;
            hpContent.fillAmount = Mathf.Lerp(hpContent.fillAmount, hpFillAmount, Time.deltaTime * lerpSpeed);

        }

        if (lerpColors)
        {
            hpContent.color = Color.Lerp(lowColor, fullColor, hpFillAmount);
        }
    }

    private float Map(float value, float inMin, float inMax, float outMin, float outMax)
    {
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }

    private void ChargeSpellGauge()
    {
        if (Input.GetKey(KeyCode.X) && circularSpellGauge.fillAmount <= canChargeMaxValue)
        {
            currentChargeValue += gaugeChargeSpeed * Time.deltaTime;
        }
        else
        {
            currentChargeValue -= gaugeChargeSpeed * Time.deltaTime;
        }

        circularSpellGauge.fillAmount = currentChargeValue / 100;
    }


    private void CastSpell()
    {
        skillPrefab = null;

        if(circularSpellGauge.fillAmount >= 0.25f && circularSpellGauge.fillAmount < 0.5f)
        {
            // 1단계 스킬
            skillPrefab = skillPrefabLevel1;
        }
        else if (circularSpellGauge.fillAmount >= 0.5f && circularSpellGauge.fillAmount < 0.75f)
        {
            // 2단계 스킬
            skillPrefab = skillPrefabLevel2;
        }
        else if (circularSpellGauge.fillAmount >= 0.75f && circularSpellGauge.fillAmount < 0.1f)
        {
            // 3단계 스킬
            skillPrefab = skillPrefabLevel3;
        }
        else if (circularSpellGauge.fillAmount >= 0.1f)
        {
            // 4단계 스킬 (궁극기)
            skillPrefab = skillPrefabLevel4;
        }

        if(skillPrefab != null)
        {
            // 스킬 발동 후 게이지 초기화
            currentChargeValue = 0;
            circularSpellGauge.fillAmount = 0;

            if (skillPrefab.GetComponent<Projectile>() != null)
            {
                UseProjectileSkill(skillPrefab);
            }
        }

    }
    private void UseProjectileSkill(GameObject newProjectilePrefab)
    {
        // 현재 projectilePrefab을 원래의 것으로 저장
        currentProjectilePrefab = newProjectilePrefab;

        // projectilePrefab을 동적으로 등록된 프리팹으로 변경
        playerScr.projectilePrefab = currentProjectilePrefab;
        // 코루틴을 호출하여 일정 시간 후에 원래의 projectilePrefab으로 되돌리기
        StartCoroutine(ResetProjectile(skillDuration));
    }

    private IEnumerator ResetProjectile(float delay)
    {
        yield return new WaitForSeconds(delay);

        // 원래 projectilePrefab으로 되돌리기
        playerScr.projectilePrefab = originalProjectilePrefab;
    }

}
