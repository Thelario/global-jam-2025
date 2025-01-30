using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    /// <summary>
    /// Clase de spawnpoint. Tiene una lista estatica para obtener todos los spawnpoints
    /// y una referencia a la posicion
    /// </summary>
    /// 

    private void Awake()
    {
        transform.parent = null;
        spawnPoints.Add(transform.position);
    }
    private void OnDestroy()
    {
        spawnPoints.Remove(transform.position);
    }
    private static List<Vector3> spawnPoints = new List<Vector3>();
    public static List<Vector3> GetSpawnPoints() => spawnPoints;//Devuelve todos los spawnpoints(no ordenado)
}
