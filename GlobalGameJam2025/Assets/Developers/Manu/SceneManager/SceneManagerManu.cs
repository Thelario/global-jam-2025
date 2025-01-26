using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneManagerManu : MonoBehaviour
{
    [SerializeField] private List<string> scenes;
    private static SceneManagerManu instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        if (scenes.Count > 0)
        {
            LoadRandomScene();
        }
    }

    void LoadRandomScene()
    {
        int randomIndex = Random.Range(0, scenes.Count);
        string sceneToLoad = scenes[randomIndex];
        scenes.RemoveAt(randomIndex);
        Debug.Log($"Cargando escena: {sceneToLoad} (quedan {scenes.Count} escenas en el pool)");
        //SceneManager.LoadScene(sceneToLoad);

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            LoadRandomScene();
        }
    }
}
