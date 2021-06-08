using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script fur die Kamera, das die Spielfigur immer im Bild hält.
/// </summary>
public class CameraMotionController : MonoBehaviour
{
    /// <summary>
    /// zeiger auf den Held, den die Kamera im Bild hält.
    /// </summary>
    public Hero hero;

    // Update is callen once per frame
    private void Update ()
    {
        Vector3 heroPos = hero.transform.position; //Position des Helden kopieren.
        heroPos.z = transform.position.z; //Kamera-Z beibehalten.
        transform.position = heroPos;
    }
}