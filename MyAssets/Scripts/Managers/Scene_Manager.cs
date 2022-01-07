using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Scene_Manager : MonoBehaviour
{

    private static Scene_Manager _instance { get; set; } // Singleton

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        Time.timeScale = 1;
        Debug.Log(Time.timeScale);
        GameObject.Find("PlayButton").GetComponent<Button>().onClick.AddListener(delegate { LoadGame(); });
        GameObject.Find("ExitButton").GetComponent<Button>().onClick.AddListener(delegate { ExitGame(); });
    }
    public void LoadGame()
    {
        Debug.Log("Load Game");
        SceneManager.LoadScene("RTSH");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
