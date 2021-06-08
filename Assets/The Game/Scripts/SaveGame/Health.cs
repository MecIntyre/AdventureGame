using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Drückt den aktuellen Gesundheitszustand der Spielfigur aus.
/// </summary>
[System.Serializable]
public class Health
{
    /// <summary>
    /// Aktueller Gesundheitswert. Sinkt er auf 0, stirbt die Spielfigur.
    /// </summary>
    public int current = 5;

    /// <summary>
    /// Höchst möglicher Gesundheitswert
    /// </summary>
    public int max = 5;

    /// <summary>
    /// Fügt Gesundheitspunkte hinzu und stellt dabei sicher,
    /// dass die Wertgrenzen (0..max) eingehalten werden.
    /// </summary>
    /// <param name="delta">Differenz, die hinzugefügt oder abgezogen werden soll.</param>
    public void Change(int delta)
    {
        current = Mathf.Clamp(current + delta,0,max);

        Debug.Log("Health ist jetzt" + current + " von " + max);
    }
}
