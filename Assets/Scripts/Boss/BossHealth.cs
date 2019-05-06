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

    IEnumerator WaitForDamage()
    {
        yield return new WaitForSeconds(1f);
        canDamage = true;
    }

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

    IEnumerator RestartGame()
    {
        yield return new WaitForSecondsRealtime(7f);
        SceneManager.LoadScene("Gameplay");
    }
}
