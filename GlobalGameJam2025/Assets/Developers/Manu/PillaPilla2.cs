using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PillaPilla2 : MonoBehaviour
{ 
//{
//    List<MultiplayerInstance> players;
//    MultiplayerInstance elQuePilla;
//    Vector3 escalaInicial;
//    float currentTime = 10;
//    [SerializeField] float maxTime;
//    bool puedePillar = true;
//    [SerializeField] TMP_Text timer;


//    void Start()
//    {

//        StartCoroutine(InicioConRetraso());
//    }

//    IEnumerator InicioConRetraso()
//    {

//        yield return new WaitForSeconds(3f);
//        //timer.text = Mathf.Ceil(maxTime).ToString();
//        ElegirElQuePilla();
//        switch (players.Count)
//        {
//            case 2:
//                currentTime = maxTime;
//                break;

//            case 3:
//                currentTime = maxTime * 2;
//                break;

//            case 4:
//                currentTime = maxTime * 3;
//                break;
//        }

//    }
//    private void Update()
//    {
//        UpdateTimer();
//    }
//    void UpdateTimer()
//    {
//        currentTime -= Time.deltaTime;
//        //timer.text = Mathf.Ceil(currentTime).ToString();
//        if (currentTime <= 0)
//        {
//            Matar();
//        }
//    }
//    void Matar()
//    {
//        MinigameManager man = MinigameManager.Instance;
//        if (elQuePilla != null)
//        {
//            SoundManager.Instance.PlaySound(Sound.BubbleExplosion);
//            switch (players.Count)
//            {
//                case 3:
//                    currentTime = maxTime;
//                    break;

//                case 4:
//                    currentTime = maxTime * 2;
//                    break;
//            }

//            man.PlayerDeath(elQuePilla);
//            elQuePilla.GetComponent<PlayerController>().SetLinearVelocity(Vector3.up * 9999999999999);
//            ElegirElQuePilla(elQuePilla);
//        }
//    }

//    private void ElegirElQuePilla(MultiplayerInstance muerto = null)
//    {
//        players = MinigameManager.Instance.GetAllPlayers();
//        if (players != null && players.Count > 0)
//        {
//            if (muerto != null)
//            {
//                do
//                {
//                    int randomIndex = Random.Range(0, players.Count);
//                    elQuePilla = players[randomIndex];
//                }
//                while (elQuePilla == muerto);
//            }
//            else
//            {
//                int randomIndex = Random.Range(0, players.Count);
//                elQuePilla = players[randomIndex];
//            }
//            escalaInicial = elQuePilla.transform.localScale;
//            StatsDelQuePilla();
//        }
//        elQuePilla.GetComponent<PlayerController>().OnCollisionEntered += SanChocao;

//    }

//    private void StatsDelQuePilla()
//    {
//        elQuePilla.transform.localScale = escalaInicial * 1.5f;

//    }
//    void ResetStats()
//    {
//        elQuePilla.transform.localScale = escalaInicial;
//    }
//    void SanChocao(Collision collision)
//    {
//        if (!puedePillar) return;
//        MultiplayerInstance jugadorChocado = collision.gameObject.GetComponent<MultiplayerInstance>();
//        if (jugadorChocado != null && jugadorChocado != elQuePilla)
//        {
//            elQuePilla.transform.localScale = escalaInicial;
//            elQuePilla.GetComponent<PlayerController>().OnCollisionEntered -= SanChocao;
//            ResetStats();
//            elQuePilla = collision.gameObject.GetComponent<MultiplayerInstance>();
//            elQuePilla.GetComponent<PlayerController>().OnCollisionEntered += SanChocao;
//            StatsDelQuePilla();
//            StartCoroutine(DelayPillar(.1f));
//        }
//    }
//    IEnumerator DelayPillar(float delay)
//    {
//        puedePillar = false;
//        yield return new WaitForSeconds(delay);
//        puedePillar = true;
//    }
}