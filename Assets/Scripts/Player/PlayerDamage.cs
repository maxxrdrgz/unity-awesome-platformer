using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerDamage : MonoBehaviour
{
    public int frame;

    private Animator anim;
    private Transform transform;
    private BoxCollider2D boxCollider;
    private Text lifeText;
    private int lifeScoreCount;
    private bool canDamage;
    private bool dead;
    private bool deathMoveUp = true;


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
        lifeScoreCount = 3;
        lifeText.text = "x" + lifeScoreCount;
        canDamage = true;
    }

    /** 
        Keeps track of how many frames have passed and calls AnimateDeath()
    */
    private void Update()
    {
        AnimateDeath();
        frame++;
    }

    /** 
        Decreases the life score count. If the life score is 0, restarts the
        game, play the game over sound and set the collider to isTrigger. If
        life score is not 0, start coroutine calling WaitForDamage()
    */
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

    /** 
        Sets canDamage bool to true

        @returns {IEnumerator} returns a time delay of 2 seconds
    */
    IEnumerator WaitForDamage()
    {
        yield return new WaitForSeconds(2f);
        canDamage = true;
    }

    /** 
        Loads the Gameplay scene

        @returns {IEnumerator} returns a time delay of 4.26 seconds
    */
    IEnumerator RestartGame()
    {
        yield return new WaitForSecondsRealtime(4.26f);
        SceneManager.LoadScene("Gameplay");
    }

    /** 
        Calls the coruotine to restart the scene, plays the game over sound and
        animates the players death.
    */
    void InstantDeath()
    {
        Time.timeScale = 0f;
        StartCoroutine(RestartGame());
        boxCollider.isTrigger = true;
        anim.Play("PlayerDied");
        dead = true;
        SoundManager.instance.PlayGameOverSound();
    }

    /** 
        While the players death animation is playing, this function will
        move the player up and then down.
    */
    void AnimateDeath()
    {
        if (dead)
        {
            if (deathMoveUp)
            {
                Vector3 temp = transform.position;
                temp.y += 6f * Time.unscaledDeltaTime;
                transform.position = temp;
                if(frame % 26 == 0)
                {
                    deathMoveUp = false;
                }
            }
            else
            {
                Vector3 temp2 = transform.position;
                temp2.y += -6f * Time.unscaledDeltaTime;
                transform.position = temp2;
            }
        }
    }

    /** 
        Detects collision with water or spikes. If true, call InstantDeath()

        @params {Collider2D} The other Collider2D involved in this collision.
    */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == Tags.WATER_TAG || 
           collision.gameObject.tag == Tags.SPIKES_TAG)
        {
            InstantDeath();
        }
    }
}