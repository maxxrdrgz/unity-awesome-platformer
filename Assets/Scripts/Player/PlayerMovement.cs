using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public Transform groundCheckPosition;
    public LayerMask groundLayer;

    private Rigidbody2D rbody;
    private Animator anim;
    private SpriteRenderer sr;
    private bool isGrounded;
    private bool jumped;

    private float jumpPower = 12f;

    private void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfGrounded();
        PlayerJump();
    }

    //called in fixed time steps Edit -> Project Settings -> Time -> Fixed Timestep | this is good for physics
    void FixedUpdate()
    {
        PlayerWalk();
    }

    /** 
        Gets horizontal input from user (arrowkeys) and ands velocity to players
        rigid body in the corresponding direction
    */
    void PlayerWalk()
    {
        float h = Input.GetAxisRaw("Horizontal");
        if(h > 0)
        {
            rbody.velocity = new Vector2(speed, rbody.velocity.y);
            ChangeDirection(true);
        }else if(h < 0)
        {
            rbody.velocity = new Vector2(-speed, rbody.velocity.y);
            ChangeDirection(false);
        }
        else
        {
            rbody.velocity = new Vector2(0f, rbody.velocity.y);
        }

        anim.SetInteger("Speed", Mathf.Abs((int)rbody.velocity.x));
    }

    /** 
        Detects if player is on the ground, if so, allows the player to jump.
    */
    void CheckIfGrounded()
    {
        isGrounded = Physics2D.Raycast(groundCheckPosition.position, Vector2.down, 0.1f, groundLayer);
        if (isGrounded)
        {
            if (jumped)
            {
                jumped = false;
                anim.SetBool("Jump", false);
            }
        }
    }

    /** 
        If grounded, gets Spacebar input from player, changes the y velocity by 
        the jumpPower of units. Sets animator param jump to true.
    */
    void PlayerJump()
    {
        if (isGrounded)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                jumped = true;
                rbody.velocity = new Vector2(rbody.velocity.x, jumpPower);
                anim.SetBool("Jump", true);
                SoundManager.instance.PlayJumpSound();
            }
        }
    }

    /** 
        Yet another example of how to switch the sign on the gameobjects scale

        @param {bool} Specify to move right
    */
    void ChangeDirection(bool moveRight)
    {
        Vector3 tempScale = transform.localScale;

        if (moveRight)
        {
            tempScale.x = Mathf.Abs(tempScale.x);
        }
        else
        {
            tempScale.x = -Mathf.Abs(tempScale.x);
        }
        transform.localScale = tempScale;
    }
}
