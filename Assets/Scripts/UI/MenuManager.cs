using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MenuManager : MonoBehaviour
{
    public Camera controlCamera;
    public Camera menuCamera;
    public Settings settings;

    public Image invertYImage;
    private bool yToggled = false;
    public AudioSource clickAudio;

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ShowControls()
    {
        menuCamera.gameObject.SetActive(false);
        controlCamera.gameObject.SetActive(true);
    }

    public void ShowMenu()
    {
        menuCamera.gameObject.SetActive(true);
        controlCamera.gameObject.SetActive(false);
    }

    public void ToggleY()
    {
        if (!yToggled)
        {
            settings.invertY = true;
            invertYImage.color = Color.red;
            yToggled = true;
        }
        else
        {
            settings.invertY = false;
            invertYImage.color = Color.white;
            yToggled = false;
        }
    }

    public void OnClick()
    {
        clickAudio.Play();
    }
}

