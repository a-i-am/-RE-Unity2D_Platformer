using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEAttack : MonoBehaviour
{
    [SerializeField] private SkillType skillType;  // 어떤 스킬인지
    [SerializeField] private float lifeTime;

    private void Start()
    {
        GetLifeTime(skillType, lifeTime);
        HandleSkillEft();
    }
    private void HandleSkillEft()
    {
        switch (skillType)
        {
            case SkillType.Lightning:
                LightningAOE();
                break;
        }
    }

    private void GetLifeTime(SkillType type, float customLifeTime)
    {
        this.lifeTime = customLifeTime;
        Destroy(gameObject, lifeTime);
    }

    private void LightningAOE()
    {
        Destroy(gameObject, lifeTime);
    }

}
