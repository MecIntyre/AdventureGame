using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Basisklasse für alle Blocker, die bei Berührung benachrichtigt 
/// werden sollen.
/// </summary>
public class TouchableBlocker : MonoBehaviour
{
    /// <summary>
    /// Wird aufgerufe, wenn das Objekt aktiviert oder eiongesammelt
    /// werden soll. Unterklassen sollten diese Methode überschreiben,
    /// um auf den Kontakt zu reagieren, ähnlich wie bei OnTrigger-Ereignissen.
    /// </summary>

    public virtual void OnTouch()
    {
        //leer
    }
}
