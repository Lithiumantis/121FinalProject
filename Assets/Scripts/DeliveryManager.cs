using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryManager : MissionManager
{
    public Text itemsText;
    public MissionSelect missionSelect;
    public ItemSpawner[] spawners;
    public int itemsCollected = 0;
    public int requiredItems = 6;

    private void Start()
    {
        itemsText.text = "Items remaining: " + requiredItems;
    }


    public void SpawnAtRandom()
    {
        Debug.Log("Spawning at random...");
        int i = Random.Range(0, spawners.Length);
        spawners[i].SpawnItem();
    }

    public override void StartMission()
    {
        Debug.Log("Starting delivery mission...");
        SpawnAtRandom();
    }
    public override void EndMission()
    {
        Debug.Log("Ending delivery mission...");
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
            EndMission();
        }
    }

    private IEnumerator SpawnDelay()
    {
        yield return new WaitForSeconds(2);
        SpawnAtRandom();
        yield return null;
    }
}
