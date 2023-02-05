using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundRandomiser : MonoBehaviour
{

    public AudioClip[] sounds;
    private AudioSource source;
    [Range(0.1f,0.5f)]
    public float volumeChangeMultiplier = 0.2f;
    [Range(0.1f, 0.5f)]
    public float pitchChangeMultiplier = 0.2f;


    // Start is called before the first frame update
    void Start()
    {
        source = gameObject.AddComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RandomiseFootstep()
    {
        source.clip = sounds[Random.Range(0, sounds.Length)];
        source.volume = 1.0f;
        source.pitch = Random.Range(1 - pitchChangeMultiplier, 1 + pitchChangeMultiplier);
        source.PlayOneShot(source.clip);
    }

    public void RandomiseSparks()
    {
        source.clip = sounds[Random.Range(0, sounds.Length)];
        source.volume = Random.Range(0.3f - volumeChangeMultiplier, 0.3f);
        source.pitch = Random.Range(1 - pitchChangeMultiplier, 1 + pitchChangeMultiplier);
        source.PlayOneShot(source.clip);
    }

    public void RandomiseExplosion()
    {
        source.clip = sounds[Random.Range(0, sounds.Length)];
        source.volume = Random.Range(0.3f - volumeChangeMultiplier, 0.3f);
        source.pitch = Random.Range(1 - pitchChangeMultiplier, 1 + pitchChangeMultiplier);
        source.PlayOneShot(source.clip);
    }
}
