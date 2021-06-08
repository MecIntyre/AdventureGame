using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Erkennt die Überschneidung von Collidern
/// ohne Physikengine (d.h. ohne Rigidbody) und ruft einen
/// registrierten Callback bei Kollision auf.
/// </summary>
public class CollisionDetector : MonoBehaviour
{
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
	/// Gibt an, wie die Funtkion aussehen muss, 
	/// die bei Kollision aufgerufen wird.
	/// </summary>
	/// <param name="collider"></param>
	public delegate void Callback(Collider2D collider);

	/// <summary>
	/// Speicherplatz für eine Funktion (nach dem Muster des "delegate Callback"),
	/// die aufgerufen wird, wenn die Kollision stattfindet.
	/// </summary>
	public Callback whenCollisionDetected;

	protected virtual void Awake()
	{
		boxCollider = GetComponent<BoxCollider2D>();
		colliders = new Collider2D[10];
		obstacleFilter = new ContactFilter2D();
	}
	/// <summary>
	/// Anzahl der bei der letzten Kollisionsprüfung gefundenen
	/// Kollisionspartner (=Anzahl der Sucheregebnisse in colliders[]).
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

    protected void Update()
    {
		if(whenCollisionDetected==null)
        {
			Debug.LogWarning("CollisionDetector funktioniert nicht, weil whenCollisionDetected nicht zugewiesen wurde.");
			enabled = false;
        }
        else if (IsColliding())
        {
			for(int i = 0; i < numFound; i++)
			whenCollisionDetected(colliders[i]);
        }
    }
}
