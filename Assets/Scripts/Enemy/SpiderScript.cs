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

    void MoveSpider()
    {
        transform.Translate(moveDirection * Time.smoothDeltaTime);
    }

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

    IEnumerator SpiderDead()
    {
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
    }

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
    }
}
