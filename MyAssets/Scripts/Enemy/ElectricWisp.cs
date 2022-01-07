using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricWisp : Enemy
{
    protected override void Quirk() // Effects unique to this kind of Enemy
    {
        Shock();
        Zap();
    }

    void Shock()
    {
        _player.GetComponent<PlayerController>().AdjustPlayerSpeed(0.15f, 1, "Electric");
    }

    void Zap()
    {
        _weapon.GetComponent<WeaponController>().AdjustShootingSpeed(0.8f, 2);
    }
}
