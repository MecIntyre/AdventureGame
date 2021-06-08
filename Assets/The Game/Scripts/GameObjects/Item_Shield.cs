using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Einsammelbare Schild-Ausrüstung, setzt Inventory-shield.
/// </summary>
public class Item_Shield : Collectible
{
    public void Start()
    {
        if (SaveGameData.current.inventory.shield) Destroy(gameObject);
    }

    public override void OnCollect()
    {
        base.OnCollect();
        SaveGameData.current.inventory.shield = true;
        Destroy(gameObject);
    }
}
