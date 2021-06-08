using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Verwaltet Texttafeln, die nur zu bestimmten Zeiten sichtbar sind.
/// </summary>
public class DialogsRenderer : MonoBehaviour
{
    /// <summary>
    /// Zeiger auf die UI-Tafel, die angezeigt wird,
    /// wenn das Spiel vorbei/verloren ist.
    /// </summary>
    public GameObject gameOverDialog;

    /// <summary>
    /// Zeiger auf die "Spiel gesepeichert"-Meldung,
    /// die kurz angezeigt wird, dann wieder verschwindet.
    /// </summary>
    public GameObject savedInfo;

    /// <summary>
    /// Mini-Menü, das zu sehen ist, während sich das Spiel
    /// im Pause-Zustand befindet.
    /// </summary>
    public GameObject pauseInfo;
    
    /// <summary>
    /// Schwarzbild, das zum Abblenden des Hintergrunds verwendet wird.
    /// </summary>
    public GameObject blackness;

    /// <summary>
    /// Sound, der beim Eintritt der Pause abspielt.
    /// </summary>
    public AudioSource pauseStart;
    /// <summary>
    /// Sound, der bei Rückkehr ins Spiel abspielt.
    /// </summary>
    public AudioSource pauseEnd;

    protected void Awake()
    {
        gameOverDialog.SetActive(false);
        savedInfo.SetActive(false);
        pauseInfo.SetActive(false);
        blackness.SetActive(false);
        pauseStart.ignoreListenerPause = true;
    }

    /// <summary>
    /// Blendet das Pause-Menü aus, wenn es sichtbar ist und
    /// blendet es ein, wenn es unsichtbar ist.
    /// </summary>
    public void TogglePause()
    {
        if (pauseInfo.activeSelf) //sichtbar -> ausblenden
        {
            pauseInfo.SetActive(false);
            Time.timeScale = 1f; //Zeit fortsetzen
            pauseEnd.Play();
        }

    else //unsichtbar -> einblenden
        {
            pauseInfo.SetActive(true);
            Time.timeScale = 0f; //Zeit anhalten
            Button button = pauseInfo.GetComponentInChildren<Button>();
            EventSystem.current.SetSelectedGameObject(button.gameObject);
            button.OnSelect(null);
            pauseStart.Play();
        }
        blackness.SetActive(pauseInfo.activeSelf);
        AudioListener.pause = pauseInfo.activeSelf;
    }

    /// <summary>
    /// Kehrt sofort zum Startmenü zurück.
    /// </summary>
    public void OnBackToStartMenu()
    {
        Time.timeScale = 1f; //Zeit fortsetzen
        AudioListener.pause = false;  //Sounds fortsetzen.
        SceneManager.LoadScene("StartMenu");
    }

    /// <summary>
    /// Blendet die "Spiel gespeichert"-Info (savedInfo) ein,
    /// wartet einen Moment und blendet sie wieder aus.
    /// </summary>
    public void ShowSavedInfo()
    {
        StartCoroutine(ShowSavedInfoAndHide());
    }

    private IEnumerator ShowSavedInfoAndHide()
    {
        savedInfo.SetActive(true);
        yield return new WaitForSeconds(2f);
        savedInfo.SetActive(false);
    }


}
