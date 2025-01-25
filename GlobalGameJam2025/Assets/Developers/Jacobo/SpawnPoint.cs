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

    Vector3 m_spawnPosition;
    public Vector3 SpawnPosition => m_spawnPosition;
    private static List<SpawnPoint> spawnPoint;
    
    private void Awake()
    {
        if(spawnPoint == null) spawnPoint = new List<SpawnPoint>();
        m_spawnPosition = transform.position;
        spawnPoint.Add(this);
    }
    public static List<SpawnPoint> GetSpawnPoints() => spawnPoint;//Devuelve todos los spawnpoints(no ordenado)
}
