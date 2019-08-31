using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    private AudioSource _audioSource;

    public static MusicPlayer instance;
    public AudioClip stageMusic;
    public AudioClip bossMusic;
    public AudioClip stageClearMusic;

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
        _audioSource.loop = true;
        _audioSource.clip = stageMusic;
        _audioSource.Play();
    }

    public void switchToBossMusic()
    {
        _audioSource.Stop();
        _audioSource.clip = bossMusic;
        _audioSource.Play();
    }

    public void switchToStageClearMusic()
    {
        _audioSource.Stop();
        _audioSource.loop = false;
        _audioSource.clip = stageClearMusic;
        _audioSource.Play();
    }
}
