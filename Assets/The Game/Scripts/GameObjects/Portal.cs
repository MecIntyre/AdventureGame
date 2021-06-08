using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Lädt ein anderes Level, wenn der Held auf das Objekt läuft.
/// </summary>
public class Portal : Collectible
{
    /// <summary>
    /// Name der zu ladenden Szene.
    /// </summary>
    public string sceneName;
    public override void OnCollect()
    {
        base.OnCollect();
        SceneManager.LoadScene(sceneName);
    }
}
