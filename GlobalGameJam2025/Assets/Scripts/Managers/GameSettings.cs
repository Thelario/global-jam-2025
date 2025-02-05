using UnityEngine;

public static class GameSettings 
{
    #region LOGGING
    public static bool LOG_EVENT_BUS = false;
    #endregion

    //Numero de jugadores maximo 
    public const int MAX_PLAYERS = 4;
    //Max Timer por defecto
    public const float MAX_TIMER = 60;
    //Siempre anadir por defecto todos los gamepads conectados al comienzo
    public const bool INITIALIZE_CONTROLLERS = true;
    //True = se van asignando por order de index(amarillo, rojo, azul, etc.)
    //False = se asigna una skin aun no asignada aleatoria
    public const bool ASSIGN_SKINS_IN_ORDER = false;

    //PELIGRO DE MUERTE BRICKEA UNITY Y QUIZAS TU ORDENADOR (no tocar)
    public const int SIMULATE_PLAYERS = 0;
}
