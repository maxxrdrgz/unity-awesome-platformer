using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public AudioSource bg_source, general_audio_source;
    public AudioClip jump, levelClear, gameover, bossShot, coin;

    void Awake()
    {
        MakeInstance();
    }

    void Start()
    {
        bg_source.Play();
    }

    void MakeInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }
    }

    public void PlayJumpSound()
    {
        general_audio_source.clip = jump;
        general_audio_source.Play();
    }

    public void PlayCoinSound()
    {
        general_audio_source.clip = coin;
        general_audio_source.Play();
    }

    public void PlayGameOverSound()
    {
        bg_source.Stop();
        general_audio_source.clip = gameover;
        general_audio_source.Play();
    }

    public void PlayLevelClearSound()
    {
        bg_source.Stop();
        general_audio_source.clip = levelClear;
        general_audio_source.Play();
    }

    public void PlayBossAttackSound()
    {
        general_audio_source.clip = bossShot;
        general_audio_source.Play();
    }
}
