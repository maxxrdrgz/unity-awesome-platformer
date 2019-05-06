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
        //print("called second");
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

    void PlayerWalk()
    {
        float h = Input.GetAxisRaw("Horizontal");
        if(h > 0)
        {
            rbody.velocity = new Vector2(speed, rbody.velocity.y);
            ChangeDirection(true);
            //sr.flipX = false;
        }else if(h < 0)
        {
            rbody.velocity = new Vector2(-speed, rbody.velocity.y);
            ChangeDirection(false);
            //sr.flipX = true;
        }
        else
        {
            rbody.velocity = new Vector2(0f, rbody.velocity.y);
        }

        anim.SetInteger("Speed", Mathf.Abs((int)rbody.velocity.x));
    }

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
