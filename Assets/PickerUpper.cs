using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickerUpper : MonoBehaviour
{
    public Text scoreText;
    private Movement movement;
    private int score = 0;
    private float thrust;

    private AudioSource beepAudio;

    // Start is called before the first frame update
    void Start()
    {
        movement = GetComponentInParent<Movement>();
        beepAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        thrust = movement.thrust;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("pickup collision");
        if (other.gameObject.tag == "Pickup")
        {
            score += (int)(thrust * 1000);
            scoreText.text = score.ToString();
            Destroy(other.gameObject);

            beepAudio.Play();

        }
    }
}
