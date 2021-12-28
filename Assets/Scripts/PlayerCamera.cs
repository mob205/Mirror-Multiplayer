using UnityEngine;

public class PlayerCamera : Singleton<PlayerCamera>
{
    [SerializeField] private float smoothing = 0.1f;
    
    private PlayerMovement linkedPlayer;

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
    public void SetFollowTarget(PlayerMovement player)
    {
        linkedPlayer = player;
    }
}
