﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CreatureAttributes))]
public class Enemy : MonoBehaviour {
    public int EnemyLevel = 1;
    public int XpYield = 5;
    CreatureAttributes _attr;
    CreatureAttributes GetAttr()
    {
        if(_attr == null)
        {
            _attr = GetComponent<CreatureAttributes>();
        }
        return _attr;
    }
    TextMesh _enemyInfo;
    bool _uiEnabled;

    private void Awake()
    {
        GetAttr();
        _enemyInfo = GetComponentInChildren<TextMesh>();
        _uiEnabled = _enemyInfo != null;
    }
    // Use this for initialization
    void Start ()
    {
        EnemyLevel = MapManager.Instance.LoadedLevel;
        float startHp = 1;//1 + (EnemyLevel - EnemyLevel % 2);
        GetAttr().SetHealth(startHp);

    }
	
    public float GetAttackPower()
    {
        return 1; //GetAttr().Attack+EnemyLevel;
    }

    public bool TakeDamage(float AttackPower)
    {
        return GetAttr().TakeDamage(AttackPower);
    }

	// Update is called once per frame
	void Update () {
        _enemyInfo.text = string.Format("HP:\t{0}\nATK:\t{1}", GetAttr().GetHealth(), GetAttackPower());
		
	}

    void OnDeath()
    {
        Player.ActivePlayer.GainXP(XpYield);
        Destroy(gameObject);
    }
}
