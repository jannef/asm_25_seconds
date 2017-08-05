using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionItem : Item
{
    bool pickedUp = false;
    public override void PickUp()
    {
        if (pickedUp) { return; }
        Player.ActivePlayer.ResetHealth();
        pickedUp = true;
        gameObject.SetActive(false);
    }
}
