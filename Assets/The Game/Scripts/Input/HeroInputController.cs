using UnityEngine;
using System.Collections;

/// <summary>
/// Wertet Eingaben aus, die die Spielfigur steuert
/// und leitet sie an das Heroscript weiter.
/// </summary>
public class HeroInputController : MonoBehaviour
{
    /// <summary>
    /// Zeiger auf das Helden-Script, das durch die Eingabe
    /// gesteuert wird.
    /// </summary>
    public Hero hero;


    // Update is called once per frame
    private void Update()
    {
        if (Time.timeScale < 1f) return; // Spiel pausiert - keine Eingabeauswertung.

        if (Input.GetAxisRaw("Horizontal") > 0f)
            hero.change.x = 1;
        else if (Input.GetAxisRaw("Horizontal") < 0f)
            hero.change.x = -1;
        else if (Input.GetAxisRaw("Vertical") > 0f)
            hero.change.y = 1;
        else if (Input.GetAxisRaw("Vertical") < 0f)
            hero.change.y = -1;

        else if (Input.GetAxisRaw("Fire1") > 0f)  //Aktionstaste gedrückt
            hero.PerformAction();
    }
}
