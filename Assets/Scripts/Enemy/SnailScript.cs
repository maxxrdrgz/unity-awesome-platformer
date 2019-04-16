using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnailScript : MonoBehaviour
{
    public float moveSpeed = 1f;
    public Transform left_Collision, right_Collision, top_Collision, down_Collision;
    public LayerMask playerLayer;

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
        left_collision_pos = left_Collision.position;
        right_collision_pos = right_Collision.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        moveLeft = true;
        sr.flipX = false;
        canMove = true;

    }

    // Update is called once per frame
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

    void CheckCollision()
    {
        RaycastHit2D leftHit = Physics2D.Raycast(left_Collision.position, Vector2.left, 0.1f, playerLayer);
        RaycastHit2D rightHit = Physics2D.Raycast(right_Collision.position, Vector2.right, 0.1f, playerLayer);

        Collider2D topHit = Physics2D.OverlapCircle(top_Collision.position, 0.2f, playerLayer);

        if(topHit != null)
        {
            if(topHit.gameObject.tag == Tags.PLAYER_TAG)
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
                    if(tag == Tags.BEETLE_TAG)
                    {
                        anim.Play("Stunned");
                        StartCoroutine(Dead(0.5f));
                    }
                }
            }
        }

        if(leftHit)
        {
            if(leftHit.collider.gameObject.tag == Tags.PLAYER_TAG)
            {
                if (!stunned)
                {
                    leftHit.collider.gameObject.GetComponent<PlayerDamage>().DealDamage();
                }
                else
                {
                    if(tag != Tags.BEETLE_TAG)
                    {
                        rbody.velocity = new Vector2(15f, rbody.velocity.y);
                        StartCoroutine(Dead(3f));
                    }
                }
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
                    if(tag != Tags.BEETLE_TAG)
                    {
                        rbody.velocity = new Vector2(-15f, rbody.velocity.y);
                        StartCoroutine(Dead(3f));
                    }
                }
            }
        }

        if (!Physics2D.Raycast(down_Collision.position, Vector2.down, 0.1f))
        {
            print("detected no ground");
            ChangeDirection();
        }
    }

    void ChangeDirection()
    {
        moveLeft = !moveLeft;
        Vector3 tempScale = transform.localScale;

        if (moveLeft)
        {
            tempScale.x = Mathf.Abs(tempScale.x);
            left_Collision.position = left_collision_pos;
            right_Collision.position = right_collision_pos;
        }
        else
        {
            tempScale.x = -Mathf.Abs(tempScale.x);
            left_Collision.position = right_collision_pos;
            right_Collision.position = left_collision_pos;
        }
        transform.localScale = tempScale;
    }

    IEnumerator Dead(float timer)
    {
        yield return new WaitForSeconds(timer);
        gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == Tags.BULLET_TAG)
        {
            if(tag == Tags.BEETLE_TAG)
            {
                anim.Play("Stunned");
                canMove = false;
                rbody.velocity = new Vector2(0, 0);
                StartCoroutine(Dead(0.4f));
            }

            if(tag == Tags.SNAIL_TAG)
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
