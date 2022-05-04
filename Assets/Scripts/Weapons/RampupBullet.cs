using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RampupBullet : Bullet
{
    public float DamageGainRate { get; set; }
    private void Update()
    {
        Damage += DamageGainRate * Time.deltaTime;
    }
}
