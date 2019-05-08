using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderScript : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rbody;
    private Vector3 moveDirection = Vector3.down;
    private string couroutine_name = "ChangeMovement";

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rbody = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(couroutine_name);
    }

    // Update is called once per frame
    void Update()
    {
        MoveSpider();
    }

    /** 
        Changes the position of the spider's transform in the moveDirection
    */
    void MoveSpider()
    {
        transform.Translate(moveDirection * Time.smoothDeltaTime);
    }

    /** 
        Changes the moveDirection up or down, then recursively starts itself in
        a new coroutine.

        @returns {IEnumerator} returns random time delay between 4 and 6 seconds
    */
    IEnumerator ChangeMovement()
    {
        yield return new WaitForSeconds(Random.Range(4f, 6f));
        if(moveDirection == Vector3.down)
        {
            moveDirection = Vector3.up;
        }
        else
        {
            moveDirection = Vector3.down;
        }
        StartCoroutine(couroutine_name);
    }

    /** 
        Disables the spider gameobject

        @returns {IEnumerator} returns a time delay of 3 seconds
    */
    IEnumerator SpiderDead()
    {
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
    }

    /** 
        Detects collision with the bullet. If so, start the SpiderDead()
        coroutine. If collision is with ground, change the direction. If the
        collision is with the player, deal damage to player.

        @param {Collider2D} The other Collider2D involved in this collision.
    */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == Tags.BULLET_TAG)
        {
            anim.Play("SpiderDead");
            rbody.bodyType = RigidbodyType2D.Dynamic;
            StartCoroutine(SpiderDead());
        }else if(collision.tag == Tags.GROUND_TAG)
        {
            moveDirection = Vector3.up;
        }

        if(collision.tag == Tags.PLAYER_TAG)
        {
            collision.GetComponent<PlayerDamage>().DealDamage();
        }
    }
}
