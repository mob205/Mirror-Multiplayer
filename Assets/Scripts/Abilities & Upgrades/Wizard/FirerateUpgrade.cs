using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirerateUpgrade : MonoBehaviour
{
    public float firerateModifier;
    public void Start()
    {
        GetComponent<PlayerCombat>().Weapon.ModifyFirerate(firerateModifier);
    }
}
