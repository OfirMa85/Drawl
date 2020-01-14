using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentSpin : MonoBehaviour
{
    public float spinRotation;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, spinRotation * Time.deltaTime);
    }
}
