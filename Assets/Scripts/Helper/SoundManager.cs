using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public AudioSource bg_source, coin_source, jump_source, boss_source;
    public AudioClip jump, levelClear, gameover, bossShot, coin;

    void Awake()
    {
        MakeInstance();
    }

    void Start()
    {
        bg_source.Play();
    }

    /** 
        Creates a singleton only in the current Scene
    */
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
        jump_source.clip = jump;
        jump_source.Play();
    }

    public void PlayCoinSound()
    {
        coin_source.clip = coin;
        coin_source.Play();
    }

    public void PlayGameOverSound()
    {
        bg_source.Stop();
        bg_source.clip = gameover;
        bg_source.Play();
    }

    public void PlayLevelClearSound()
    {
        bg_source.Stop();
        bg_source.loop = false;
        bg_source.clip = levelClear;
        bg_source.Play();
    }

    public void PlayBossAttackSound()
    {
        boss_source.clip = bossShot;
        boss_source.Play();
    }
}
