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
	// Use this for initialization
	void Start () {
        GetAttr();
	}
	
    public float GetAttackPower()
    {
        return GetAttr().Attack;
    }

    public void TakeDamage(float AttackPower)
    {
        _attr.TakeDamage(AttackPower);
    }

	// Update is called once per frame
	void Update () {
		
	}

    void OnDeath()
    {
        Player.ActivePlayer.GainXP(XpYield);
        Destroy(gameObject);
    }
}
