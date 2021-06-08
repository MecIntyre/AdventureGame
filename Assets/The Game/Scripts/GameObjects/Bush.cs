using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bush : MonoBehaviour
{
    /// <summary>
    /// Liste der Einzelbilder, die bei Zerstörung des Busches abgespielt werden.
    /// </summary>
    public Sprite[] destructionFrames = new Sprite[0];

    /// <summary>
    /// Dauer der gesamten Animation (Abspielzeit destructionFrames).
    /// </summary>
    public float duration = 0.5f;

    /// <summary>
    /// Wurde playOnHitBySwordAni schon gestartet?
    /// </summary>
    private bool isHitAniPlaying = false;

    public void Start()
    {
        SaveGameData.current.RecoverDestroy(gameObject);
    }

    /// <summary>
    /// Zerstört den Busch, wenn er vom Schwert getroffen wird.
    /// (Wird aufgerufen von Sword.onCollisionDetected.)
    /// </summary>
    public void OnHitBySword()
    {
        if(!isHitAniPlaying)
            StartCoroutine(PlayOnHitBySwordAni());
    }

    protected IEnumerator PlayOnHitBySwordAni()
    {
        isHitAniPlaying = true;

        RandomSpawn rs = GetComponent<RandomSpawn>();
        if (rs!=null)
        {
            GameObject item = rs.Spawn(); //Zufällig gespawnter Schatz.
            if(item!=null)
            {
                item.transform.position = transform.position;
            }
        }

        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        for (int i = 0; i < destructionFrames.Length; i++)
        {
            sr.sprite = destructionFrames[i];
            yield return new WaitForSeconds(duration / destructionFrames.Length);
        }
        SaveGameData.current.RecordDestroy(gameObject);
        
    }
}
