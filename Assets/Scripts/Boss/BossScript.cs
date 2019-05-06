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

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(coroutine_Name);
    }

    void Attack()
    {
        GameObject stoneObj = Instantiate(stone, attackInstantiate.position, Quaternion.identity);
        stoneObj.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-300f, -700f), 0f));
    }

    void BackToIdle()
    {
        anim.Play("BossIdle");
    }

    public void DeactivateBossScript()
    {
        StopCoroutine(coroutine_Name);
        enabled = false;
    }

    IEnumerator StartAttack()
    {
        yield return new WaitForSeconds(Random.Range(1f, 3f));
        anim.Play("BossAttack");
        SoundManager.instance.PlayBossAttackSound();
        StartCoroutine(coroutine_Name);
    }
}
