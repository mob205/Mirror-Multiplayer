using UnityEngine;

public class OneshotAudio : MonoBehaviour
{
    public float minPitch = 1;
    public float maxPitch = 1;
    private AudioSource sound;
    void Start()
    {
        sound = GetComponent<AudioSource>();
        sound.pitch = Random.Range(minPitch, maxPitch);
        sound.Play();
    }
    void Update()
    {
        if (!sound.isPlaying)
        {
            Destroy(gameObject);
        }
    }
}
