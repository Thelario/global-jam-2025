using UnityEngine;

public static class GameSettings 
{
    //Numero de jugadores maximo 
    public const int MAX_PLAYERS = 4;
    //Max Timer por defecto
    public const float MAX_TIMER = 60;
    //Siempre anadir por defecto el teclado que este conectado
    public const bool ALWAYS_CREATE_KEYBOARD = true;
    //True = se van asignando por order de index(amarillo, rojo, azul, etc.)
    //False = se asigna una skin aun no asignada aleatoria
    public const bool ASSIGN_SKINS_IN_ORDER = true;
    //PELIGRO DE MUERTE
    public const int SIMULATE_PLAYERS = 0;
}
