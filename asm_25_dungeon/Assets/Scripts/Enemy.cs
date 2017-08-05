using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CreatureAttributes))]
public class Enemy : MonoBehaviour {
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
    void Start () {
	}
	
    public float GetAttackPower()
    {
        return GetAttr().Attack;
    }

    public void TakeDamage(float AttackPower)
    {
        GetAttr().TakeDamage(AttackPower);
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
