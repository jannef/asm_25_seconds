using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyItem : Item
{
    bool pickedUp = false;
    public override void OnPickUp()
    {
        MapManager.Instance.UnlockExit(true);
        gameObject.SetActive(false);
    }
}
