using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicWisp : Enemy
{
    protected override void Quirk() // Effects unique to this kind of Enemy
    {
        PoisonPlayer();
    }

    void PoisonPlayer()
    {
        _player.GetComponent<PlayerController>().SetPoisoned(10, 1f);
    }
}
