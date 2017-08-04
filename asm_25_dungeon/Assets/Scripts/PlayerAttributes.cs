using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttributes : CreatureAttributes {
    public static int Level = 1;
    public int XP = 0;
    public int XpToNextLevel = 10;
    [Header("UI")]
    public bool EnableUI = true;
    public Text LevelValue;
    public Text HPValue;
    public Text AtkValue;

    private void Start()
    {
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
        }
    }
    internal void GainXP(int amount)
    {
        // TODO: Increase max time by 5 per levelup
        XP += amount;
        while(XP >= XpToNextLevel)
        {
            Level++;
            XP = XP - XpToNextLevel;
            Debug.LogFormat("<color='teal'>Player leveled up to " + Level + "!</color>");
        }
    }

    public float GetPlayerAttack()
    {
        return Level;
    }

    private void Update()
    {
        if (EnableUI)
        {
            LevelValue.text = Level.ToString();
            HPValue.text = Health.ToString("#.0");
            AtkValue.text = GetPlayerAttack().ToString();
        }
    }
}
