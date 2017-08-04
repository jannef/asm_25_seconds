using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionItem : Item
{
    public override void PickUp()
    {
        Player.ActivePlayer.ResetHealth();
        gameObject.SetActive(false);
    }
}
