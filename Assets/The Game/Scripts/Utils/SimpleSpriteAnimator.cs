using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSpriteAnimator : MonoBehaviour
{
    /// <summary>
    /// Liste der Einzelbilder, die abgespielt werden.
    /// </summary>
    public Sprite[] frames = new Sprite[0];

    /// <summary>
    /// Dauer der gesamten Animation (Abspielzeit frames).
    /// </summary>
    public float duration = 0.5f;

    /// <summary>
    /// Gibt an, dass die Frame-Sequenz endlos wiederholft wird.
    /// </summary>
    public bool loop = true;

    /// <summary>
    /// Wenn true, wird das Objekt am Ende der Aniamtion zerstört.
    /// </summary>
    public bool destroyObject = false;

    /// <summary>
    /// Soundeffekt, der zu Beginn der Animation startet.
    /// </summary>
    public AudioSource sound = null;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void OnEnable()
    {

        StartCoroutine(PlayAni());

    }

    protected IEnumerator PlayAni()
    {

        if (sound != null) sound.Play();

        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        do
        {
            for (int i = 0; i < frames.Length; i++)
            {
                if (!enabled) break;
                sr.sprite = frames[i];
                yield return new WaitForSeconds(duration / frames.Length);
            }
        }
        while (enabled && loop);

        if (destroyObject) Destroy(gameObject);
    }
}
