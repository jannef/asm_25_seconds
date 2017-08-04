using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureAttributes : MonoBehaviour {
    public float Attack = 1.0f;
    [SerializeField]
    protected float Health = 1.0f;
    protected bool dying = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Health <= 0)
        {
            Die();
        }
    }

    public void TakeDamage(float AttackPower)
    {
        Health -= AttackPower;

        if (Health <= 0)
        {
            Die();
        }
    }
    public void SetHealth(float value)
    {
        Health = value;
    }
    public float GetHealth()
    {
        return Health;
    }
    public void Die()
    {
        if (dying)
        {
            return;
        }
        dying = true;
        SendMessage("OnDeath");
    }
}
