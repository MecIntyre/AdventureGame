using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Realisiert eine Gefahrenquelle, die den Spieler bei Berührung verletzt.
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
public class Danger : TouchableBlocker
{
    /// <summary>
    /// Zeitpunkt der letzten Verletzung.
    /// </summary>
    private float lastHit = 0f;

    /// <summary>
    /// Muss true sein, wenn das O´bject mit der linken, oberen Ecke
    /// an den Kacheln ausgerichtet wird, false, wenn das Objekt
    /// am Mittelpunkt ausgerichtet wird.
    /// </summary>
    public bool topLeftAnchor = true;

    /// <summary>
    /// Wenn gesetzt/true, dann nimmt der Spieler bei Berührung
    /// keinen Schaden, wenn sich der Schild im Inventar befindet.
    /// </summary>
    public bool shieldProtection = false;

    public override void OnTouch()
    {
        base.OnTouch();

        if (Time.time - lastHit > 1f)
        {
            bool isSafe = shieldProtection && SaveGameData.current.inventory.shield;
            if (!isSafe)
                SaveGameData.current.health.Change(-1);

            lastHit = Time.time;

            if (SaveGameData.current.health.current > 0)
            {
                Hero hero = FindObjectOfType<Hero>();
                hero.PushAwayFrom(this, topLeftAnchor);
                if (!isSafe) hero.Flicker(5);

            }
        }
    }
}
