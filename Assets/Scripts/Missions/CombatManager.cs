using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MissionManager
{
    public Text itemsText;
    public MissionSelect missionSelect;
    public GameObject[] guns;
    private int targetsDestroyed = 0;
    private int requiredTargets = 1;

    // Start is called before the first frame update
    void Start()
    {
        requiredTargets = guns.Length;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void StartMission()
    {
        Debug.Log("Starting delivery mission...");
        itemsText.text = "Targets remaining: " + requiredTargets;
        foreach(GameObject gun in guns)
        {
            gun.SetActive(true);
        }

    }

    public override void EndMission()
    {
        Debug.Log("Ending delivery mission...");
        missionSelect.NextMission();
    }

    public void OnKilled()
    {
        targetsDestroyed++;
        itemsText.text = "Targets remaining: " + (requiredTargets - targetsDestroyed);
        if (targetsDestroyed >= requiredTargets)
            EndMission();
    }
}
