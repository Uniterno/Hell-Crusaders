using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDController : MonoBehaviour
{



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateScore(int Score)
    {
        this.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().SetText(Score.ToString());
    }

    public void UpdateRound(int Round)
    {
        this.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().SetText(Round.ToString());
    }

    public void UpdateRemainingEnemies(int RemainingEnemies)
    {
        this.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().SetText(RemainingEnemies.ToString());
    }

    public void UpdateAmmo(int Ammo)
    {
        TextMeshProUGUI AmmoText = this.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>();
        AmmoText.SetText(Ammo.ToString() + "/30");
        if(Ammo == 0)
        {
            AmmoText.color = new Color32(204, 8, 8, 255);
        }
        else if(Ammo < 10)
        {
            AmmoText.color = new Color32(204, 86, 8, 255);
        }
        else
        {
            AmmoText.color = new Color32(0, 0, 0, 255);
        }
    }

    public void UpdateHP(int HP)
    {
        TextMeshProUGUI HPText = this.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>();
        HPText.SetText(HP.ToString() + "/100");
        if (HP == 0)
        {
            HPText.color = new Color32(204, 8, 8, 255);
        }
        else if (HP <= 25)
        {
            HPText.color = new Color32(204, 86, 8, 255);
        }
        else if (HP <= 50)
        {
            HPText.color = new Color32(204, 181, 8, 255);
        }
        else
        {
            HPText.color = new Color32(40, 150, 6, 255);
        }
    }

    public void UpdateEffects(string Effect, int Time)
    {
        TextMeshProUGUI Details = null;
        GameObject Icon = null;

        string DetailsText = Time.ToString() + "s";
        if (Effect == "Frozen")
        {
            Icon = GameObject.Find("Canvas").transform.GetChild(1).GetChild(5).GetChild(0).GetChild(1).gameObject;
            Details = this.transform.GetChild(5).GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
        }
        else if (Effect == "Poisoned")
        {
            Icon = GameObject.Find("Canvas").transform.GetChild(1).GetChild(5).GetChild(1).GetChild(1).gameObject;
            Details = this.transform.GetChild(5).GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
        }
        else if (Effect == "Shock")
        {
            Icon = GameObject.Find("Canvas").transform.GetChild(1).GetChild(5).GetChild(2).GetChild(1).gameObject;
            Details = this.transform.GetChild(5).GetChild(2).GetComponentInChildren<TextMeshProUGUI>();
        }
        else if (Effect == "Zapped")
        {
            Icon = GameObject.Find("Canvas").transform.GetChild(1).GetChild(5).GetChild(3).GetChild(1).gameObject;
            Details = this.transform.GetChild(5).GetChild(3).GetComponentInChildren<TextMeshProUGUI>();
        }

        Details.SetText(DetailsText);
        Icon.SetActive(Time > 0);
        if(Time <= 0)
        {
            Details.SetText("");
        }
    }
    public void ShowAim(bool show = true)
    {
        this.transform.GetChild(6).gameObject.SetActive(show);
    }

    public void ShowDeathScreen(int round, int score)
    {
        TextMeshProUGUI ResultsText = GameObject.Find("Canvas").transform.GetChild(3).GetChild(2).GetComponent<TextMeshProUGUI>();
        ResultsText.SetText("\nRound: " + round.ToString() + "\nScore: " + score.ToString() + "\n");
    }

    public void ShowVictoryScreen(int score)
    {
        TextMeshProUGUI ResultsText = GameObject.Find("Canvas").transform.GetChild(4).GetChild(3).GetComponent<TextMeshProUGUI>();
        ResultsText.SetText("Score: " + score.ToString() + "\n");
    }
}
