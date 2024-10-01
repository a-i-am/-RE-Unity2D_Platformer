using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEffect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnParticleCollision(GameObject other)
    {
        GameObject collidedObject = other.gameObject;
        // 적과 충돌했을 때
        if (other.gameObject.tag == "Enemy")
        {
            EnemyScr enemy = collidedObject.GetComponent<EnemyScr>();
            if (enemy != null)
            {
                enemy.TakeDamage();
            }
        }
    }


}
