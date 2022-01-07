using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreController : MonoBehaviour
{
    private int _score = 0;

    HUDController HUD;
    private void Start()
    {
      HUD = GameObject.FindObjectOfType<HUDController>();
    }

    public void AddScore(int score)
    {
        this._score += score;
        HUD.UpdateScore(_score);
    }

    public int GetScore()
    {
        return _score;
    }
}
