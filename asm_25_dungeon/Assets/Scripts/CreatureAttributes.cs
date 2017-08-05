using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CreatureAttributes : MonoBehaviour {
    public float Attack = 1.0f;
    public UnityEvent OnDeathEvent;
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

    public bool TakeDamage(float AttackPower)
    {
        Health -= AttackPower;

        if (Health <= 0)
        {
            Die();
            return true;
        }
        return false;
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

        if (OnDeathEvent != null) { OnDeathEvent.Invoke(); }
        SendMessage("OnDeath");
    }
}
