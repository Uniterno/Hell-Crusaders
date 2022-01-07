using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceWisp : Enemy
{
    protected override void Quirk() // Effects unique to this kind of Enemy
    {
        SlowdownPlayer();
    }

    void SlowdownPlayer()
    {
        _player.GetComponent<PlayerController>().AdjustPlayerSpeed(0.8f, 5, "Ice");
    }
}
