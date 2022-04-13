using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekingProjectile : MonoBehaviour
{
    [SerializeField] private LayerMask targetLayers;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float seekRange;
    [SerializeField] private float searchDelay = 1;

    private GameObject currentTarget;
    private Rigidbody2D rb;
    private Bullet bullet;
    private float currentCooldown;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bullet = GetComponent<Bullet>();
        FindTarget();
    }
    private void FixedUpdate()
    {
        if (currentCooldown <= 0 && currentTarget == null)
        {
            FindTarget();
            currentCooldown = searchDelay;
        }
        else if (currentTarget != null)
        {
            FollowTarget();
        }
        else
        {
            currentCooldown -= Time.deltaTime;
        }
    }
    private void FindTarget()
    {
        currentTarget = null;
        var targetsInRange = Physics2D.OverlapCircleAll(bullet.Shooter.transform.position, seekRange);
        float leastDistance = Mathf.Infinity;
        foreach(var target in targetsInRange)
        {
            var squaredDist = (target.gameObject.transform.position - transform.position).sqrMagnitude;
            if(squaredDist < leastDistance && (targetLayers.value & (1 << (target.gameObject.layer))) > 0 && target.gameObject != bullet.Shooter)
            {
                currentTarget = target.gameObject;
                leastDistance = squaredDist;
            }
        }
    }
    private void FollowTarget()
    {
        var currentAngle = transform.rotation.eulerAngles.z;
        var targetAngle = Utility.GetDirection(currentTarget.transform.position, transform).eulerAngles.z;
        var angleDiff = targetAngle - currentAngle;
        var rotation = Time.fixedDeltaTime * rotationSpeed;

        if(angleDiff > 180)
        {
            angleDiff -= 360;
        }
        else if (angleDiff < -180)
        {
            angleDiff += 360;
        }

        if(angleDiff <= rotation && angleDiff >= -rotation)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, targetAngle));
        }
        else
        {
            var dir = angleDiff / Mathf.Abs(angleDiff);
            transform.Rotate(new Vector3(0, 0, dir * rotation));
        }
        rb.velocity = transform.right * rb.velocity.magnitude;
    }
}
