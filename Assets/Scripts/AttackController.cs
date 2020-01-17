using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    public int playerId;
    public AudioClip clip;

    public void DestroyAttack(bool playOOF)
    {
        Destroy(gameObject);
        if (playOOF)
            AudioSource.PlayClipAtPoint(clip, transform.position);

    }
}
