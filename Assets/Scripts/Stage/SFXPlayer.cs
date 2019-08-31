using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlayer : MonoBehaviour
{
    private AudioSource _audioSource;
    private AudioClip chosen_clip = null;

    public static SFXPlayer instance;

    // Enemy sounds
    public AudioClip[] enemyFires;
    public AudioClip[] enemyHitInvulnerable;
    public AudioClip[] enemyHit;
    public AudioClip[] enemyDying;

    // Megaman sounds
    public AudioClip megamanShot;
    public AudioClip megamanJump;
    public AudioClip megamanHurt;
    public AudioClip megamanDeath;
    public AudioClip megamanVictory;
    public AudioClip megamanTeleport;

    // Gutsman sounds
    public AudioClip gutsmanRockThrown;
    public AudioClip gutsmanLanding;
    public AudioClip gutsmanDamage;
    public AudioClip gutsmanDeath;

    // Objects
    public AudioClip onPlatform;
    public AudioClip doorOpen;
    public AudioClip doorClose;
    public AudioClip pickEnergy;
    public AudioClip pickLife;
    public AudioClip pickPoints;

    private int _chance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void Play(string clip)
    {
        switch (clip)
        {
            case "enemyFires":
                _chance = Random.Range(0, enemyFires.Length);
                chosen_clip = enemyFires[_chance];
                break;
            case "enemyHitInvulnerable":
                _chance = Random.Range(0, enemyHitInvulnerable.Length);
                chosen_clip = enemyHitInvulnerable[_chance];
                break;
            case "enemyHit":
                _chance = Random.Range(0, enemyHit.Length);
                chosen_clip = enemyHit[_chance];
                break;
            case "enemyDying":
                _chance = Random.Range(0, enemyDying.Length);
                chosen_clip = enemyDying[_chance];
                break;
            case "megamanShot":
                chosen_clip = megamanShot;
                break;
            case "megamanJump":
                chosen_clip = megamanJump;
                break;
            case "megamanHurt":
                chosen_clip = megamanHurt;
                break;
            case "megamanDeath":
                chosen_clip = megamanDeath;
                break;
            case "megamanVictory":
                chosen_clip = megamanVictory;
                break;
            case "megamanTeleport":
                chosen_clip = megamanTeleport;
                break;
            case "gutsmanRockThrown":
                chosen_clip = gutsmanRockThrown;
                break;
            case "gutsmanLanding":
                chosen_clip = gutsmanLanding;
                break;
            case "gutsmanDamage":
                chosen_clip = gutsmanDamage;
                break;
            case "gutsmanDeath":
                chosen_clip = gutsmanDeath;
                break;
            case "pickEnergy":
                chosen_clip = pickEnergy;
                break;
            case "pickLife":
                chosen_clip = pickLife;
                break;
            case "pickPoints":
                chosen_clip = pickPoints;
                break;
            case "doorOpen":
                chosen_clip = doorOpen;
                break;
            case "doorClose":
                chosen_clip = doorClose;
                break;
            case "onPlatform":
                chosen_clip = onPlatform;
                break;
        }
        _audioSource.pitch = Random.Range(0.9f, 1.1f);
        _audioSource.volume = 1;
        // Special cases
        if (chosen_clip == onPlatform)
        {
            StartCoroutine(loopingSFX());
        } else
        {
            _audioSource.PlayOneShot(chosen_clip);
        }
        if (chosen_clip == gutsmanDeath)
        {
            StartCoroutine(fadeOutSound());
        }
    }

    public void StopLoopSFX()
    {
        StopCoroutine(loopingSFX());
        _audioSource.Stop();
    }

    IEnumerator fadeOutSound()
    {
        float fadeTime = 0.7f;
        yield return new WaitForSeconds(3f);
        while (_audioSource.volume > 0)
        {
            _audioSource.volume -= Time.deltaTime / fadeTime;
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator loopingSFX()
    {
        _audioSource.loop = true;
        _audioSource.clip = chosen_clip;
        _audioSource.Play();
        yield return null;
    }
}
