using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public ParticleSystem gunParticles;
    // Start is called before the first frame update
    void Start()
    {
        gunParticles = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            gunParticles.Play();
        }
        if (Input.GetButtonUp("Fire1"))
        {
            gunParticles.Stop();
        }
    }


}
