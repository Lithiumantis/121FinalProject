using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class Movement : MonoBehaviour
{
    [System.NonSerialized]
    public bool paused = false;
    public bool advancedControls = false;
    public bool invertPitchControls = true;
    public float mouseSensitivity = 1;

    public float maxRoll = 90f;
    public float baseSpeed = 75f;
    public float thrust = 1.5f;
    public float thrustRestingPoint = 1.5f;
    public float maxthrust = 5f;
    public float minthrust = 0.25f;

    //components
    public Text altitudeText;
    public Text thrustText;

    public GameObject explosionPrefab;

    //vfx and sfx 
    protected Rigidbody rb;
    protected Vignette vignette;
    public PostProcessProfile postProcessProfile;
    protected AudioSource audioSource;

    //for getting mouse position on screen
    protected readonly float centerOfScreenX = Screen.width / 2;
    protected readonly float centerOfScreenY = Screen.height / 2;
    protected float mouseX;
    protected float mouseY;

    Vector3 rotateValue;
    Vector3 rollValue;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        vignette = postProcessProfile.GetSetting<Vignette>();
        vignette.intensity.value = 0.3f;
        audioSource = GetComponent<AudioSource>();
        audioSource.Pause();
    }

    void FixedUpdate()
    {
        if (!advancedControls) StandardMovement();
        else AdvancedMovement();

    }

    protected void StandardMovement()
    {
        //Debug.Log(GetMousePercentageX());
        if (!paused)
        {
            //movement variables
            float x; float y; float z;


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

            rotateValue = new Vector3(0, x * -1.5f, 0); //yaw
            transform.localEulerAngles = transform.localEulerAngles - rotateValue;

            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, maxRoll * GetMousePercentageX()); //roll

            transform.Rotate(2 * y, 0, 0, Space.Self); // pitch

            //forward thrust
            rb.MovePosition(transform.position + transform.forward * baseSpeed * thrust * Time.deltaTime);

            UpdateUI();
        }
    }

    protected void AdvancedMovement()
    {
        //Debug.Log(GetMousePercentageX());
        if (!paused)
        {
            //movement variables
            float x; float y; float z;


            //mouse input
            GetMousePosNormalized();
            y = mouseY;
            x = mouseX;

            // GET ROLL
            float manualRoll = 0.75f * Input.GetAxis("Roll");

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
            
            rotateValue = new Vector3(0, x * -1.5f, 2*z); //(0, yaw, roll)
            transform.localEulerAngles = transform.localEulerAngles - rotateValue;
            transform.Rotate(2*y, 0, 0, Space.Self); // pitch
            /*
            rotateValue = new Vector3(0, x * -1.5f, 0); //yaw
            transform.localEulerAngles = transform.localEulerAngles - rotateValue;
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, maxRoll * GetMousePercentageX()); //roll
            transform.Rotate(2 * y, 0, 0, Space.Self); // pitch
            */

            //forward thrust
            rb.MovePosition(transform.position + transform.forward * baseSpeed * thrust * Time.deltaTime);

            UpdateUI();
        }
    }

    protected void GetMousePosNormalized()
    {
        mouseY = 4f * (Input.mousePosition.y - centerOfScreenY) / Screen.height;
        mouseX = 4f * (Input.mousePosition.x - centerOfScreenX) / Screen.width;

        //Debug.Log(Input.mousePosition.x + "/" + Screen.width);
    }

    protected float GetMousePercentageX()
    {
        float centerPoint = Screen.width / 2; //e.g. 1920/2

        float mousePosX = Input.mousePosition.x;
        float relativeMousePosX = Input.mousePosition.x - centerPoint;
        float mousePercent = relativeMousePosX / centerPoint;

        return mousePercent * -1 * mouseSensitivity;
    }

    protected void UpdateUI()
    {
        altitudeText.text = "ALT\n" + transform.position.y;
        thrustText.text = "SPEED\n" + thrust * 1000f;
        audioSource.volume = (thrust + 0.5f) / maxthrust;
        audioSource.pitch = (thrust + 0.5f) / maxthrust;
        
    }

    public void TogglePause()
    {
        if (paused)
        {
            Debug.Log("Unpausing");
            audioSource.Play();
            paused = false;
        }
        else
        {
            Debug.Log("Pausing");
            audioSource.Pause();
            paused = true;
        }
    }


    //lol you crashed into the ground
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Terrain")
        {
            Debug.Log("Crash");
            //vignette.intensity.value = 1;
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }

    }

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("Hit by particle");
    }
}
