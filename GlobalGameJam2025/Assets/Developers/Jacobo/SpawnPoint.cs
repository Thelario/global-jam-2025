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
        if(spawnPoint == null) spawnPoint = new List<Vector3>();
        spawnPoint.Add(transform.position);
    }
    private static List<Vector3> spawnPoint;
    public static List<Vector3> GetSpawnPoints() => spawnPoint;//Devuelve todos los spawnpoints(no ordenado)
}
