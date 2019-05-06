using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerDamage : MonoBehaviour
{
    private Animator anim;
    private Transform transform;
    private BoxCollider2D boxCollider;
    private Text lifeText;
    private int lifeScoreCount;
    private bool canDamage;
    private bool dead;


    void Start()
    {
        Time.timeScale = 1f;
    }

    void Awake()
    {
        lifeText = GameObject.Find("LifeText").GetComponent<Text>();
        transform = gameObject.GetComponent<Transform>();
        anim = gameObject.GetComponent<Animator>();
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
        lifeScoreCount = 1;
        lifeText.text = "x" + lifeScoreCount;
        canDamage = true;
    }

    private void Update()
    {
        CheckIfDead();
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
                boxCollider.isTrigger = true;
                anim.Play("PlayerDied");
                dead = true;
                SoundManager.instance.PlayGameOverSound();
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

    void InstantDeath()
    {
        Time.timeScale = 0f;
        StartCoroutine(RestartGame());
        boxCollider.isTrigger = true;
        anim.Play("PlayerDied");
        dead = true;
        SoundManager.instance.PlayGameOverSound();
    }

    void CheckIfDead()
    {
        if (dead)
        {
            Vector3 temp = transform.position;
            temp.y += -6f * Time.unscaledDeltaTime;
            transform.position = temp;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == Tags.WATER_TAG)
        {
            InstantDeath();
        }
    }
}