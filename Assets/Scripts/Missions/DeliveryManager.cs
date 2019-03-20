using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryManager : MissionManager
{
    public Text itemsText;
    public Text waitText;
    public MissionSelect missionSelect;
    public ItemSpawner[] spawners;
    public int itemsCollected = 0;
    public int requiredItems = 6;

    private void Start()
    {

    }


    public void SpawnAtRandom()
    {
        //Debug.Log("Spawning at random...");
        int i = Random.Range(0, spawners.Length);
        spawners[i].SpawnItem();
    }

    public override void StartMission()
    {
        Debug.Log("Starting delivery mission...");
        itemsText.text = "Items remaining: " + requiredItems;
        SpawnAtRandom();
    }
    public override void EndMission()
    {
        Debug.Log("Ending delivery mission...");
        waitText.text = "";
        missionSelect.NextMission();
    }

    public void OnItemCollected()
    {
        itemsCollected++;
        itemsText.text = "Items remaining: " + (requiredItems - itemsCollected);
        if (itemsCollected < requiredItems)
        {
            StartCoroutine(SpawnDelay());
        }
        else
        {
            waitText.text = "";
            EndMission();
        }
    }

    private IEnumerator SpawnDelay()
    {
        //Debug.Log("Entering spawn delay");
        waitText.text = "Preparing new item...";
        yield return new WaitForSeconds(2);
        SpawnAtRandom();
        //Debug.Log("Exiting spawn delay");
        waitText.text = "";
        yield return null;
    }
}
