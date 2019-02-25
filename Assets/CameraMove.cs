using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Camera firstPersonCamera, thirdPersonCamera;
    public bool firstPerson = false;

    private void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            firstPerson = !firstPerson;

            firstPersonCamera.gameObject.SetActive(firstPerson);
            thirdPersonCamera.gameObject.SetActive(!firstPerson);
        }
            
    }
}
