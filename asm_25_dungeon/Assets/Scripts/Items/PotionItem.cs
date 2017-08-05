using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionItem : Item
{
    public override void OnPickUp()
    {
        Player.ActivePlayer.ResetHealth();
        gameObject.SetActive(false);
    }
}
