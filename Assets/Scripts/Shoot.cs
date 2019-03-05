using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public ParticleSystem gunParticles;
    private AudioSource audioSource;

    public bool paused;
    // Start is called before the first frame update
    void Start()
    {
        gunParticles = GetComponent<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (paused)
        {
            gunParticles.Pause();
            audioSource.Pause();
        }
        if (Input.GetButtonDown("Fire1") && !paused)
        {
            gunParticles.Play();
            audioSource.Play();
        }
        if (Input.GetButtonUp("Fire1") && !paused)
        {
            gunParticles.Stop();
            audioSource.Stop();
        }
    }


}
