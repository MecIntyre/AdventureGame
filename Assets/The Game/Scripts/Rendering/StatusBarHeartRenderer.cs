using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusBarHeartRenderer : MonoBehaviour
{
    /// <summary>
    /// Ist die gesundheit höher oder gleich diesem
    /// wert, wird das Herz gefüllt angezeigt.
    /// </summary>
    public int value = 5;

    /// <summary>
    /// Zeiger auf die Bildkomponente, die umgefärbt
    /// bzw. ausgeblendet wird.
    /// </summary>
    private Image image;

        private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void Update()
    {
        if (SaveGameData.current.health.current >= value)
            image.color = Color.white;
        else
            image.color = Color.black;
    }
}
