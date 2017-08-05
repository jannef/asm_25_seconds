using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyItem : Item
{
    bool pickedUp = false;
    public override void PickUp()
    {
        if (pickedUp) { return; }
        MapManager.Instance.UnlockExit(true);
        pickedUp = true;
        gameObject.SetActive(false);
    }
}
