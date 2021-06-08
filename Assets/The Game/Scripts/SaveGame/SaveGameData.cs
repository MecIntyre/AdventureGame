using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/// <summary>
/// Datenspeicher für den aktuellen Spielstand.
/// </summary>
public class SaveGameData
{
    /// <summary>
    /// Der aktuelle Spielstand.
    /// </summary>
    public static SaveGameData current = LoadOrNew();
    /// <summary>
    /// Speicher für einsammelbare, mitgeführte Gegenstände.
    /// </summary>
    public Inventory inventory = new Inventory();

    /// <summary>
    /// Aktueller Gesundheitszustand des Spielers.
    /// </summary>
    public Health health = new Health();

    /// <summary>
    /// Name/ID des Savepoint-Objekts ("BarberPole") an dem
    /// zuletzt gespeichert wurde und and em Figur beim laden
    /// platziert wird.
    /// </summary>
    public string savepoint = "";

    /// <summary>
    /// Speichert den Spielstand.
    /// </summary>
    public void Save()
    {
        string filepath = "";
        try
        {
            string data = JsonUtility.ToJson(this);
            filepath = Path.Combine(Application.persistentDataPath, "savegame.json");
            
            File.WriteAllText(filepath, data);
            Debug.Log("Gespeichert!\nDatei=" + filepath + "\nDaten=" + data);

            DialogsRenderer dr = UnityEngine.Object.FindObjectOfType<DialogsRenderer>();
            if (dr != null)
                dr.ShowSavedInfo();
        }
        catch(System.Exception ex)
        {
            Debug.LogError("Datei konnte nicht gespeichert werden:\nDatei=" + filepath + "\nFehlermeldung=" + ex.Message + "\nStacktrace:" + ex.StackTrace);
        }

    }

    /// <summary>
    /// Lädt den Spielstand aus dem Savegame (wenn vorhanden)
    /// oder erzeugt einen neuen, leeren Spielstand.
    /// </summary>
    /// <returns>Der geladene oder neue Spielstand</returns>
    private static SaveGameData LoadOrNew()
    {
        SaveGameData result = new SaveGameData();

        string filepath = Path.Combine(Application.persistentDataPath, "savegame.json");
        if (File.Exists(filepath))
        {
            try
            {
                string data = File.ReadAllText(filepath);
                result = JsonUtility.FromJson<SaveGameData>(data);
                Debug.Log("Geladen!\nDatei=" + filepath + "\nDaten=" + data);
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Datei konnte nicht geladen werden:\nDatei="+filepath+"\nFehlermeldung="+ex.Message+"\nStacktrace:"+ex.StackTrace);
            }
        }
        //else: das neue, leere Savegame verwenden.

        return result;
    }

    /// <summary>
    /// Liste der IDs der gelöschten Objekte, siehe recordDestroy().
    /// </summary>
    public List<string> deletedObjects = new List<string>();
    

    /// <summary>
    /// Löscht das angegebene Objekt aus der Szene
    /// und zeichnet dies Vorgang im Savegame auf.
    /// </summary>
    /// <param name="go">Zu löschendes Objekt</param>
    public void RecordDestroy(GameObject go)
    {
        string ID = go.scene.name+"/"+go.name;
        deletedObjects.Add(ID);

        UnityEngine.Object.Destroy(go);
    }

    /// <summary>
    /// Löscht das Spielobjekt, wenn es als gelöscht im
    /// Spielzustand gespeichert ist.
    /// </summary>
    /// <param name="go">Zu löschendes bzw zuuvor evtl. gelöschtes Objekt.</param>
    public void RecoverDestroy(GameObject go)
    {
        string ID = go.scene.name + "/" + go.name;
        if (deletedObjects.Contains(ID)) UnityEngine.Object.Destroy(go);
    }
}
