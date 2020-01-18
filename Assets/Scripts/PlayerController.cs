using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //Initialize variables

    public int playerId; //1 or 2 - player's id
    public GameObject attackPrefab; //attack prefab
    public GameObject shieldOrbPrefab; //shield orb prefab
    public int maxShieldOrbs;
    public float shieldOffset;
    public float shieldRotationSpeed;

    public AudioClip[] audioClips; //array of audioclips saved on the player

    private Camera cam; //main cam
    private GameObject currentAttack; //current attack that is summoned
    private GameObject shield; //shield orbs parent
    private AudioSource audioSource;

    private int shieldOrbs;

    void Start()
    {
        cam = Camera.main;
        shieldOrbs = maxShieldOrbs;
        audioSource = GetComponent<AudioSource>();

        CreateShield();
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

    //Player got hit
    public void GotHit(int damage)
    {
        audioSource.PlayOneShot(audioClips[0]);
        DamageShieldOrbs(damage);
    }

    public void DamageShieldOrbs(int damage)
    {
        int leftoverDamage = 0;
        if (shield.transform.childCount > 0)
            leftoverDamage = shield.transform.GetChild(0).GetComponent<ShieldOrbController>().GotHit(damage);
        if (shield.transform.childCount > 0 && leftoverDamage > 0)
            shield.transform.GetChild(0).GetComponent<ShieldOrbController>().GotHit(leftoverDamage);
    }

    //Player clicked, start an attack
    private void OnMouseDown()
    {

        //Initialize an attacks
        currentAttack = Instantiate(attackPrefab, transform.parent);
        currentAttack.GetComponent<AttackController>().playerId = playerId;
    }
}
