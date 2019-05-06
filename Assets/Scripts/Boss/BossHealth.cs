using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if (canDamage)
        {
            if (collision.tag == Tags.BULLET_TAG)
            {
                print("boss hit");
                health--;
                canDamage = false;
                if (health == 0)
                {
                    GetComponent<BossScript>().DeactivateBossScript();
                    anim.Play("BossDead");
                }
                StartCoroutine(WaitForDamage());
            }
        }
    }
}
