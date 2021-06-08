using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Visualisert den aktuellen Spielstand in der Statusleiste.
/// </summary>
public class StatusBarRenderer : MonoBehaviour
{
    /// <summary>
    /// Die TextMeshPro-Komponente, die den Kristallzähler zeichnet.
    /// </summary>
    public TextMeshProUGUI gemLabel;

    /// <summary>
    /// Das Bild, das die Waffe auf Position A in der Statusleiste zeichnet.
    /// </summary>
    public Image weaponA_renderer;

    /// <summary>
    /// Das Bild, das die Waffe auf Position B in der Statusleiste zeichnet.
    /// </summary>
    public Image weaponB_renderer;

    // Update is called once per frame
    void Update()
    {
        gemLabel.text = SaveGameData.current.inventory.gems.ToString("D3");
        weaponA_renderer.gameObject.SetActive(SaveGameData.current.inventory.shield);
        weaponB_renderer.gameObject.SetActive(SaveGameData.current.inventory.sword);
    }
}
