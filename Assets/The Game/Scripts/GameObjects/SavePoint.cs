using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Läuft der Spieler über dieses Objekt, wird das Spiel gespeichert.
/// </summary>
public class SavePoint : Collectible
{
    public void Start()
    {
        if (SaveGameData.current.savepoint==(gameObject.scene.name + "/" + gameObject.name)) //An diesem Punkt wurde zuletzt gespeichert.
        {
            SetLocked(true);
            FindObjectOfType < Hero >().transform.position = transform.position+new Vector3(0.5f,-0.5f,0f);
        }
    }
    public override void OnCollect()
    {
        base.OnCollect();

        if  (IsLocked()) return;

        SaveGameData.current.savepoint = gameObject.scene.name + "/" + gameObject.name;
        SaveGameData.current.Save();
        SetLocked(true);
    }

    /// <summary>
    /// Zeitpunkt in der Zukunft, an dem das Objekt automatisch entsperrt wird.
    /// </summary>
    private float lockedUntil = 0f;
    
    /// <summary>
    /// Animator der aktiv ist, wenn das Objekt nicht gesperrt ist.
    /// </summary>
    public SimpleSpriteAnimator unlockedSprites;

    /// <summary>
    /// Bild, das gezeigt wird, wenn das Objekt gesperrt ist.
    /// </summary>
    public Sprite lockedSprite;

    /// <summary>
    /// Renderer, der das lockedSprite empfängt.
    /// </summary>
    public SpriteRenderer spriteRenderer;

    /// <summary>
    /// Sperrt das Objekt so, dass das Speichern nicht ausgelöst werden kann.
    /// </summary>
    /// <param name="doLock">Gibt an, ob das Objekt gesperrt (true) oder entsperrt (false) wird. </param>
    private void SetLocked(bool doLock)
    {
        if (doLock != unlockedSprites.enabled) return; //Wichtig: Zustand nur ändern, wenn sich wirklich etwas ändert, sonst endloses neu-setzen des lockedUntil-Timers.
        if(doLock)
        {
            lockedUntil = Time.time + 10f; // Objekt wird in 10 Sekunden wieder entsperrt.
            unlockedSprites.enabled = false; //Animation stoppen.
            spriteRenderer.sprite = lockedSprite; //Stattdesse Sprite für Deaktivierung zeigen.
        }
        else
        {
            unlockedSprites.enabled = true;
        }
    }

    /// <summary>
    /// Ermittelt, ob das Objekt bei Berührung speichert oder ob
    /// das Objekt noch gesperrt ist.
    /// </summary>
    /// <returns><c>true</c>, wenn das Objekt gesperrt ist, d.h. nicht speichert.</returns>
    private bool IsLocked()
    {
        return lockedUntil > Time.time;
    }
    public void Update()
    {
        SetLocked(IsLocked());
    }
}
