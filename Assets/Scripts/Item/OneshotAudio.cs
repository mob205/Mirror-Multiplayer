using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneshotAudio : MonoBehaviour
{
    private AudioSource sound;
    void Start()
    {
        sound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!sound.isPlaying)
        {
            Destroy(gameObject);
        }
    }
}
