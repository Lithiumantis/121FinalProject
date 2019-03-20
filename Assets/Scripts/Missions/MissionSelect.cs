using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MissionSelect : MonoBehaviour
{
    public GameObject[] missionList;
    private MissionManager currentMission;
    private int missionNumber = 0;
    public Text endText;
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
            StartCoroutine(NextDelay());
        }
        else
        {
            Debug.Log("You win!");
            endText.text = "Mission Accomplished";
            StartCoroutine(End());
        }
    }

    private IEnumerator End()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("Menu");
    }

    private IEnumerator NextDelay()
    {
        currentMission = missionList[missionNumber].GetComponent<MissionManager>();
        endText.text = "Next mission: " + currentMission.displayName;
        yield return new WaitForSeconds(4);
        endText.text = "";

        currentMission.StartMission();

    }
}
