using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWisp : Enemy
{
    protected override void Quirk() // Effects unique to this kind of Enemy
    {
        _attackDamage = 6;
    }
}
