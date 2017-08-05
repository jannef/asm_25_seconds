using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Item : MonoBehaviour
{
    public UnityEvent PickUpEvent;
    protected bool picked = false;

    public void PickUp()
    {
        if (!picked)
        {
            picked = true;
            if (PickUpEvent != null)
            {
                PickUpEvent.Invoke();
            }
            OnPickUp();
        }
    }

    public abstract void OnPickUp();
}
