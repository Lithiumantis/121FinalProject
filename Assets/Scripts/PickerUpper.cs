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
    public DeliveryManager deliveryManager;
    public GameObject destination;

    bool hasItem;

    private AudioSource beepAudio;

    // Start is called before the first frame update
    void Start()
    {
        movement = GetComponentInParent<Movement>();
        beepAudio = GetComponent<AudioSource>();
        scoreText.text = "";
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
            //score += (int)(thrust * 1000);
            scoreText.text = "Item collected";
            hasItem = true;
            destination.SetActive(true);

            ItemSpawner spawner = other.gameObject.GetComponentInParent<ItemSpawner>();
            spawner.SendCollectSignal();
            Destroy(other.gameObject);
            beepAudio.Play();
        }
        else if (other.gameObject.tag == "DeliveryTarget" && hasItem)
        {
            destination.SetActive(false);
            deliveryManager.OnItemCollected();
            hasItem = false;
            beepAudio.Play();
            scoreText.text = "";
        }
    }
}
