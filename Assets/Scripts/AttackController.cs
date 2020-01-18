using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    public int playerId;
    public AudioClip clip;
    public float maxTime;
    public int maxDamage;
    public float damageDropoffMultiplier;

    int damage;

    private Camera cam;
    private float lifeTime;

    private void Start()
    {
        damage = maxDamage;
        cam = Camera.main;

        Vector3 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        transform.position = mousePosition;
    }

    public void DestroyAttack(bool playOOF)
    {
        Destroy(gameObject);
        if (playOOF)
            AudioSource.PlayClipAtPoint(clip, transform.position);

    }

    private void FixedUpdate()
    {
        AttackControl();
        lifeTime++;
        if (lifeTime >= maxTime)
            DestroyAttack(false);
    }

    private void AttackControl()
    {
        //If an attack is on the scene - control it
        if (Input.GetMouseButton(0))
        {
            //mouse position in world coords
            Vector3 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;

            int layerMask = LayerMask.GetMask("Hittable");

            //raycast between each 2 points of the trail, to detect hittables that were skipped
            RaycastHit2D[] hits = Physics2D.LinecastAll(transform.position, mousePosition, layerMask);

            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider != null)
                {
                    //Something was hit
                    if (hit.collider.tag == "Obstacle")
                    {
                        //Obstacle was hit - destroy attack
                        DestroyAttack(true);
                    }
                    else if (hit.collider.tag == "Player")
                    {
                        CalculateDamage();
                        //PLayer was hit
                        if (hit.collider.GetComponent<PlayerController>().playerId != playerId)
                        {
                            //Debug.Log(damage + " damage");
                            hit.collider.GetComponent<PlayerController>().GotHit(damage);
                            DestroyAttack(false);
                        }
                    }
                }
            }

            transform.position = new Vector3(mousePosition.x, mousePosition.y, 0);
        }
        //Mouse button up, destroy attack
        else if (Input.GetMouseButtonUp(0))
        {
            //destroy attack, no sound
            DestroyAttack(false);
        }
    }
    private void CalculateDamage()
    {
        damage = Mathf.Max((int)((float)maxDamage * Mathf.Pow(1 - lifeTime/maxTime, damageDropoffMultiplier)), 1);
    }
}
