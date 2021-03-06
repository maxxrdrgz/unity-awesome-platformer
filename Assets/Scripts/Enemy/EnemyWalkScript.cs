﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWalkScript : MonoBehaviour
{
    public float moveSpeed = 1f;
    public Transform left_Collision, right_Collision, top_Collision, down_Collision;
    public LayerMask playerLayer;
    public LayerMask obstacleLayer;

    private Rigidbody2D rbody;
    private Animator anim;
    private SpriteRenderer sr;
    private bool moveLeft;
    private bool canMove;
    private bool stunned;
    private Vector3 left_collision_pos, right_collision_pos;

    private void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        left_collision_pos = left_Collision.localPosition;
        right_collision_pos = right_Collision.localPosition;
    }

    // Start is called before the first frame update
    void Start()
    {
        moveLeft = true;
        sr.flipX = false;
        canMove = true;

    }

    /** 
        Sets the enemie's rigid body's velocity to move in either the left
        or right direction. Also checks for collision.
    */
    void Update()
    {
        if (canMove)
        {
            if (moveLeft)
            {
                rbody.velocity = new Vector2(-moveSpeed, rbody.velocity.y);
            }
            else
            {
                rbody.velocity = new Vector2(moveSpeed, rbody.velocity.y);
            }
        }

        CheckCollision();
    }

    /** 
        Checks for collisions on either the left, right or top of the gameobject.
        If a collision is detected from the top from the player, the enemy
        gameobject will will be in a stunned state where it can no longer move
        and start the Dead coroutine.

        Detects collision from left or right side. If collision is with the
        player, deals damage to the player. If in the stunned state and the
        gameobject is the snail, the snail will be pushed in the opposite
        direction of the collision. If the collision is with another walking
        enemy type or a block, the enemy will change directions.

        Detects collision downwards with the ground. If no ground is detected,
        gameobject changes direction.

    */
    void CheckCollision()
    {
        RaycastHit2D leftHit = Physics2D.Raycast(
                                    left_Collision.position, 
                                    Vector2.left, 
                                    0.1f, 
                                    obstacleLayer
                                );
        RaycastHit2D rightHit = Physics2D.Raycast(right_Collision.position, Vector2.right, 0.1f, obstacleLayer);

        Collider2D topHit = Physics2D.OverlapCircle(top_Collision.position, 0.3f, playerLayer);

        if (topHit != null)
        {
            if (topHit.gameObject.tag == Tags.PLAYER_TAG)
            {
                if (!stunned)
                {
                    topHit.gameObject.GetComponent<Rigidbody2D>().velocity =
                        new Vector2(topHit.gameObject.GetComponent<Rigidbody2D>().velocity.x, 7f);
                    canMove = false;
                    rbody.velocity = new Vector2(0, 0);
                    anim.Play("Stunned");
                    stunned = true;
                    // beetle code here
                    if (tag == Tags.BEETLE_TAG)
                    {
                        anim.Play("Stunned");
                        StartCoroutine(Dead(0.5f));
                    }
                }
            }
        }

        if (leftHit)
        {

            if (leftHit.collider.gameObject.tag == Tags.PLAYER_TAG)
            {
                if (!stunned)
                {
                    leftHit.collider.gameObject.GetComponent<PlayerDamage>().DealDamage();
                }
                else
                {
                    if (tag == Tags.SNAIL_TAG)
                    {
                        rbody.velocity = new Vector2(15f, rbody.velocity.y);
                        StartCoroutine(Dead(3f));
                    }
                }
            }
            else if (leftHit.collider.gameObject.tag == Tags.BEETLE_TAG ||
                leftHit.collider.gameObject.tag == Tags.SNAIL_TAG ||
                leftHit.collider.gameObject.tag == Tags.BLOCK_TAG)
            {
                ChangeDirection();
            }
        }


        if (rightHit)
        {
            if (rightHit.collider.gameObject.tag == Tags.PLAYER_TAG)
            {
                if (!stunned)
                {
                    rightHit.collider.gameObject.GetComponent<PlayerDamage>().DealDamage();
                }
                else
                {
                    if (tag == Tags.SNAIL_TAG)
                    {
                        rbody.velocity = new Vector2(-15f, rbody.velocity.y);
                        StartCoroutine(Dead(3f));
                    }
                }
            }
            else if (rightHit.collider.gameObject.tag == Tags.BEETLE_TAG ||
                rightHit.collider.gameObject.tag == Tags.SNAIL_TAG ||
                rightHit.collider.gameObject.tag == Tags.BLOCK_TAG)
            {
                ChangeDirection();
            }
        }
        if (!Physics2D.Raycast(down_Collision.position, Vector2.down, 0.1f, obstacleLayer))
        {
            ChangeDirection();
        }
    }

    /** 
        Flips the sign on the scale of the gameobject to mirror it in the
        opposite direction. Also changes the left and right colliders as well.
    */
    void ChangeDirection()
    {
        moveLeft = !moveLeft;
        Vector3 tempScale = transform.localScale;

        if (moveLeft)
        {
            tempScale.x = Mathf.Abs(tempScale.x);
            left_Collision.localPosition = left_collision_pos;
            right_Collision.localPosition = right_collision_pos;
        }
        else
        {
            tempScale.x = -Mathf.Abs(tempScale.x);
            left_Collision.localPosition = right_collision_pos;
            right_Collision.localPosition = left_collision_pos;
        }
        transform.localScale = tempScale;
    }

    /** 
        Disables the gameobjec this script is attached (enemy) to after
        specified time delay.

        @param {float} The amount of time for the delay
        @returns {IEnumerator} returns time delay specified by the callee
    */
    IEnumerator Dead(float timer)
    {
        yield return new WaitForSeconds(timer);
        gameObject.SetActive(false);
    }

    /** 
        Detects collision with the bullet. If the game object is the beetle,
        the gameobject will start the dead coroutine. If the gameobject is the
        snail, it will be stunned if not already stunned and it will become
        disabled if already stunned.

        @param {Collider2D} The other Collider2D involved in this collision.
    */
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == Tags.BULLET_TAG)
        {
            if (tag == Tags.BEETLE_TAG)
            {
                anim.Play("Stunned");
                canMove = false;
                stunned = true;
                rbody.velocity = new Vector2(0, 0);
                StartCoroutine(Dead(0.3f));
            }

            if (tag == Tags.SNAIL_TAG)
            {
                if (!stunned)
                {
                    anim.Play("Stunned");
                    stunned = true;
                    canMove = false;
                    rbody.velocity = new Vector2(0, 0);
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }
}
