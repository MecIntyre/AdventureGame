using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawn : MonoBehaviour
{
    /// <summary>
    /// Liste möglicher Spielobjekte, aus denen zufällig eines 
    /// ausgewählt und in der Szene dupliziert wird.
    /// </summary>
    public GameObject[] possibleElements = new GameObject[0];
    
    /// <summary>
    /// Wählt per Zufall ein Element aus der possibleElements-liste aus
    /// und erzeugt eine Kopie/Instanz davon, in der Szene.
    /// </summary>
    /// <returns></returns>
    public GameObject Spawn()
    {
        GameObject template = possibleElements[Random.Range(0, possibleElements.Length)];
        if (template == null) return null;
        else return Instantiate(template);
    }
}
