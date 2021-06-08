using UnityEngine;
using System.Collections;

/// <summary>
/// Oberste Klasse für Spielobjekte meines PixelArt-Games.
/// Enthält allgemeine Funktionen, die für die meisten Szenenobjekte
/// potenziell nützlich sind.
/// </summary>
public class TheGameObject : MonoBehaviour
{

	/// <summary>
	/// Größe eines PixelArt-Pixels in Unity-Einheiten
	/// </summary>
	private static float pixelFrac = 1f / 16f;  //16 = Pixels per Unit

	/// <summary>
	/// Runde auf PixelArt-pixel.
	/// </summary>
	/// <param name="f">Zahl, die gerundet werden soll.</param>
	/// <returns>f, eingerastet im PixelArt-Raster.</returns>
	protected float RoundToPixelGrid(float f)
	{
		return Mathf.Ceil(f / pixelFrac) * pixelFrac;
	}
	/// <summary>
	/// Zeiger auf den Box-Collider für die Kollisionserkennung
	/// mittels isColliding, um die Suchfunktion (getComponent) einzusparen.
	/// </summary>
	protected BoxCollider2D boxCollider;
	/// <summary>Ergebnis-Zwischenspeicher für Kollisionserkennung
	/// mittels isColliding.
	/// </summary>
	protected Collider2D[] colliders;

	/// <summary>
	/// Der Filter, der Kollisionsobjekte im Sinne von Hindernissen
	/// findet (Trigger werden ignoriert).
	/// </summary>
	protected ContactFilter2D obstacleFilter;

	/// <summary>
	/// Zeiger auf die Animation-Komponente, die die Sprite-Animation
	/// realisiert. Die Laufbewegung wird mit diesem Animator synchronisiert.
	/// </summary>
	protected Animator anim;

	protected virtual void Awake()
	{
		boxCollider = GetComponent<BoxCollider2D>();
		colliders = new Collider2D[10];
		anim = GetComponent<Animator>();
		obstacleFilter = new ContactFilter2D();
	}

	/// <summary>
	/// Anzahl der bei der letzten Kollisionsprüfung gefundenen
	/// Kollisionspartner (=Anzahl der Suchergebnisse in colliders[]).
	/// </summary>
	protected int numFound = 0;

	/// <summary>
	/// Prüft, ob eine Kollision zwischen dem BoxCollider2D, dieses Spielobjekts
	/// und anderen 2D-Kollidern stattfindet.
	/// </summary>
	/// <returns><c>true</c> bei Kollision, <c>false</c> sonst.</returns>
	protected bool IsColliding()
	{
		numFound = boxCollider.OverlapCollider(obstacleFilter, colliders);
		return numFound > 0;
	}
	/// <summary>
	/// Bewegungen, die die Figur in diesem Frame vollziehen soll.
	/// 1 = nach rechts/oben, -1 = nach links/unten.
	/// </summary>
	public Vector3 change = new Vector3();
	private void LateUpdate()
	{
		anim.SetFloat("change_x", change.x);
		anim.SetFloat("change_y", change.y);

		if (change.y <= -1f) anim.SetFloat("lookAt", 0f);
		else if (change.x <= -1f) anim.SetFloat("lookAt", 1f);
		else if (change.y >= 1f) anim.SetFloat("lookAt", 2f);
		else if (change.x >= 1f) anim.SetFloat("lookAt", 3f);

		//Anwenden der in change gesetzten Bewegung:
		float step = RoundToPixelGrid(1f * Time.deltaTime);
		Vector3 oldPos = transform.position;
		transform.position += change * step;
		if (IsColliding())
		{ 
			transform.position = oldPos; //auf alte Position sptingen (d.h. nicht in den Collider laufen).

			for (int i = 0; i < numFound; i++)
            {
				TouchableBlocker tb = colliders[i].GetComponent<TouchableBlocker>();
				if (tb != null) tb.OnTouch();
            }
		}
		change = Vector3.zero;
	}

