using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryManager : MonoBehaviour
{
    private bool _won = false;
    private GameObject _victoryHUD;
    private HUDController HUD;
    private SpawnController _spawn;
    private ScoreController _scoreController;
    private Scene_Manager _sceneManager;
    private WeaponController _weapon;
    private GameObject _bgmController;

    // Start is called before the first frame update
    void Start()
    {
        GameObject OptimizedCanvas = GameObject.Find("Canvas"); // Stored as local variable for Start to save looking for it twice to get HUD and _deathHUD
        HUD = OptimizedCanvas.transform.GetChild(1).gameObject.GetComponent<HUDController>();
        _victoryHUD = OptimizedCanvas.transform.GetChild(4).gameObject;
        _spawn = GameObject.Find("Spawn").GetComponent<SpawnController>();
        _scoreController = HUD.GetComponentInChildren<ScoreController>();
        _sceneManager = GameObject.Find("Scene Manager").GetComponent<Scene_Manager>();
        _weapon = GameObject.Find("Weapon").GetComponent<WeaponController>();
        _bgmController = GameObject.Find("BGM Controller");
    }

    // Update is called once per frame
    void Update()
    {
        if (_won && Input.GetKeyDown(KeyCode.Return))
        {
            Time.timeScale = 1;
            _sceneManager.LoadMainMenu();
        }
    }

    public void Won()
    {
        _won = true;
        Time.timeScale = 0;
        _weapon.transform.GetChild(0).GetComponent<AudioSource>().volume = 0;
        _weapon.transform.GetChild(1).GetComponent<AudioSource>().volume = 0;
        HUD.ShowVictoryScreen(_scoreController.GetScore());
        _victoryHUD.SetActive(true);
        _bgmController.GetComponent<AudioSource>().Stop();
        this.GetComponent<AudioSource>().Play();
    }
}
