using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttributes : CreatureAttributes {
    const int InitialXpToNext = 10;
    public static int Level = 1;
    public int XP = 0;
    public int XpToNextLevel = InitialXpToNext;
    [Header("Balance")]
    public int MaxLevel = 6;
    public int[] XpVals =     {10,15,20,25,30,35};
    public float[] HPVals =     {25,25,25,25,25,25};
    public float[] ATKVals =    {1,2,3,4,5,6};

    [Header("UI")]
    public bool EnableUI = true;
    public Text LevelValue;
    public Text HPValue;
    public Text AtkValue;
    public Image XPBar;

    private void Start()
    {
        ResetAttributes();
        if (EnableUI)
        {
            GameObject lvl_obj = GameObject.FindWithTag("lvl_value");
            if (lvl_obj != null)
            {

                LevelValue = lvl_obj.GetComponent<Text>();
            }

            GameObject hp_obj = GameObject.FindWithTag("hp_value");
            if (hp_obj != null)
            {
                HPValue = hp_obj.GetComponent<Text>();
            }

            GameObject atk_obj = GameObject.FindWithTag("atk_value");
            if (atk_obj != null)
            {
                AtkValue = atk_obj.GetComponent<Text>();
            }
            GameObject xpbar_obj = GameObject.FindWithTag("xp_bar");
            if (xpbar_obj != null)
            {
                XPBar = xpbar_obj.GetComponent<Image>();
            }
        }
    }
    internal void GainXP(int amount)
    {
        // TODO: Increase max time by 5 per levelup
        XP += amount;
        while(XP >= XpToNextLevel)
        {
            Level++;
            if(Level > MaxLevel)
            {
                Level = MaxLevel;
            }
            XP = XP - XpToNextLevel;
            //XpToNextLevel = Level * 5;
            XpToNextLevel = XpVals[Level-1];
            Attack = ATKVals[Level-1];
            Debug.LogFormat("<color='teal'>Player leveled up to " + Level + "!</color>");
        }
    }

    public float GetPlayerAttack()
    {
        return Level*2;
    }

    private void Update()
    {
        if (EnableUI)
        {
            LevelValue.text = Level.ToString();
            HPValue.text = Health.ToString("#.0");
            AtkValue.text = GetPlayerAttack().ToString();
            XPBar.fillAmount = (float)XP / XpToNextLevel;
        }
    }
    public void ResetAttributes()
    {
        Level = 1;
        XpToNextLevel = XpVals[Level - 1];
        Attack = ATKVals[Level - 1];
        XpToNextLevel = InitialXpToNext;
        XP = 0;
    }
}
