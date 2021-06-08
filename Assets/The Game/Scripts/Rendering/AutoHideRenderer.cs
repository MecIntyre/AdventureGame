using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Deaktiviert die Renderer-Komponente auf diesem Spielobjekt
/// beim Start.
/// </summary>
public class AutoHideRenderer : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start() 
    {
        Renderer r = GetComponent<Renderer>();
        if (r != null) r.enabled = false;
    }
}
