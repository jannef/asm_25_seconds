using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyItem : Item
{
    public override void PickUp()
    {
        MapManager.Instance.UnlockExit(true);
        gameObject.SetActive(false);
    }
}
