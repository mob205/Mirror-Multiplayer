using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackstabUpgrade : Upgrade
{
    public float backstabDamageModifier;
    public float angleThreshold;
    public override void Initialize()
    {
        var oldHitbox = GetComponentInChildren<MeleeHitbox>();
        var hitboxObj = oldHitbox.gameObject;
        var newHitbox = hitboxObj.AddComponent<BackstabHitbox>();

        newHitbox.DamageModifier = backstabDamageModifier;
        newHitbox.AngleThreshold = angleThreshold;

        // DestroyImmediate to avoid oldHitbox being used over the new one in the frame
        DestroyImmediate(oldHitbox);
    }
}