	/// <summary>
	/// Berechnet den genauen Mittelpunkt der Kachel, 
	/// in der sich die Figur gerade befindet.
	/// Kann verwendet werden, um eine Figur in eine Kachel
	/// "einzurasten".
	/// </summary>
	/// <returns>Mittelpunkt der Kachel</returns>
	public Vector3 GetFullTilePosition()
	{
		Vector3 p = transform.position;
		p.x = Mathf.FloorToInt(p.x);
		p.y = Mathf.CeilToInt(p.y);

		p.x += 0.5f;
		p.y -= 0.5f;

		return p;
	}

	/// <summary>
	/// Schiebt die Figur auf die um deltaX/deltaY Kachel entfernte
	/// Nachbarkachel weiter.
	/// </summary>
	/// <param name="deltaX">Anzahl der Kacheln, um die die Figur horizontal verschoben wird.</param>
	/// <param name="deltaY">Anzahl der Kacheln, um die die Figur vertikal verschoben wird.</param>
	public void PushByTiles(float deltaX, float deltaY)
	{
		Vector3 tilePos = GetFullTilePosition();

		tilePos.x += deltaX;
		tilePos.y += deltaY;

		Vector3 oldPosition = GetFullTilePosition();
		transform.position = tilePos; //Übersprung-endposition
		if (IsColliding()) transform.position = oldPosition;
		else StartCoroutine(AnimateMoveTowards(oldPosition, tilePos));

	}

	/// <summary>
	/// Schiebt die Figur mit einer Animation von einer Position zu einer anderen.
	/// </summary>
	/// <param name="fromPos">Start-Position</param>
	/// <param name="targetPos">Ziel-Position</param>
	/// <returns></returns>
	private IEnumerator AnimateMoveTowards(Vector3 fromPos, Vector3 targetPos)
	{
		float duration = 0.1f; //Dauer der Bewegung.
		for (float t = 0f; t <= 1f; t += Time.deltaTime  /duration)
        {
			transform.position = Vector3.Lerp(fromPos,targetPos,t);
			yield return new WaitForEndOfFrame();
        }

	}

	/// <summary>
	/// Drückt die Figur vom angebeben Objekt wegwärts.
	/// </summary>
	/// <param name="deflector">Das Objekt, von dem die Figur "abprallt".</param>
	/// <param name="topLeftAnchor">Íst das Deflector-Objekt links oben ausgerichtet?
	/// (ansonsten Mittelpunkt.)</param>
	public void PushAwayFrom(MonoBehaviour deflector, bool topLeftAnchor)
    {
		Vector3 diff;
		if (topLeftAnchor) // = Ausrichtung links oben -> Mittelpunkt hier manuell berechnen
			diff = transform.position - (deflector.transform.position + new Vector3(0.5f, -0.5f, 0f));
		else //=Ausrichtung am Mittelpunkt
			diff = transform.position - deflector.transform.position;

		PushByTiles(diff.x, diff.y);
    }

	/// <summary>
	/// Lässt die Figur times mal blinken.
	/// </summary>
	/// <param name="times">Anzahl wie oft die Figur blinken soll.</param>
	public virtual void Flicker(int times)
    {
		StartCoroutine(AnimateFlicker(times));
    }

	/// <summary>
	/// Animiert das Blinken, das mittels flicker() gestartet werden kann.
	/// </summary>
	/// <param name="times">Anzahl der Blink-Wiederholungen.</param>
	/// <returns></returns>
	private IEnumerator AnimateFlicker(int times)
    {
		SpriteRenderer sr = GetComponent<SpriteRenderer>();
		for (int i = 0; i < times; i++)
        {
			sr.color = Color.red;
			yield return new WaitForSeconds(0.05f);
			sr.color = Color.white;
			yield return new WaitForSeconds(0.05f);
		}
    }
}
