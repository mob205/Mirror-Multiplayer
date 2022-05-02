using Mirror;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(NetworkIdentity))]
[RequireComponent(typeof(NetworkTransform))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : NetworkBehaviour
{
    public enum State
    {
        Walking = 0,
        Dashing = 1,
        Immobilized = 2
    }
    [SerializeField] private float speed = 5f;
    [SerializeField] private LayerMask dashingLayer;

    [HideInInspector] [SyncVar] public float speedModifier = 1;
    public State CurrentState { get; private set; } = State.Walking;
    [SyncVar] private Vector3 dashVector;
    private Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public override void OnStartLocalPlayer()
    {
        var cam = FindObjectOfType<PlayerCamera>();
        if (cam) { cam.SetFollowTarget(gameObject); }
    }
    private void Update()
    {
        if(CurrentState == State.Immobilized || !hasAuthority) { return; }
        if (CurrentState == State.Walking)
        {
            ProcessInput();
        }
        else if (CurrentState == State.Dashing)
        {
            ApplyDash();
        }
    }
    private void ProcessInput()
    {
        Vector2 movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        rb.MovePosition(transform.position + speed * speedModifier * Time.fixedDeltaTime * (Vector3)movement);
    }
    private void ApplyDash()
    {
        rb.MovePosition(transform.position + dashVector * Time.fixedDeltaTime);
    }
    [Server]
    public void SetState(State state, float resetDelay)
    {
        CurrentState = state;
        StartCoroutine(ResetState(resetDelay));
        RpcSetState(state, resetDelay);
    }
    [ClientRpc]
    public void RpcSetState(State state, float resetDelay)
    {
        CurrentState = state;
        StartCoroutine(ResetState(resetDelay));
    }
    [Server]
    public void SetDashVector(Vector3 dir, float speed)
    {
        dashVector = dir * speed;
    }
    private IEnumerator ResetState(float time)
    {
        yield return new WaitForSeconds(time);
        CurrentState = State.Walking;
        gameObject.layer = LayerMask.NameToLayer("Player");
    }
}
