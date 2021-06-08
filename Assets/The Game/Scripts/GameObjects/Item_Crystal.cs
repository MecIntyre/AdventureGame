using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script für einsammelbare Kristalle
/// </summary>
public class Item_Crystal : Collectible
{
    public void Start()
    {
        SaveGameData.current.RecoverDestroy(gameObject);
    }

    public override void OnCollect()
    {
        base.OnCollect();
        Debug.Log("Kristall eingesammelt!");
        SaveGameData.current.inventory.gems += 1;
        SaveGameData.current.RecordDestroy(gameObject); //Destroy(gameObject);
    }
}