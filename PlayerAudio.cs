using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip jumpClip;
    public AudioClip hitClip;
    public AudioClip attackClip;
    public AudioClip chargeClip;

    void Start()
    {
        if (!audioSource) audioSource = GetComponent<AudioSource>();
    }

    public void PlayJump() => audioSource?.PlayOneShot(jumpClip);
    public void PlayHit() => audioSource?.PlayOneShot(hitClip);
    public void PlayAttack() => audioSource?.PlayOneShot(attackClip);
    public void PlayCharge() => audioSource?.PlayOneShot(chargeClip);
}
