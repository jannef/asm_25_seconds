using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CreatureAttributes))]
public class TimerHealth : MonoBehaviour {
    public float TimeLimit = 25.0f;
    public float TimeLeft = 25.0f;
    public bool GODMODE = true;
    CreatureAttributes _attr;
	// Use this for initialization
	void Start () {
        _attr = GetComponent<CreatureAttributes>();
        ResetTime();
	}
	
    public void ResetTime()
    {
        //TimeLeft = 25.0f;
        _attr.SetHealth(TimeLimit + (PlayerAttributes.Level-1)*5);

    }

	// Update is called once per frame
	void Update () {
        if (GODMODE){return;}
        //TimeLeft -= Time.deltaTime;
        //_attr.SetHealth(TimeLeft);
        _attr.TakeDamage(Time.deltaTime);
        if (_attr.GetHealth() < 0)
        {
            string deadStr = "<color=red>TIME'S UP, YOU'RE DE";
            for(int i = 0; i < -_attr.GetHealth(); i++)
            {
                deadStr += "E";
            }
            deadStr += "AD!</color>";
            Debug.LogFormat(deadStr);

            if(TimeLeft < -10)
            {
                ResetTime();
            }
        }
	}
}
