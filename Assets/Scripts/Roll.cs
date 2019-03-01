using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roll : MonoBehaviour
{
    Vector3 rollValue;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float manualRoll = 1.5f * Input.GetAxis("Roll");
        rollValue = new Vector3(0, 0, manualRoll);
        transform.localEulerAngles = transform.localEulerAngles - rollValue;
    }
}
