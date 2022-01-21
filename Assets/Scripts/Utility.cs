using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
    public static Quaternion GetDirection(Vector3 target, Transform transform)
    {
        // Get displacement vector components from player object to target
        var y = target.y - transform.position.y;
        var x = target.x - transform.position.x;

        // Get rotation from the arctangent of displacement components
        float angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
        return Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
