using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //Initialize variables

    public int playerId; //1 or 2 - player's id
    public float maxHealth;
    public GameObject attackPrefab; //attack prefab

    public AudioClip[] audioClips; //array of audioclips saved on the player

    Camera cam; //main cam
    GameObject currentAttack; //current attack that is summoned
    AudioSource audioSource;

    float health;

    void Start()
    {
        cam = Camera.main;

        health = maxHealth;

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        //If an attack is on the scene - control it
        if (Input.GetMouseButton(0) && currentAttack != null)
        {
            //mouse position in world coords
            Vector3 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;

            int layerMask = LayerMask.GetMask("Obstacle");

            //raycast between each 2 points of the trail, to detect hittables that were skipped
            RaycastHit2D[] hits = Physics2D.LinecastAll(currentAttack.transform.position, mousePosition, layerMask);
            bool destroyed = false;

            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider != null)
                {
                    //Something was hit
                    if (hit.collider.tag == "Obstacle")
                    {
                        //Obstacle was hit - destroy attack
                        currentAttack.GetComponent<AttackController>().DestroyAttack(true);
                        destroyed = true;
                    }
                }
            }

            //move attack to mouse pos
            if (!destroyed)
                currentAttack.transform.position = new Vector3(mousePosition.x, mousePosition.y, 0);
        }
        //Mouse button up, destroy attack
        else if (Input.GetMouseButtonUp(0) && currentAttack != null)
        {
            //destroy attack, no sound
            currentAttack.GetComponent<AttackController>().DestroyAttack(false);
        }
    }

    //Player got hit
    public void GotHit(int damage)
    {
        health -= damage;
        audioSource.PlayOneShot(audioClips[0]);
    }

    //Player clicked, start an attack
    private void OnMouseDown()
    {
        //If an attack doenst exist already - if mousebuttonup didnt register
        if (currentAttack != null)
            DestroyImmediate(currentAttack);

        //Initialize an attack
        currentAttack = Instantiate(attackPrefab, transform.position, transform.rotation);
        currentAttack.GetComponent<AttackController>().playerId = playerId;
        Vector3 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        attackPrefab.transform.position = mousePosition;
    }
}
