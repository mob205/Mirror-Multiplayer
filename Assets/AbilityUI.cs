using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUI : MonoBehaviour
{
    [SerializeField] private AbilityDisplay template;
    [SerializeField] private Transform startingLocation;
    [SerializeField] private Vector2 offset;

    private List<AbilityDisplay> displayObjects = new List<AbilityDisplay>();
    public void AddAbility(AbilityUpgrade ability)
    {
        var displayObj = Instantiate(template, transform);
        displayObj.transform.localPosition = startingLocation.localPosition + (Vector3)(offset * displayObjects.Count);
        displayObj.SetAbility(ability);
        displayObjects.Add(displayObj);

    }
}
