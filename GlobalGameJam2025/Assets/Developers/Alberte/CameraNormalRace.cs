using System.Collections.Generic;
using UnityEngine;

public class CameraNormalRace : MonoBehaviour
{

    public GameObject[] jugadores; // Lista de jugadores en el nivel
    public float velocidad = 5f;       // Velocidad con la que la c�mara se mueve hacia adelante
    public float suavizado = 0.1f;     // Suavizado para el movimiento de la c�mara

    private Transform jugadorDelante;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        jugadores = GameObject.FindGameObjectsWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        jugadorDelante = ObtenerJugadorDelante();

        // Si hay un jugador adelante, mueve la c�mara hacia �l
        if (jugadorDelante != null)
        {
            // Mueve la c�mara hacia adelante a la velocidad definida
            Vector3 nuevaPosicion = new Vector3(transform.position.x, transform.position.y, jugadorDelante.position.z -4f);

            // Aplicar suavizado a la transici�n de la c�mara
            transform.position = Vector3.Lerp(transform.position, nuevaPosicion, suavizado * Time.deltaTime);
        }
    }

    Transform ObtenerJugadorDelante()
    {
        Transform jugadorDelante = null;
        float maxZ = float.MinValue;

        // Recorremos todos los jugadores y encontramos el que est� m�s adelante
        foreach (GameObject jugador in jugadores)
        {
            if (jugador.transform.position.z > maxZ)
            {
                maxZ = jugador.transform.position.z;
                jugadorDelante = jugador.transform;
            }
        }

        return jugadorDelante;
    }
}
