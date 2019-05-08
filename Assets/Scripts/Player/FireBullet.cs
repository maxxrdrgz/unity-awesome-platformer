using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullet : MonoBehaviour
{
    private float speed = 10f;
    private Animator anim;
    private bool canMove;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    // Start is called before the first frame update

    /** 
        When the bullet gameobject is instantiated, start a coroutine to
        disable the bullet after 3 seconds
    */
    void Start()
    {
        canMove = true;
        StartCoroutine(DisableBullet(1f));
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    /** 
        Moves the bullet horizontally
    */
    void Move()
    {
        if (canMove)
        {
            Vector3 temp = transform.position;
            temp.x += speed * Time.unscaledDeltaTime;
            transform.position = temp;
        }
    }

    public float Speed
    {
        get
        {
            return speed;
        } 
        set
        {
            speed = value;
        }
    }

    /** 
        Disables the gameobject (bullet) that this script is attached to

        @param {float} amount of time to delay for 
        @returns {IEnumerator} returns time delay of specified time
    */
    IEnumerator DisableBullet(float timer)
    {
        yield return new WaitForSeconds(timer);
        gameObject.SetActive(false);
    }

    /** 
        Detects collision will all colliders except with the player and the 
        camera. After collision is detected, starts a coroutine to disable the
        bullet gameobject after .1 seconds.

        @param {Collider2D} the other collider2d involved in the collision
    */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag != Tags.PLAYER_TAG && 
           collision.gameObject.tag != Tags.MAIN_CAMERA &&
           collision.gameObject.tag != Tags.COIN_TAG)
        {
            canMove = false;
            anim.Play("Explode");
            StartCoroutine(DisableBullet(0.1f));
        }
    }

}

