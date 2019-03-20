using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Movement player;
    public AudioSource musicPlayer;
    public Text pauseText;
    public Timer timer;

    bool paused;

    // Start is called before the first frame update
    void Start()
    {
        player.TogglePause();
        musicPlayer.Pause();
        pauseText.gameObject.SetActive(true);
        paused = true;
        timer.paused = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !paused)
        {
            player.TogglePause();
            musicPlayer.Pause();
            timer.paused = true;
            pauseText.gameObject.SetActive(true);
            Time.timeScale = 0;
            paused = true;
            
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && paused)
        {
            player.TogglePause();
            musicPlayer.Play();
            timer.paused = false;
            pauseText.gameObject.SetActive(false);
            Time.timeScale = 1;
            paused = false;
        }
    }
}
