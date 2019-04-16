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
    void Start()
    {
        canMove = true;
        StartCoroutine(DisableBullet(5f));
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        if (canMove)
        {
            Vector3 temp = transform.position;
            temp.x += speed * Time.deltaTime;
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

    IEnumerator DisableBullet(float timer)
    {
        yield return new WaitForSeconds(timer);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if(collision.gameObject.tag == Tags.BEETLE_TAG || collision.gameObject.tag == Tags.SNAIL_TAG || collision.gameObject.tag == Tags.BIRD_TAG)
        //{
        if(collision.gameObject.tag != Tags.PLAYER_TAG && collision.gameObject.tag != Tags.MAIN_CAMERA)
        {
            canMove = false;
            anim.Play("Explode");
            StartCoroutine(DisableBullet(0.1f));
        }
        //}
    }

}

