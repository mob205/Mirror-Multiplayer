using Mirror;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(NetworkIdentity))]
[RequireComponent(typeof(NetworkTransform))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : NetworkBehaviour
{
    private enum State
    {
        Walking,
        Dashing
    }
    [SerializeField] private float speed = 5f;

    private Rigidbody2D rb;
    private State state = State.Walking;
    private Vector3 dashVector;

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
        if (!hasAuthority) { return; }
        if (state == State.Walking)
        {
            ProcessInput();
        }
        else if (state == State.Dashing)
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
    public void Dash(Vector3 dir, float speed, float duration)
    {
        dashVector = dir * speed;
        state = State.Dashing;
        StartCoroutine(ResetState(duration));
    }
    private IEnumerator ResetState(float time)
    {
        yield return new WaitForSeconds(time);
        state = State.Walking;
    }
}
