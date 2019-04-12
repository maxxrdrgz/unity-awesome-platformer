using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogScript : MonoBehaviour
{
    private Animator anim;
    private bool animation_started;
    private bool animation_finished;

    private int jumpedTimes;
    private bool jumpLeft = true;
    private string frogJumpCoroutine = "FrogJump";
    private Vector3 frogpos;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(frogJumpCoroutine);
    }

    // Update is called once per frame
    void Update()
    {
        if(animation_finished && animation_started)
        {
            animation_started = false;
            print("parent position:");
            print(transform.parent.position);
            print("child position:");
            print(transform.position);

            transform.parent.position = transform.position;
            transform.localPosition = Vector3.zero;

            print("AFTER parent position:");
            print(transform.parent.position);
            print("AFTER child position:");
            print(transform.position);
        }
    }

    IEnumerator FrogJump()
    {
        yield return new WaitForSeconds(Random.Range(1f, 4f));
        animation_started = true;
        animation_finished = false;
        jumpedTimes++;
        if (jumpLeft)
        {
            anim.Play("FrogJumpLeft");
        }
        else
        {
            anim.Play("FrogJumpRight");
        }
        StartCoroutine(frogJumpCoroutine);
    }

    void AnimationFinished()
    {
        animation_finished = true;
        if (jumpLeft)
        {
            anim.Play("FrogIdleLeft");
        }
        else
        {
            anim.Play("FrogIdleRight");
        }
        if (jumpedTimes == 3)
        {
            jumpedTimes = 0;
            Vector3 tempscale = transform.localScale;
            tempscale.x *= -1;
            transform.localScale = tempscale;
            jumpLeft = !jumpLeft;
        }
        print("finished the jump animation");
    }
}
