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
        Walking,
        Dashing
    }
    [SerializeField] private float speed = 5f;
    [SerializeField] private LayerMask dashingLayer;

    private Rigidbody2D rb;
    public State CurrentState { get; private set; } = State.Walking;
    private Vector3 dashVector;
    private BoxCollider2D col;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
    }
    public override void OnStartLocalPlayer()
    {
        var cam = FindObjectOfType<PlayerCamera>();
        if (cam) { cam.SetFollowTarget(gameObject); }
    }
    private void Update()
    {
        if(CurrentState == State.Dashing)
        {
            gameObject.layer = LayerMask.NameToLayer("Dashing");
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("Player");
        }

        if (!hasAuthority) { return; }
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
        rb.MovePosition(transform.position + speed * Time.fixedDeltaTime * (Vector3)movement);
    }
    private void ApplyDash()
    {
        rb.MovePosition(transform.position + dashVector * Time.fixedDeltaTime);
    }
    public void StartDash(Vector3 dir, float speed, float duration)
    {
        dashVector = dir * speed;
        CurrentState = State.Dashing;
        CmdSetState(CurrentState);
        StartCoroutine(ResetState(duration));
    }
    [Command]
    public void CmdSetState(State state)
    {
        CurrentState = state;
    }
    private IEnumerator ResetState(float time)
    {
        yield return new WaitForSeconds(time);
        CurrentState = State.Walking;
        CmdSetState(CurrentState);
    }
}
