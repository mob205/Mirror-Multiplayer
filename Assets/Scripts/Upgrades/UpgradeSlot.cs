using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeSlot : MonoBehaviour
{
    [Header("Upgrade Management")]
    public int level;
    public string upgradeID;
    public GameObject upgradePrefab;
    [HideInInspector] public string prereqUpgradeID;
    [HideInInspector] public string nextUpgradeID;

    [Header("Display")]
    [TextArea] public string description;
    public Sprite icon;
    public Color color;
    void Start()
    {
        var parentSlot = transform.parent.GetComponent<UpgradeSlot>();
        if (parentSlot)
        {
            prereqUpgradeID = parentSlot.upgradeID;
        }
        UpgradeSlot childSlot = null;
        if (transform.childCount > 0) { childSlot = transform.GetChild(0).GetComponent<UpgradeSlot>(); }
        if (childSlot)
        {
            nextUpgradeID = childSlot.upgradeID;
        }
    }
}
