using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunsFollowPlayer : MonoBehaviour
{

    public GameObject player;
    float distFromPlayer;
    private ParticleSystem particles;
    public float range = 200;
    public float elevationrange = 1;

    private bool active = false;

    // Start is called before the first frame update
    void Start()
    {

        particles = GetComponent<ParticleSystem>();
        particles.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        distFromPlayer = Vector3.Distance(transform.position, player.transform.position);
        //Debug.Log(distFromPlayer);
        float verticalDifference = player.transform.position.y - transform.position.y;

        if (distFromPlayer <= range && verticalDifference > elevationrange)
        {
            if (!active)
            {
                particles.Play();
                active = true;
            }

            transform.LookAt(player.transform);
        }else if (active)
        {
            particles.Stop();
            active = false;
        }
    }
}
