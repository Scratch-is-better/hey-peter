using System.Collections;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip jumpClip;
    public AudioClip hitClip;
    public AudioClip attackClip;
    public AudioClip chargeClip;

    private bool jumpPlaying;
    private bool hitPlaying;
    private bool attackPlaying;
    private bool chargePlaying;

    void Start()
    {
        if (!audioSource)
            audioSource = GetComponent<AudioSource>();
    }

    public void PlayJump()
    {
        if (!jumpPlaying && jumpClip)
            StartCoroutine(PlaySoundOnce(jumpClip, () => jumpPlaying = false, "jump"));
    }

    public void PlayHit()
    {
        if (!hitPlaying && hitClip)
            StartCoroutine(PlaySoundOnce(hitClip, () => hitPlaying = false, "hit"));
    }

    public void PlayAttack()
    {
        if (!attackPlaying && attackClip)
            StartCoroutine(PlaySoundOnce(attackClip, () => attackPlaying = false, "attack"));
    }

    public void PlayCharge()
    {
        if (!chargePlaying && chargeClip)
            StartCoroutine(PlaySoundOnce(chargeClip, () => chargePlaying = false, "charge"));
    }

    private IEnumerator PlaySoundOnce(AudioClip clip, System.Action onComplete, string type)
    {
        switch (type)
        {
            case "jump": jumpPlaying = true; break;
            case "hit": hitPlaying = true; break;
            case "attack": attackPlaying = true; break;
            case "charge": chargePlaying = true; break;
        }

        audioSource.clip = clip;
        audioSource.Play();
        yield return new WaitForSeconds(clip.length + 0.05f); 
        onComplete?.Invoke();
    }
}