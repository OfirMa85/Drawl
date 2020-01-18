using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //Initialize variables

    public int playerId; //1 or 2 - player's id
    public GameObject attackPrefab; //attack prefab
    public float maxHealth;

    public AudioClip[] audioClips; //array of audioclips saved on the player

    private Camera cam; //main cam
    private GameObject currentAttack; //current attack that is summoned
    private AudioSource audioSource;

    private float health;

    void Start()
    {
        cam = Camera.main;
        health = maxHealth;
        audioSource = GetComponent<AudioSource>();
    }

    //Player got hit
    public void GotHit(int damage)
    {
        audioSource.PlayOneShot(audioClips[0]);
    }

    //Player clicked, start an attack
    private void OnMouseDown()
    {
        //Initialize an attacks
        currentAttack = Instantiate(attackPrefab, transform.parent);
        currentAttack.GetComponent<AttackController>().playerId = playerId;
    }
}
