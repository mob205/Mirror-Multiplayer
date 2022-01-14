using Mirror;
using UnityEngine;

[RequireComponent(typeof(NetworkIdentity))]
[RequireComponent(typeof(NetworkTransform))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private float speed = 5f;

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
        if (hasAuthority)
        {
            ProcessInput();
        }
    }
    private void ProcessInput()
    {
        Vector2 movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        rb.MovePosition(transform.position + (Vector3)movement * speed * Time.fixedDeltaTime);
    }
}
