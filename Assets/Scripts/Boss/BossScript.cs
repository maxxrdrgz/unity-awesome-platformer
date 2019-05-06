using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    public GameObject stone;
    public Transform attackInstantiate;

    private Animator anim;
    private string coroutine_Name = "StartAttack";

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    /**
        Starts the StartAttack coroutine
    */
    void Start()
    {
        StartCoroutine(coroutine_Name);
    }

    /**
        Instantiates the stone gameobject and launches it in the negative x,y direction
    */
    void Attack()
    {
        GameObject stoneObj = Instantiate(stone, attackInstantiate.position, Quaternion.identity);
        stoneObj.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-300f, -700f), 0f));
    }

    /**
        Plays the boss's idle animation
    */
    void BackToIdle()
    {
        anim.Play("BossIdle");
    }

    /**
        Stops the StartAttack coroutine and tells the gameobject to disable this script
    */
    public void DeactivateBossScript()
    {
        StopCoroutine(coroutine_Name);
        enabled = false;
    }

    /**
        Plays the boss's attack animation, and attack sound. Recursively starts it's
        coroutine again to keep attacking.
        
        @returns {IEnumerator} returns a random time delay between 1.5 and 4 seconds
    */
    IEnumerator StartAttack()
    {
        yield return new WaitForSeconds(Random.Range(1.5f, 4f));
        anim.Play("BossAttack");
        SoundManager.instance.PlayBossAttackSound();
        StartCoroutine(coroutine_Name);
    }
}
