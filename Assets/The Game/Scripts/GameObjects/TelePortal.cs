using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Einfaches Teleportationsportal das zu einem anderen Portal innerhalb der selben Szene führt. 
/// Demonstriert einen Weg, der verhindert, dass das Zielportal sofort wieder auslöst.
/// 
/// "Installation"/Integration: 
/// 
/// - In Hero:Update jeweils BeforeCollectUpdate und AfterCollectUpdate aufrufen:
/// 
/// private void Update()
/// {
///     foreach (TelePortal portal in TelePortal.portale) portal.BeforeCollectUpdate(); // NEU
///     int found = boxCollider.OverlapCollider(triggerContactFilter, colliders);
///     for (int i = 0; i<found; i++)
///     ...
///               collectible.OnCollect();
///     ...
///
///     }
///     foreach (TelePortal portal in TelePortal.portale) portal.AfterCollectUpdate(); // NEU
///     
///        if (SaveGameData.current.inventory.shield)
///            anim.runtimeAnimatorController = shieldSkin;
///     ...
///     
/// 
/// Verwendung: 
/// - Einfaches Spielobjekt ähnlich Flagge (SpriteRenderer+BoxCollider2D)
/// - Teleportal-Script einfügen
/// - Ziel im Inspektor auf 2. Portal setzen
/// </summary>
public class TelePortal : Collectible
{
    /// <summary>
    /// Zwischenspeicher für alle aktiven Portale, 
    /// damit wir nicht in jedem Update per findObjectOfType() 
    /// suchen müssen, was den Code verlangsamen würde. 
    /// </summary>
    public static List<TelePortal> portale = new List<TelePortal>();

    /// <summary>
    /// Wenn dieses Script aktiv wird, in die Liste eintragen.
    /// </summary>
    private void OnEnable()
    {
        portale.Add(this);
    }

    /// <summary>
    /// Wenn dieses Script inaktiv wird, aus der Liste austragen.
    /// </summary>
    private void OnDisable()
    {
        portale.Remove(this);
    }

    //----

    /// <summary>
    /// Im Inspektor auf das Portal setzen, zu dem
    /// dieses Portal hin teleportiert.
    /// </summary>
    public TelePortal ziel;


    /// <summary>
    /// Zwischenspeicher für das momentane Teleportationsziel.
    /// 
    /// Wird ein Teleporter betreten, wird das Ziel vermerkt.
    /// Steht eine Figur auf diesem Ziel, wird es als inaktiv 
    /// behandelt, um nicht sofort wieder zu teleportieren.
    /// Wird das aktive Ziel verlassen, wird aktivesZiel gelöscht
    /// womit das Portal beim nächsten betreten wieder auslöst.
    /// </summary>
    private static TelePortal aktivesZiel = null;

    //----

    /// <summary>
    /// Zwischenspeicher, der vermerkt, ob eine Collision mit der Figur 
    /// in diesem Update-Zyklus festgestellt wurde. Nötig, um später
    /// feststellen zu können, ob eine Kollision NICHT stattfand.
    /// </summary>
    private bool isCollectedInThisFrame = false;

    /// <summary>
    /// Statischer Helfer, der vermerkt, ob in diesem Update-Zyklus
    /// bereits irgendein Portal ausgelöst wurde. Nötig, um zu verhindern, 
    /// dass AfterCollectUpdate des Sprungziels sofort wieder auslöst, 
    /// fall das Script des Ziels nach dem Script des Portals abgearbeitet 
    /// wird.
    /// </summary>
    private static bool zielfindungFertig = false;

    /// <summary>
    /// Muss von Hero.Update vor der OverlapCollider-Schleife aufgerufen werden.
    /// Setzt die Zustandsvariablen für diesen Update-Zyklus zurück.
    /// </summary>
    public void BeforeCollectUpdate()
    {
        isCollectedInThisFrame = false; //für diesen Update-Zyklus zunächst zurück setzen
        zielfindungFertig = false; //in diesem Update-Zyklus hat noch kein Portal ein neues Sprungziel ausgelöst
    }

    /// <summary>
    /// Die Kollisionsfunktion, die jetzt aber erst nur vermerkt, 
    /// dass eine Kollision statt gefunden hat. Wird das nicht aufgerufen,
    /// bleibt isCollectedInThisFrame auf dem von BeforeUpdate() gesetzten false-Zustand
    /// wodurch Keine-Kollision festgestellt werden kann. 
    /// </summary>
    public override void OnCollect()
    {
        base.OnCollect();
        isCollectedInThisFrame = true; //wenn Collision in diesem Zyklus statt findet, merken
    }

    /// <summary>
    /// Muss von Hero.Update nach der OnCollect-Schleife aufgerufen werden.
    /// 
    /// Wir wissen jetzt hier in jedem Update-Zyklus, ob die Figur auf dem Collider steht oder nicht.
    /// 
    /// Hier wird ggf. ein Portal-Ziel ermittelt oder zurückgesetzt, was dem Exit-Ereignis 
    /// entspricht. Zudem wird nun die Teleportation durchgeführt.
    /// </summary>
    public void AfterCollectUpdate()
    {
        if (zielfindungFertig) return; // In diesem Update-Zyklus wurde bereits ein Ziel festgelegt. Verarbeitung abbrechen, da sonst evtl. das neue Ziel sofort ausgelöst wird.
                
        //teleportiere:
        if (aktivesZiel==this) // Dies ist das derzeit aktive Ziel -> nichts tun bis das Portal einmal verlassen wurde.
        {
            if (!isCollectedInThisFrame) aktivesZiel = null; // Ziel wurde verlassen -> Sperre aufheben
        }
        else if (isCollectedInThisFrame) // keine Sperre aktiv (oder Kontakt mit nicht-gesperrtem Portal) -> Teleportation durchführen
        {
            aktivesZiel = ziel; //1. neues ziel merken

            Hero h = FindObjectOfType<Hero>();
            h.gameObject.transform.position = ziel.transform.position + new Vector3(0.5f, -0.5f); //2. figur setzen, je nach mittelpunkt-logik, ggf. um 50% versetzen
            zielfindungFertig = true; // In diesem Update kein anderes AfterCollectUpdate mehr verarbeiten.
        }
    }

}
