using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Helden-spezifische Funktionen.
/// </summary>
public class Hero : TheGameObject
{
    private ContactFilter2D triggerContactFilter;

    /// <summary>
    /// Sprites für den Charakter ohne Ausrüstung.
    /// </summary>
    public RuntimeAnimatorController emptySkin;

    /// <summary>
    /// Sprites für den Charakter mit angelegtem Schild.
    /// </summary>
    public RuntimeAnimatorController shieldSkin;

    /// <summary>
    /// Sound, der beim Tod abgespielt eird.
    /// DIeser Sound ignoriert die AudioListener-Pause.
    /// </summary>
    public AudioSource deathSound;

    protected override void Awake()
    {
        base.Awake();
        triggerContactFilter = new ContactFilter2D();
        triggerContactFilter.useTriggers = true; //Trigger-Collider auch erkennen!
        deathSound.ignoreListenerPause = true;
    }
      
    private void Update()
    {
        foreach (TelePortal portal in TelePortal.portale) portal.BeforeCollectUpdate();
        int found = boxCollider.OverlapCollider(triggerContactFilter, colliders);
        for (int i = 0; i < found; i++)
        {
            Collider2D foundCollider = colliders[i];
            if (foundCollider.isTrigger)
            {
                foreach (Collectible collectible in foundCollider.GetComponents<Collectible>())
                {
                    collectible.OnCollect();
                }
            }
        }
        foreach (TelePortal portal in TelePortal.portale) portal.AfterCollectUpdate();

        if (SaveGameData.current.inventory.shield)
            anim.runtimeAnimatorController = shieldSkin;
        else
            anim.runtimeAnimatorController = emptySkin;

        if (SaveGameData.current.health.current == 0)
        {
            anim.SetTrigger("die");
            anim.updateMode = AnimatorUpdateMode.UnscaledTime; //Hero-Animator soll time.timeScale ignorieren, damit die sterbe-Animation lauft.
            GetComponent<HeroInputController>().enabled = false;

            Time.timeScale = 0f; //Spiel pausieren
            AudioListener.pause = true;
        }
    }

    public void OnDeathAnimationComplete()
    {
        DialogsRenderer dr = FindObjectOfType<DialogsRenderer>();
        dr.gameOverDialog.SetActive(true);
    }

    /// <summary>
    ///Helfer-Struktur, um eine Reihe von Bildern
    ///für die Figur zu hinterlegen.
    /// </summary>
    [System.Serializable]
    public class SpriteSet
    {
        public Sprite down;
        public Sprite left;
        public Sprite up;
        public Sprite right; //zusätzlich gespiegelt

        /// <summary>
        /// Weist dem Sprite-Renderer das Sprite zu, das zur Blickrichtung passt.
        /// </summary>
        /// <param name="spriteRenderer">Sprite Renderer</param>
        /// <param name="lookAt">Blickrichtung, entspricht dem Animator-Paramter lookAt.</param>
        public void Apply(SpriteRenderer spriteRenderer, int lookAt)
        {
            spriteRenderer.flipX = false;
            if (lookAt == 0)
                spriteRenderer.sprite = down;
            else if (lookAt == 1)
                spriteRenderer.sprite = left;
            else if (lookAt == 2)
                spriteRenderer.sprite = up;
            else if (lookAt == 3)
            {
                spriteRenderer.sprite = right;
                spriteRenderer.flipX = true;
            }
        }
    }


    /// <summary>
    /// Satz der Bilder, die die Schlagaktionen ohne Schild visualisieren.
    /// </summary>
    public SpriteSet emptyActionSkin;
    /// <summary>
    /// Satz der Bilder, die die Schlagaktionen mit Schild visualisieren.
    /// </summary>
    public SpriteSet shieldActionSkin;


    /// <summary>
    /// Reaktion auf Aktionstastendruck
    /// </summary>
    public void PerformAction()
    {
        if (!SaveGameData.current.inventory.sword) return; //Kein Schwert = Kein Schlag

        anim.enabled = false;

        AnimationEventDelegate.WhenTimelineEventReached += ResetSkin;

        if (SaveGameData.current.inventory.shield) shieldActionSkin.Apply(GetComponent<SpriteRenderer>(), Mathf.RoundToInt(anim.GetFloat("lookAt")));
        else emptyActionSkin.Apply(GetComponent<SpriteRenderer>(), Mathf.RoundToInt(anim.GetFloat("lookAt")));

        Sword sword = GetComponentInChildren<Sword>();
        sword.Stroke();
    }

    private void ResetSkin()
    {
        anim.enabled = true;
        AnimationEventDelegate.WhenTimelineEventReached -= ResetSkin;
    }

    /// <summary>
    /// Sound, der abgespielt wird, wenn der Spieler verletzt wird.
    /// </summary>
    public AudioSource hitSound;

    public override void Flicker(int times)
    {
        hitSound.Play();

        base.Flicker(times);
    }

}
