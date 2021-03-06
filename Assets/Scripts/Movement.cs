﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{
    [System.NonSerialized]
    public bool paused = false;
    private bool crashed = false;
    public bool advancedControls = false;
    public bool invertPitchControls = true;
    public float mouseSensitivity = 1;

    public float maxRoll = 90f;
    public float baseSpeed = 75f;
    public float thrust = 1.5f;
    public float thrustRestingPoint = 1.5f;
    public float maxthrust = 5f;
    public float minthrust = 0.25f;
    public int maxHealth = 5;
    private int health = 5;

    //components
    public Text altitudeText;
    public Text thrustText;
    public Slider hpSlider;

    public GameObject explosionPrefab;
    public AudioClip explosionSound;
    public AudioSource globalHitPlayer;

    //vfx and sfx 
    protected Rigidbody rb;
    protected Vignette vignette;
    public PostProcessProfile postProcessProfile;
    protected AudioSource audioSource;
    Shoot shooter;

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
        shooter = GetComponentInChildren<Shoot>();
        audioSource.Pause();

        health = maxHealth;

    }

    private void Awake()
    {
        //get y invert
        GameObject settingsObject = GameObject.Find("Settings");

        if(settingsObject != null)
        {
            Settings settings = settingsObject.GetComponent<Settings>();

            if (settings != null)
                invertPitchControls = settings.invertY;

            Destroy(settingsObject);
        }
    }

    void FixedUpdate()
    {
        if (!advancedControls) StandardMovement();

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

            //Debug.Log(transform.localEulerAngles.x);
            
            if(transform.localEulerAngles.x <= 85)
                transform.Rotate(2 * y, 0, 0, Space.Self); // pitch
            else if (transform.localEulerAngles.x >= 275)
                transform.Rotate(2 * y, 0, 0, Space.Self); // pitch
            else if(transform.localEulerAngles.x <= 275 && transform.localEulerAngles.x >= 180 && y >= 0)
                transform.Rotate(2 * y, 0, 0, Space.Self); // pitch
            else if (transform.localEulerAngles.x >= 85 && transform.localEulerAngles.x <= 180 && y <= 0)
                transform.Rotate(2 * y, 0, 0, Space.Self); // pitch
                


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
        if(altitudeText != null)
            altitudeText.text = "ALT\n" + transform.position.y;
        if (thrustText != null)
            thrustText.text = "SPEED\n" + thrust * 1000f;
        audioSource.volume = (thrust + 0.5f) / maxthrust;
        audioSource.pitch = (thrust + 0.5f) / maxthrust;
        
    }

    public void TogglePause()
    {
        if (paused)
        {
            Debug.Log("Unpausing");
            if(!crashed)
                audioSource.Play();
            shooter.paused = false;
            paused = false;


        }
        else
        {
            Debug.Log("Pausing");
            if (!crashed)
                audioSource.Pause();
            shooter.paused = true;
            paused = true;
        }
    }


    //lol you crashed into the ground
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Terrain")
        {
            Explode();
        }

    }

    private void OnParticleCollision(GameObject other)
    {
        health--;
        hpSlider.value = health;

        if (health > 0)
        {
            globalHitPlayer.Play();
        }
        if(health <= 0)
        {
            Explode();
        }
    }

    private void Explode()
    {
        if (!crashed)
        {
            hpSlider.value = 0;
            crashed = true;


            //vignette.intensity.value = 1;
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            TogglePause();

            MeshRenderer mr = GetComponent<MeshRenderer>();
            mr.enabled = false;

            audioSource.clip = explosionSound;
            audioSource.loop = false;
            audioSource.Play();

            StartCoroutine(LoadAfterDelay());
        }

    }

    private IEnumerator LoadAfterDelay()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("Menu");

    }


}
