using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Sword : Collectible
{
    public void Start()
    {
        if (SaveGameData.current.inventory.sword) Destroy(gameObject);
    }

    /// <summary>
    /// Einsammelbare Schwert-Ausrüstung, setzt Inventory-sword.
    /// </summary>
    public override void OnCollect()
    {
        base.OnCollect();
        SaveGameData.current.inventory.sword = true;
        Destroy(gameObject);
    }
}
