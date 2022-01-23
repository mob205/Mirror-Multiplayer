using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UpgradeUI : MonoBehaviour
{
    [SerializeField] private UpgradeDisplay displayPrefab;
    [SerializeField] private Transform displayCenter;
    [SerializeField] private int displayDist;
    [SerializeField] private GameObject playerPreview;

    private List<UpgradeDisplay> displayObjects = new List<UpgradeDisplay>();
    private UpgradeManager upgradeManager;
    private void Awake()
    {
        upgradeManager = FindObjectOfType<UpgradeManager>();
    }
    private void ClearDisplay()
    {
        foreach(var displayObj in displayObjects)
        {
            Destroy(displayObj.gameObject);
        }
        displayObjects.Clear();
    }
    private Vector3[] GetDisplayPositions(int total)
    {
        var output = new List<Vector3>();
        for(int i = 0; i < total; i++)
        {
            var x = 0f;
            var displayOffset = 0f;
            // If i is even, direction is 1. If it's odd, direction is -1.
            var direction = 1 + 2 * (i % 2 * -1);
            if (total % 2 == 0)
            {
                // Evens alternate offset from the center
                displayOffset = displayDist * .5f;
                x = (displayDist * ((i / 2)) + displayOffset) * direction;
            }
            else
            {
                // Odds start in the center and then alternate from center
                x = (int)Mathf.Ceil(i / 2f) * displayDist * direction;
            }
            output.Add(new Vector3(x, displayCenter.localPosition.y));
        }
        output = output.OrderBy(v => v.x).ToList();
        return output.ToArray();
    }
    public void DisplayUpgrades(string[] upgrades)
    {
        ClearDisplay();
        var positions = GetDisplayPositions(upgrades.Length);
        for (int i = 0; i < upgrades.Length; i++)
        {
            var upgrade = upgrades[i];
            var upgradeSlot = upgradeManager.GetSlotFromID(upgrade);
            var displayObj = Instantiate(displayPrefab, transform);
            displayObjects.Add(displayObj);

            displayObj.transform.localPosition = positions[i];
            displayObj.nameText.text = upgradeSlot.name;
            displayObj.descText.text = upgradeSlot.description;
            displayObj.iconImage.sprite = upgradeSlot.icon;
            displayObj.panel.color = upgradeSlot.color;
            displayObj.button.onClick.AddListener(() =>
            {
                OnDisplayClick(upgrade);
            });
            if(upgradeSlot.level > 0)
            {
                displayObj.lvlText.text = "LVL " + upgradeSlot.level.ToString();
            }
        }
    }
    public void OnDisplayClick(string id)
    {
        upgradeManager.CmdRequestAddUpgrade(id);
    }
    public void UpdatePreview(string classUpgradeID)
    {
        var upgrade = UpgradeManager.GetUpgradeFromID(classUpgradeID) as ClassUpgrade;
        playerPreview.GetComponent<SpriteRenderer>().sprite = upgrade.sprite;
        var weaponObj = playerPreview.transform.GetChild(0);
        var rotOffset = weaponObj.rotation;
        Destroy(weaponObj.gameObject);
        Instantiate(upgrade.weapon, playerPreview.transform.position, rotOffset, playerPreview.transform);
    }
}
