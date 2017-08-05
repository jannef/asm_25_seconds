using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CreatureAttributes))]
public class Enemy : MonoBehaviour {
    public int EnemyLevel = 1;
    public int XpYield = 1;
    float[] _hpValues = { 1, 1, 1, 2, 2, 2, 3, 3, 3, 4, 4 };
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
        if(EnemyLevel >= _hpValues.Length)
        {
            EnemyLevel = _hpValues.Length-1;
        }
        float startHp = _hpValues[EnemyLevel];
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
        var player = FindObjectOfType<SoundPlayer>();
        if (player != null)
        {
            Debug.Log("Called sound!!");
            player.Source.PlayOneShot(player.EnemyDies);
        }

        Player.ActivePlayer.GainXP(XpYield);
        Destroy(gameObject);
    }
}
