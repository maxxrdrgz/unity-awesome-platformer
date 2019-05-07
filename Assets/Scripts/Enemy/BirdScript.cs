\using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdScript : MonoBehaviour
{
    private Rigidbody2D rbody;
    private Animator anim;
    private Vector3 moveDirection = Vector3.left;
    private Vector3 originPosition;
    private Vector3 leftMostPosition;
    private Vector3 rightMostPosition;
    private Vector3 movePosition;


    public GameObject birdEgg;
    public LayerMask playerLayer;
    private bool attacked;
    private bool canMove;

    private float speed = 2.5f;

    private void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    /**
        Initializes the horizontal bounds for the bird
    */
    void Start()
    {
        originPosition = transform.position;
        rightMostPosition.x = originPosition.x + 6f;
        leftMostPosition.x = originPosition.x - 6f;
        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        MoveTheBird();
        DropTheEgg();
    }

    /**
        Moves the birds transform in the left and right direction between the
        leftMost and rightMost positions
    */
    void MoveTheBird()
    {
        if (canMove)
        {
            transform.Translate(moveDirection * speed * Time.smoothDeltaTime);
            if(transform.position.x >= rightMostPosition.x)
            {
                moveDirection = Vector3.left;
                ChangeDirection(Vector3.left);
            }
            else if(transform.position.x <= leftMostPosition.x)
            {
                moveDirection = Vector3.right;
                ChangeDirection(Vector3.right);
            }
        }
    }

    /** 
        Swaps the sign on the x scale to mirror the gameobjects transform
    */
    void ChangeDirection(Vector3 direction)
    {
        Vector3 tempScale = transform.localScale;

        if (direction.x == -1f)
        {
            tempScale.x = Mathf.Abs(tempScale.x);
            //left_Collision.position = left_collision_pos;
            //right_Collision.position = right_collision_pos;
        }
        else
        {
            tempScale.x = -Mathf.Abs(tempScale.x);
            //left_Collision.position = right_collision_pos;
            //right_Collision.position = left_collision_pos;
        }
        transform.localScale = tempScale;
    }
    
    /**
        Detects if the player is below the bird, and if so, instantiates the
        egg gameobject, plays a new animation, and starts the ReloadEgg()
        coroutine.
    */
    void DropTheEgg()
    {
        if (!attacked)
        {
            if(Physics2D.Raycast(transform.position, Vector2.down, 
            Mathf.Infinity, playerLayer))
            {
                Instantiate(birdEgg, new Vector3(
                    transform.position.x,
                    transform.position.y - 1,
                    transform.position.z), 
                    Quaternion.identity
                );
                attacked = true;
                anim.Play("BirdFly");
                StartCoroutine(ReloadEgg());
            }
        }
    }

    /**
        Disables the gameobject this script is attached to. The bird in this 
        case.

        @returns {IEnumerator} returns a time delay of 3 seconds
    */
    IEnumerator BirdDead()
    {
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
    }

    /**
        Detects if the gameobject has collided with a bullet. If so, plays
        the bird dead animation, changes the rigid body type to make the
        object fall from the sky, and starts the bird dead coroutine.
    */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == Tags.BULLET_TAG)
        {
            anim.Play("BirdDead");
            GetComponent<BoxCollider2D>().isTrigger = true;
            rbody.bodyType = RigidbodyType2D.Dynamic;
            canMove = false;
            StartCoroutine(BirdDead());
        }
    }
    /**
        Sets the gameobjects animator to the BirdStone animation and sets the
        attacked boolean back to false.

        @returns {IEnumerator} returns a time delay of 4 seconds
    */
    IEnumerator ReloadEgg()
    {
        yield return new WaitForSeconds(4f);
        anim.Play("BirdStone");
        attacked = false;
    }
}
