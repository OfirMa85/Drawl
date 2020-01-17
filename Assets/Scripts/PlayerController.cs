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
    public GameObject shieldOrbPrefab; //shield orb prefab
    public float damage;
    public int maxShieldOrbs;
    public float shieldOffset;
    public float shieldRotationSpeed;

    public AudioClip[] audioClips; //array of audioclips saved on the player

    private Camera cam; //main cam
    private GameObject currentAttack; //current attack that is summoned
    private GameObject shield; //shield orbs parent
    private AudioSource audioSource;

    private float health;
    private int shieldOrbs;

    void Start()
    {
        cam = Camera.main;
        health = maxHealth;
        shieldOrbs = maxShieldOrbs;
        audioSource = GetComponent<AudioSource>();

        CreateShield();
    }

    void Update()
    {
        AttackControl();
    }

    private void CreateShield()
    {
        shield = new GameObject("Shield");
        shield.transform.parent = transform.parent;
        shield.transform.localPosition = Vector2.zero;

        for (int i = 0; i < maxShieldOrbs; i++)
        {
            GameObject shieldOrbGO = Instantiate(shieldOrbPrefab, shield.transform);
            shieldOrbGO.transform.position = transform.position + new Vector3(Mathf.Sin(Mathf.Deg2Rad * (float)i * (360 / maxShieldOrbs)) * shieldOffset, Mathf.Cos(Mathf.Deg2Rad * (float)i * (360 / maxShieldOrbs)) * shieldOffset);
            ShieldOrbController shieldOrbController = shieldOrbGO.GetComponent<ShieldOrbController>();
            shieldOrbController.shieldOffset = shieldOffset;
            shieldOrbController.rotationSpeed = shieldRotationSpeed;
            shieldOrbController.rotation = ((float)i * (360 / maxShieldOrbs));
        }
    }

    private void AttackControl()
    {
        //If an attack is on the scene - control it
        if (Input.GetMouseButton(0) && currentAttack != null)
        {
            //mouse position in world coords
            Vector3 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;

            int layerMask = LayerMask.GetMask("Hittable");

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
                    else if (hit.collider.tag == "Player")
                    {
                        if (hit.collider.gameObject != gameObject)
                        {
                            hit.collider.GetComponent<PlayerController>().GotHit(damage);
                            currentAttack.GetComponent<AttackController>().DestroyAttack(false);
                            destroyed = true;
                        }
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
    public void GotHit(float damage)
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
