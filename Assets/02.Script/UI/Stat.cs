using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Stat
{
    [SerializeField] private UI playerHP;
    //[SerializeField] private UI bossHP;

    [SerializeField] private float maxVal;
    [SerializeField] private float currentVal;
    //public float MaxVal { get => maxVal; set => maxVal = value; }
    
    public float CurrentVal
    {
        get
        {
            return currentVal;
        }

        set
        {
            //this.currentVal = value;
            this.currentVal = Mathf.Clamp(value, 0, MaxVal);
            playerHP.Value = currentVal;
            //bossHP.Value = currentVal;
        }

    }

    public float MaxVal
    {
        get
        {
            return maxVal;
        }
        set
        {
            this.maxVal = value;
            playerHP.MaxValue = maxVal;
            //bossHP.MaxValue = maxVal;


        }
    }

    public void Initialize()
    {
        this.MaxVal = maxVal;
        this.CurrentVal = currentVal;
    }
}
