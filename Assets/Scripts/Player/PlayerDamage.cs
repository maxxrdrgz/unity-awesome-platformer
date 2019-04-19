using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerDamage : MonoBehaviour
{
    public AudioClip playerDeadSound;
    private AudioSource audioManager;
    private Text lifeText;
    private int lifeScoreCount;
    private bool canDamage;


    void Start()
    {
        Time.timeScale = 1f;
    }

    void Awake()
    {
        lifeText = GameObject.Find("LifeText").GetComponent<Text>();
        lifeScoreCount = 1;
        lifeText.text = "x" + lifeScoreCount;
        canDamage = true;
        audioManager = GetComponent<AudioSource>();
    }

    public void DealDamage()
    {
        if (canDamage)
        {
            lifeScoreCount--;
            if (lifeScoreCount >= 0)
            {
                lifeText.text = "x" + lifeScoreCount;
            }

            if (lifeScoreCount == 0)
            {
                Time.timeScale = 0f;
                StartCoroutine(RestartGame());
                audioManager.PlayOneShot(playerDeadSound);
            }
            canDamage = false;
            StartCoroutine(WaitForDamage());
        }

    }

    IEnumerator WaitForDamage()
    {
        yield return new WaitForSeconds(2f);
        canDamage = true;
    }

    IEnumerator RestartGame()
    {
        yield return new WaitForSecondsRealtime(4.26f);
        SceneManager.LoadScene("Gameplay");
    }
}