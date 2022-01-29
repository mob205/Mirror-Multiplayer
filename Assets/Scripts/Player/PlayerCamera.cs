using Mirror;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private float smoothing = 0.1f;
    
    private GameObject linkedPlayer;

    private void FixedUpdate()
    {
        if (linkedPlayer && linkedPlayer.activeSelf)
        {
            FollowPlayer();
        }
        else
        {
            SetRandomTarget();
        }
    }
    private void FollowPlayer()
    {
        transform.position = Vector3.Lerp(transform.position,
            new Vector3(linkedPlayer.transform.position.x, linkedPlayer.transform.position.y, transform.position.z), smoothing);
    }
    private void SetRandomTarget()
    {
        var players = FindObjectsOfType<PlayerMovement>();
        if(players.Length > 0)
        {
            SetFollowTarget(players[0].gameObject);
        }
    }
    public void SetFollowTarget(GameObject player)
    {
        linkedPlayer = player;
        // Center the camera when the target is set
        transform.position = new Vector3(linkedPlayer.transform.position.x, linkedPlayer.transform.position.y, transform.position.z);
    }
}
