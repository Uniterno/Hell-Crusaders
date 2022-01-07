using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    private bool _isPaused = false;
    private GameObject _pauseHUD;
    private WeaponController _weapon;

    // Start is called before the first frame update
    void Start()
    {
        _pauseHUD = GameObject.Find("Canvas").transform.GetChild(2).gameObject;
        _weapon = GameObject.Find("Weapon").GetComponent<WeaponController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Time.timeScale = Time.timeScale == 1 ? 0 : 1;
            _isPaused = Time.timeScale == 1 ? false : true;
            _pauseHUD.SetActive(_isPaused);
            _weapon.transform.GetChild(0).GetComponent<AudioSource>().volume = Time.timeScale == 1 ? 0 : 1;
            _weapon.transform.GetChild(1).GetComponent<AudioSource>().volume = Time.timeScale == 1 ? 0 : 1;

        }
    }
}
