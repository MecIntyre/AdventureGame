using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Basis-Script für grundsätzliches Feindverhalten.
/// Zerstörbarkeit mit dem Schwert.
/// </summary>
public class Enemy : MonoBehaviour
{
    /// <summary>
    /// Prefab der Explosion, das bei Tod des Feindes, in der Szene
    /// instanziert wird.
    /// </summary>
    public GameObject explosionPrototype;

    public void Start()
    {
        SaveGameData.current.RecoverDestroy(gameObject);
    }

    /// <summary>
    /// Zerstört den Feind, wenn er vom Schwert getroffen wird.
    /// (Wird aufgerufen von Sword.onCollisionDetected.)
    /// </summary>
    public void OnHitBySword()
    {

        RandomSpawn rs = GetComponent<RandomSpawn>();
        if (rs != null)
        {
            GameObject item = rs.Spawn(); //Zufällig gespawnter Schatz.
            if (item != null)
            {
                item.transform.position = transform.position;
            }
        }

        GameObject explosion = Instantiate(explosionPrototype, transform.parent);
        explosion.transform.position = transform.position;

        SaveGameData.current.RecordDestroy(gameObject); //Destroy(gameObject);
    }
}
