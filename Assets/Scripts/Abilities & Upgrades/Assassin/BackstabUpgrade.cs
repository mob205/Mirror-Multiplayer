using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackstabUpgrade : MonoBehaviour
{
    public float backstabDamageModifier;
    public float angleThreshold;
    private void Start()
    {
        Debug.Log("Apply backstab upgrade.");
        var oldHitbox = GetComponentInChildren<MeleeHitbox>();
        var hitboxObj = oldHitbox.gameObject;
        var newHitbox = hitboxObj.AddComponent<BackstabHitbox>();
        newHitbox.DamageModifier = backstabDamageModifier;
        newHitbox.AngleThreshold = angleThreshold;
        Destroy(oldHitbox);
    }
}
