using Mirror;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private float smoothing = 0.1f;
    
    private GameObject linkedPlayer;

    private void FixedUpdate()
    {
        if (linkedPlayer)
        {
            FollowPlayer();
        }
    }
    private void FollowPlayer()
    {
        transform.position = Vector3.Lerp(transform.position, 
            new Vector3(linkedPlayer.transform.position.x, linkedPlayer.transform.position.y, transform.position.z), smoothing);
    }
    public void SetFollowTarget(GameObject player)
    {
        linkedPlayer = player;
        // Center the camera when the target is set
        transform.position = new Vector3(linkedPlayer.transform.position.x, linkedPlayer.transform.position.y, transform.position.z);
    }
}
