using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class BossHealth : MonoBehaviour
{
    private Animator anim;
    private int health = 3;
    private bool canDamage;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        canDamage = true;
    }
    
    /**
        The canDamage bool determines whether or not the boss can be damaged.
        Once called, this function will return a delay before setting canDamage
        to true again.
    
        @returns {IEnumerator}
    */
    IEnumerator WaitForDamage()
    {
        yield return new WaitForSeconds(1f);
        canDamage = true;
    }

    /**
        Detects if the boss's collider has collided with the bullet. If so, deal 
        damage. Once boss dies, play sounds and restart game.
    
        @returns {void}
    */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("hit");
        if (canDamage)
        {
            if (collision.gameObject.tag == Tags.BULLET_TAG)
            {
                print("boss hit");
                health--;
                canDamage = false;
                if (health == 0)
                {
                    GetComponent<BossScript>().DeactivateBossScript();
                    anim.Play("BossDead");
                    SoundManager.instance.PlayLevelClearSound();
                    Time.timeScale = 0f;
                    StartCoroutine(RestartGame());
                }
                StartCoroutine(WaitForDamage());
            }
        }
    }

    /**
        Reloads the Gamplay scene after returning from time delay
    
        @returns {IEnumerator}
    */
    IEnumerator RestartGame()
    {
        yield return new WaitForSecondsRealtime(7f);
        SceneManager.LoadScene("Gameplay");
    }
}
