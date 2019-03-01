using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class AltMovement : Movement
{
 
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        vignette = postProcessProfile.GetSetting<Vignette>();
        vignette.intensity.value = 0.3f;
        audioSource = GetComponent<AudioSource>();

    }

    void FixedUpdate()
    {
        if (!paused)
        {
            //movement variables
            float x; float y; float z;
            Vector3 rotateValue;

            //mouse input
            GetMousePosNormalized();
            y = mouseY;
            x = mouseX;

            // GET ROLL
            float manualRoll = 1.5f * Input.GetAxis("Roll");

            //Case 1: Within acceptable zones for mouse movement
            if (transform.eulerAngles.z < maxRoll || transform.eulerAngles.z > (360 - maxRoll))
            {
                z = 2 * Input.GetAxis("Mouse X") + manualRoll;
            }
            //Case 2: Too far to the right, but moving left
            else if ((transform.eulerAngles.z > 180f && transform.eulerAngles.z < (360 - maxRoll)) && Input.GetAxis("Mouse X") <= 0f)
            {
                z = 2 * Input.GetAxis("Mouse X") + manualRoll;
            }
            //Case 3: Too far to the left, but moving right
            else if ((transform.eulerAngles.z > maxRoll && transform.eulerAngles.z < 180f) && Input.GetAxis("Mouse X") >= 0f)
            {
                z = 2 * Input.GetAxis("Mouse X") + manualRoll;
            }
            else
            {
                z = 0 + manualRoll;
            }

            // GET THRUST, RESET TO REST POINT IF NO INPUT
            if (Input.GetAxis("Vertical") != 0)
            {
                thrust += Input.GetAxis("Vertical") * 0.07f;
                if (thrust < minthrust) thrust = minthrust;
                if (thrust > maxthrust) thrust = maxthrust;
            }
            else if (Input.GetAxis("Vertical") == 0 && thrust > thrustRestingPoint)
            {
                thrust -= 0.05f;
                if (thrust < thrustRestingPoint)
                {
                    thrust = thrustRestingPoint;
                }
            }
            else if (Input.GetAxis("Vertical") == 0 && thrust < thrustRestingPoint)
            {
                thrust += 0.05f;
                if (thrust > thrustRestingPoint)
                {
                    thrust = thrustRestingPoint;
                }
            }


            if (!invertPitchControls)
            {
                y = -y;
            }

            //rotation
            rotateValue = new Vector3(0, x * -1.5f, z);
            transform.localEulerAngles = transform.localEulerAngles - rotateValue;
            transform.Rotate(2*y, 0, 0, Space.Self);

            //forward thrust
            rb.MovePosition(transform.position + transform.forward * baseSpeed * thrust * Time.deltaTime);

            UpdateUI();
        }

    }



    //lol you crashed into the ground
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Terrain")
        {
            Debug.Log("Crash");
            vignette.intensity.value = 1;
        }
    }
}
