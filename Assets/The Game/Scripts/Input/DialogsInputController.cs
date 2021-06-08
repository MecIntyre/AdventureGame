using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Steuert Eingabesignale, die von Dialogen ausgewertet werden sollen.
/// </summary>
public class DialogsInputController : MonoBehaviour
{
    private DialogsRenderer dialogsRenderer;

    protected void Awake()
    {
        dialogsRenderer = GetComponent<DialogsRenderer>();
    }

    /// <summary>
    /// Speichert, ob die menü-Taste im vorherigen update
    /// schon gedrückt war.
    /// </summary>
    private bool menuButtonDownBefore = false;

    protected void Update ()
    {
        if (dialogsRenderer.gameOverDialog.activeInHierarchy)  //Game-Over zustand.
        {
            if (Input.anyKey)
            {
                SaveGameData.current = new SaveGameData();
                Time.timeScale = 1f;
                AudioListener.pause = false;
                SceneManager.LoadScene("StartMenu");
            }
        }
        else //spiel läuft noch...
        {
            if (Input.GetAxisRaw("Menu") > 0f)
            {
                if (!menuButtonDownBefore)
                    dialogsRenderer.TogglePause();

                menuButtonDownBefore = true;
            }
            else
                menuButtonDownBefore = false;
        }
    }
}
