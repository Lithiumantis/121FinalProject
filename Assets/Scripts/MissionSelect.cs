using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionSelect : MonoBehaviour
{
    public GameObject[] missionList;
    private MissionManager currentMission;
    private int missionNumber = 0;
    // Start is called before the first frame update
    void Start()
    {
        currentMission = missionList[missionNumber].GetComponent<MissionManager>();
        currentMission.StartMission();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void NextMission()
    {
        missionNumber++;
        if(missionNumber < missionList.Length)
        {
            currentMission = missionList[missionNumber].GetComponent<MissionManager>();
            currentMission.StartMission();
        }
        else
        {
            Debug.Log("You win!");
        }
    }
}
