using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogScript : MonoBehaviour
{
    public LayerMask playerLayer;
    public LayerMask groundLayer;

    private GameObject player;
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

    void Start()
    {
        StartCoroutine(frogJumpCoroutine);
        player = GameObject.FindGameObjectWithTag(Tags.PLAYER_TAG);
    }

    /**
        Detects if touching player. If so, deal damage to player. Checks for
        ground which would mean that the frog has finished it's jump animation.
        This would cause the frogs parent gameobject to update to the frogs
        current transform.position.
    */
    void Update()
    {
        if (Physics2D.OverlapCircle(transform.position, 0.5f, playerLayer))
        {
            player.GetComponent<PlayerDamage>().DealDamage();
        }

        DetectGround();

        if (animation_finished)
        {
            transform.parent.position = transform.position;
            transform.localPosition = Vector3.zero;
        }

    }

    /** 
        Plays the frog's left or right jump animation. Counts the number of
        times the frog has jumped and then recursively starts the 
        frogJumpCoroutine.

        @retuns {IEnumerator} returns random time delay between 1 and 4 seconds
    */
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

    /** 
        Called once the frog's jump animation has completed. Plays either the
        left or right idle animation and resets the jump counter to 0 after
        the frog as jumped 3 times. Then flips the sign of the scale to move
        the frog in the opposite direction.
    */
    void AnimationFinished()
    {
        animation_finished = true;
        animation_started = false;
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
    }

    /** 
        Detected collision with the ground. If the ground is detected and the
        jump animation had been started, call AnimationFinished().
    */
    void DetectGround()
    {
        RaycastHit2D groundDetected = Physics2D.Raycast(gameObject.transform.position, Vector2.down, 0.4f, groundLayer);
        if (groundDetected && animation_started)
        {
            AnimationFinished();
        }
    }
}
