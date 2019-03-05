using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MenuManager : MonoBehaviour
{
    public Camera controlCamera;
    public Camera menuCamera;

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
}

