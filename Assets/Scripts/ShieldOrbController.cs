using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldOrbController : MonoBehaviour
{
    public float shieldOffset;
    public float rotationSpeed;

    public float rotation;

    void Update()
    {
        transform.position = transform.parent.position + new Vector3(Mathf.Sin(Mathf.Deg2Rad * rotation) * shieldOffset, Mathf.Cos(Mathf.Deg2Rad * rotation) * shieldOffset);
        rotation += rotationSpeed;
    }
}
