using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassUpgrade : MonoBehaviour
{
    public GameObject weapon;
    public Sprite sprite;
    private void Start()
    {
        Debug.Log("Starting class upgrade.");
        GetComponent<PlayerCombat>().SetWeapon(weapon);

        GetComponent<SpriteRenderer>().sprite = sprite;
    }
}
