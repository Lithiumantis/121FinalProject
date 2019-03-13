using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{

    public GameObject item;
    private GameObject spawnedItem;
    private ParticleSystem particle;
    public DeliveryManager deliveryManager;

    // Start is called before the first frame update
    void Start()
    {
        particle = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnItem()
    {
        spawnedItem = Instantiate(item, transform.position, Quaternion.identity);
        spawnedItem.transform.parent = this.gameObject.transform;
        particle = GetComponent<ParticleSystem>();
        particle.Play();
    }

    public void SendCollectSignal()
    {
        //deliveryManager.OnItemCollected();
        particle.Stop();
    }
}
