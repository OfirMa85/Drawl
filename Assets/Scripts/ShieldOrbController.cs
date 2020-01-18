using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldOrbController : MonoBehaviour
{
    public float shieldOffset;
    public float rotationSpeed;

    public float rotation;

    public Sprite[] sprites;

    private int state;
    //0 - full
    //1 - hit
    //2 - almost destroyed

    private void Start()
    {
        state = 0;
    }

    void Update()
    {
        transform.position = transform.parent.position + new Vector3(Mathf.Sin(Mathf.Deg2Rad * rotation) * shieldOffset, Mathf.Cos(Mathf.Deg2Rad * rotation) * shieldOffset);
        rotation += rotationSpeed;
    }

    //Deal damage to the shield orb, and destroy if got to it's last state
    //Returns the leftover damage
    public int GotHit(int damage)
    {
        state += damage;
        if (state > 2)
        {
            DestroyImmediate(gameObject);
        }
        else
            GetComponent<SpriteRenderer>().sprite = sprites[state];
        return Mathf.Max(state - 3, 0);
    }
}
