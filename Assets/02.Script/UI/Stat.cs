using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Stat
{
    [SerializeField] private UI playerHP;
    [SerializeField] private BossUI bossHP;

    // Player
    [SerializeField] private float playerMaxVal;
    [SerializeField] private float playerCurrentVal;

    // Boss
    [SerializeField] private float bossMaxVal;
    [SerializeField] private float bossCurrentVal;


    //public float MaxVal { get => maxVal; set => maxVal = value; }

    #region Player HP Stat

    public float PlayerCurrentVal
    {
        get
        {
            return playerCurrentVal;
        }

        set
        {
            this.playerCurrentVal = Mathf.Clamp(value, 0, playerMaxVal);
            playerHP.PlayerValue = playerCurrentVal;
        }

    }

    public float PlayerMaxVal
    {
        get
        {
            return playerMaxVal;
        }
        set
        {
            this.playerMaxVal = value;
            playerHP.PlayerMaxValue = playerMaxVal;
        }
    }

    public void PlayerHPInitialize()
    {
        this.PlayerMaxVal = playerMaxVal;
        this.PlayerCurrentVal = playerCurrentVal;
    }

    #endregion

    #region Boss HP Stat

    public float BossCurrentVal
    {
        get
        {
            return bossCurrentVal;
        }

        set
        {
            this.bossCurrentVal = Mathf.Clamp(value, 0, bossMaxVal);
            bossHP.BossValue = bossCurrentVal;
        }

    }

    public float BossMaxVal
    {
        get
        {
            return bossMaxVal;
        }
        set
        {
            this.bossMaxVal = value;
            bossHP.BossMaxValue = bossMaxVal;
        }
    }

    public void BossHPInitialize()
    {
        this.BossMaxVal = bossMaxVal;
        this.BossCurrentVal = bossCurrentVal;
    }

    #endregion
}
