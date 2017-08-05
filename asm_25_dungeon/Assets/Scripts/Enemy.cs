using System.Collections;
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
        EnemyLevel = PlayerAttributes.Level;
    }
    // Use this for initialization
    void Start ()
    {
        GetAttr().SetHealth(1+((EnemyLevel-1)*2-1));

    }
	
    public float GetAttackPower()
    {
        return GetAttr().Attack+EnemyLevel;
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
