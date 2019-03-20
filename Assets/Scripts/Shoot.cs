using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shoot : MonoBehaviour
{
    public ParticleSystem gunParticles;
    private AudioSource audioSource;
    private bool shooting = false;

    public bool paused;

    public int maxAmmo = 1000;
    private int ammo = 1000;

    public Slider ammoSlider;
    public Image ammoImage;

    private bool recharging = false;
    private WaitForSeconds RechargeTime = new WaitForSeconds(1);

    // Start is called before the first frame update
    void Start()
    {
        gunParticles = GetComponent<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
        ammoSlider.maxValue = maxAmmo;
        ammo = maxAmmo;
        ammoImage.color = Color.magenta;
    }

    // Update is called once per frame
    void Update()
    {
        Fire();
    }

    private void FixedUpdate()
    {
        AmmoRefill();

        Debug.Log(ammo);
        ammoSlider.value = ammo;
    }

    void Fire()
    {
        if (paused)
        {
            gunParticles.Pause();
            audioSource.Pause();
        }
        if (Input.GetButtonDown("Fire1") && !paused && !recharging)
        {
            gunParticles.Play();
            audioSource.Play();
            shooting = true;
        }
        if (Input.GetButtonUp("Fire1") && !paused)
        {
            gunParticles.Stop();
            audioSource.Stop();
            shooting = false;
        }

        if(ammo <= 0 && !recharging)
        {
            gunParticles.Stop();
            audioSource.Stop();
            shooting = false;
            recharging = true;
            ammoImage.color = Color.red;
        }
    }

    void AmmoRefill()
    {
        if (shooting && ammo > 0)
        {
            ammo--;
        }

        if (!shooting && ammo < maxAmmo)
        {
            ammo++;
        }

        if (ammo >= maxAmmo)
        {
            recharging = false;
            ammoImage.color = Color.magenta;
        }
    }



}
