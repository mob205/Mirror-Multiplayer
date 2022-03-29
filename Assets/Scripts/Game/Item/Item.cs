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

    private Vector2 startPos;
    private float startTime;

    private void Start()
    {
        startPos = transform.position;
        startTime = Time.time;
    }
    private void Update()
    {
        float oscillateFactor = 0.5f * (1 + Mathf.Sin(speed * (Time.time - startTime) - (Mathf.PI / 2)));
        transform.position = Vector2.Lerp(startPos, startPos + distance, oscillateFactor);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the colliding object is in a player layer
        if ((interactableLayers.value & (1 << (collision.gameObject.layer))) > 0)
        {
            LocalActivate(collision);
            if (isServer) { ServerActivate(collision); }
            NetworkServer.Destroy(gameObject);
        }
    }
    protected abstract void LocalActivate(Collider2D collision);
    protected abstract void ServerActivate(Collider2D collision);
}
