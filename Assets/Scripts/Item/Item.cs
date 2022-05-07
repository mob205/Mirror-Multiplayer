using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : NetworkBehaviour
{
    [SerializeField] private LayerMask interactableLayers;

    [Header("Oscillation")]
    [SerializeField] private float speed;
    [SerializeField] private Vector2 distance;
    [SerializeField] private Transform shadowTransform;

    private Vector2 startPos;
    private double startTime;
    private Vector2 shadowOffset;

    protected virtual void Start()
    {
        startPos = transform.position;
        startTime = NetworkTime.time;
        if (shadowTransform)
        {
            shadowOffset = shadowTransform.position;
        }
    }
    private void Update()
    {
        float oscillateFactor = 0.5f * (1 + Mathf.Sin(speed * (float)(NetworkTime.time - startTime) - (Mathf.PI / 2)));
        transform.position = Vector2.Lerp(startPos, startPos + distance, oscillateFactor);
        if (shadowTransform)
        {
            shadowTransform.position = shadowOffset;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the colliding object is in a player layer
        if ((interactableLayers.value & (1 << (collision.gameObject.layer))) > 0)
        {
            if (!IsTriggerable(collision)) { return; }
            LocalActivate(collision);
            if (isServer) { ServerActivate(collision); }
            NetworkServer.Destroy(gameObject);
        }
    }
    protected virtual bool IsTriggerable(Collider2D collision)
    {
        return true;
    }
    protected abstract void LocalActivate(Collider2D collision);
    protected abstract void ServerActivate(Collider2D collision);
}
