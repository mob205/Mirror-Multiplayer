using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeUI : MonoBehaviour
{
    [SerializeField] UpgradeDisplay displayPrefab;
    [SerializeField] Transform displayCenter;
    [SerializeField] int displayDist;

    List<UpgradeDisplay> displayObjects = new List<UpgradeDisplay>();
    private UpgradeManager upgradeManager;
    private void Awake()
    {
        upgradeManager = FindObjectOfType<UpgradeManager>();
    }
    private void ClearDisplay()
    {
        foreach(var displayObj in displayObjects)
        {
            Destroy(displayObj);
        }
        displayObjects.Clear();
    }
    private Vector3 GetDisplayPos(int i, int total)
    {
        var displayOffset = 0f;
        // If i is even, direction is 1. If it's odd, direction is -1.
        var direction = 1 + 2*(i % 2 * -1);
        if(total % 2 == 0)
        {
            displayOffset = displayDist * .5f;
        }
        var x = (displayDist * ((i / 2)) + displayOffset) * direction;
        Debug.Log($"{displayDist} * (({i} / 2)) + {displayOffset}) * {direction}");
        //if (total % 2 == 1)
        //{
        //    // Odds start in center
        //    x = (i * displayDist) * direction;
        //}
        //else
        //{
        //    // Evens start off the center
        //    x = (float)((displayDist * .5) + (i * displayDist)) * direction;
        //}
        Debug.Log($"{i}: {x} | {i/2}");
        return new Vector3(x, displayCenter.position.y);
    }
    public void DisplayUpgrades(string[] upgrades)
    {
        for (int i = 0; i < upgrades.Length; i++)
        {
            var upgrade = upgrades[i];
            var upgradeSlot = upgradeManager.GetSlotFromID(upgrade);
            var displayObj = Instantiate(displayPrefab, transform);
            displayObjects.Add(displayObj);

            displayObj.transform.localPosition = GetDisplayPos(i, upgrades.Length);
            displayObj.nameText.text = upgradeSlot.name;
            displayObj.descText.text = upgradeSlot.description;
            displayObj.iconImage.sprite = upgradeSlot.icon;
            displayObj.button.onClick.AddListener(() =>
            {
                OnDisplayClick(upgrade);
            });
        }
    }
    public void OnDisplayClick(string id)
    {
        Debug.Log($"Clicked with {id}");
    }
}
