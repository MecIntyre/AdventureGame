using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Heart : Collectible
{
    public void Start()
    {
        SaveGameData.current.RecoverDestroy(gameObject);
    }

    public override void OnCollect()
    {
        base.OnCollect();
        {
            base.OnCollect();
            SaveGameData.current.health.Change(+1);
            SaveGameData.current.RecordDestroy(gameObject); //Destroy(gameObject);
        }
    }
}
