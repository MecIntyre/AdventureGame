using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Speicherstruktur für Zähler und aufnehmbare Objekte.
/// </summary>
[System.Serializable]
public class Inventory
{
    /// <summary>
    /// Anzahl der Kristalle im Inventar.
    /// </summary>
    public int gems = 0;

    /// <summary>
    /// Trägt der Charakter den Schild?
    /// </summary>
    public bool shield = false;

    /// <summary>
    /// Trägt der Charakter das Schwert?
    /// </summary>
    public bool sword = false;
}
