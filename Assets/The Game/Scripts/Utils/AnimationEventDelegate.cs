using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Helfer, um ein Ereignis der Zeitleiste an ein beliebiges
/// Script weiterzuleiten.
/// </summary>
public class AnimationEventDelegate : MonoBehaviour
{
    /// <summary>
    /// Definiert, wie Funktionen aufgebaut sein müssen, damit sie in die
    /// Liste whenTimelineEventReached aufgenommen werden können.
    /// </summary>
    public delegate void callback();

    /// <summary>
    /// Eine "Liste" die Funktionen nach dem Bauplan des 'delegate Callback'
    /// enthält. Diese Funktionen werden aufgerufen, wenn das Zeitleistenereignis
    /// eintritt.
    /// </summary>
    public static event callback WhenTimelineEventReached;
    public void OnTimelineEvent()
    {
        WhenTimelineEventReached?.Invoke();
    }
}
