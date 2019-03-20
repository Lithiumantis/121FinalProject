using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AAGun : MonoBehaviour
{

    public CombatManager combatMission;

    public int health = 10;

    public GameObject explodePrefab;
    public AudioClip explodeSFX;
    public AudioClip damageSFX;
    private AudioSource explodeAudio;

    MeshRenderer mr;
    BoxCollider bc;
    // Start is called before the first frame update
    void Start()
    {
        explodeAudio = GetComponent<AudioSource>();
        mr = GetComponent<MeshRenderer>();
        bc = GetComponent<BoxCollider>();
        explodeAudio.clip = damageSFX;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Explode();
        }
    }

    private void OnParticleCollision(GameObject other)
    {

        health--;
        Debug.Log("Health: " + health);
        if (health <= 0)
        {
            Explode();
        }
        else
        {
            explodeAudio.Play();
        }

    }

    private void Explode()
    {
        combatMission.OnKilled();
        explodeAudio.clip = explodeSFX;
        explodeAudio.Play();
        Instantiate(explodePrefab, transform.position, Quaternion.identity);
        mr.enabled = false;
        bc.enabled = false;

        Transform bullets = transform.GetChild(0);
        bullets.gameObject.SetActive(false);
    }
}
