using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUI : MonoBehaviour
{
    [SerializeField] private GameObject template;
    [SerializeField] private Transform startingLocation;
    [SerializeField] private Vector2 offset;

    public void AddAbility(AbilityUpgrade ability)
    {
        Debug.Log("Adding ability to UI " + ability.name);
    }
}
