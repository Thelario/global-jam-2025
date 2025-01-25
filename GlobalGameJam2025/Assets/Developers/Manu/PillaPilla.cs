using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PillaPilla : MonoBehaviour
{
    [SerializeField] TMP_Text timer;
    [SerializeField] TMP_Text quienPillaText;
    [SerializeField] float maxTime;
    [SerializeField] float delayStart;
    Vector3 escalaInicial;
    float currentTime;
    List<MultiplayerInstance> players;
    MultiplayerInstance elQuePilla;
    bool endGame = false;
    bool puedePillar = true;

    void Start()
    {
        StartCoroutine(InicioConRetraso());
    }

    IEnumerator InicioConRetraso()
    {
        yield return new WaitForSeconds(delayStart);

        players = null;// GameplayMultiplayerManager.Instance.GetAllPl;
        if (players != null && players.Count > 0)
        {
            int randomIndex = Random.Range(0, players.Count);
            elQuePilla = players[randomIndex];
            escalaInicial=elQuePilla.transform.localScale;
            StatsDelQuePilla();
        }

        elQuePilla.GetComponent<PlayerController>().OnCollisionEntered += SanChocao;

        currentTime = maxTime;
        UpdateQuienPilla(elQuePilla.playerIndex);
    }
    void Update()
    {
        UpdateTimer();
        FinJuego();
    }
    void UpdateTimer()
    {
        if (!endGame)
        {
            currentTime -= Time.deltaTime;
            timer.text = Mathf.Ceil(currentTime).ToString();
        }
    }
    void UpdateQuienPilla(int playerIndex)
    {
        switch (playerIndex)
        {
            case 0:
                quienPillaText.text = "pilla el jugador 1";
                break;
            case 1: 
                quienPillaText.text = "pilla el jugador 2";
                break;
        }
    }

    void FinJuego()
    {

        if (currentTime <= 0 && elQuePilla != null)
        {
            Destroy(elQuePilla.gameObject);
            endGame = true;
        }
    }
    private void SanChocao(Collision collision)
    {
        if (!puedePillar) return;
        MultiplayerInstance jugadorChocado = collision.gameObject.GetComponent<MultiplayerInstance>();
        if (jugadorChocado != null && jugadorChocado != elQuePilla ) 
        {
            elQuePilla.transform.localScale = escalaInicial;
            elQuePilla.GetComponent<PlayerController>().OnCollisionEntered -= SanChocao;
            ResetStats();
            elQuePilla = collision.gameObject.GetComponent<MultiplayerInstance>();
            elQuePilla.GetComponent<PlayerController>().OnCollisionEntered += SanChocao;
            UpdateQuienPilla(elQuePilla.playerIndex);
            StatsDelQuePilla();

            StartCoroutine(DelayPillar(.1f));
        }
    }

    private IEnumerator DelayPillar(float segundos)
    {
        puedePillar = false;
        yield return new WaitForSeconds(segundos);
        puedePillar = true;
    }
    void StatsDelQuePilla()
    {
        elQuePilla.transform.localScale = escalaInicial * 1.5f;
        elQuePilla.GetComponent<PlayerController>().SetMovementForceMultiplier(2.5f);
    }
    void ResetStats()
    {
        elQuePilla.transform.localScale = escalaInicial;
        elQuePilla.GetComponent<PlayerController>().ResetMovementForce();
    }
}
