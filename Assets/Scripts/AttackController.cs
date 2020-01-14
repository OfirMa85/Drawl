using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    public int playerId;
    public int damage;
    public AudioClip clip;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (collision.GetComponent<PlayerController>().playerId != playerId)
            {
                collision.GetComponent<PlayerController>().GotHit(damage);
                DestroyAttack(false);
            }
        }
    }

    public void DestroyAttack(bool playOOF)
    {
        Destroy(gameObject);
        if (playOOF)
            AudioSource.PlayClipAtPoint(clip, transform.position);

    }
}
