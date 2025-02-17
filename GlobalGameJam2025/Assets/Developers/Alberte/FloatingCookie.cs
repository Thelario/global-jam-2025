using UnityEngine;

public class FloatingCookie : MonoBehaviour
{
    public Transform[] players; // Lista de jugadores
    public float maxRotation = 30f;
    private Vector3 rotacionInicial;// Rotación máxima permitida en grados
    private Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 rotacionActual = rb.rotation.eulerAngles;

        // Limitar cada componente de la rotación en torno a la rotación inicial
        rotacionActual.x = Mathf.Clamp(rotacionActual.x, -maxRotation, maxRotation);
        rotacionActual.y = Mathf.Clamp(rotacionActual.y, -maxRotation, maxRotation);
        rotacionActual.z = Mathf.Clamp(rotacionActual.z, -maxRotation, maxRotation);

        // Asignar la rotación limitando
        rb.MoveRotation(Quaternion.Euler(rotacionActual));
    }

    void AplicarRotacion()
    {
        Vector3 centroDeMasa = CalcularCentroDeMasa();
        Vector3 direccionDeFuerza = centroDeMasa - transform.position;

        // Crear una fuerza en la dirección del centro de masa calculado
        float fuerza = direccionDeFuerza.magnitude;

        // Rotar la plataforma de acuerdo con esa fuerza pero limitando la rotación
        Vector3 rotacion = new Vector3(Mathf.Clamp(direccionDeFuerza.y, -maxRotation, maxRotation),
                                       Mathf.Clamp(direccionDeFuerza.x, -maxRotation, maxRotation),
                                       0);

        rb.MoveRotation(Quaternion.Euler(rotacion));
    }

    Vector3 CalcularCentroDeMasa()
    {
        Vector3 centro = Vector3.zero;
        foreach (var jugador in players)
        {
            centro += jugador.position; // Sumar las posiciones de todos los jugadores
        }
        centro /= players.Length; // Promedio de las posiciones de los jugadores
        return centro;
    }
}
