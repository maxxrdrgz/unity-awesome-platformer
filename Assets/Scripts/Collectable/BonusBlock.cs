using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusBlock : MonoBehaviour
{

    public Transform bottom_Collision;
    public LayerMask playerLayer;

    private Animator anim;
    private Vector3 moveDirection = Vector3.up;
    private Vector3 originPosition;
    private Vector3 animPosition;
    private bool startAnim;
    private bool canAnimate = true;
    private GameObject player;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag(Tags.PLAYER_TAG);
    }

    // Start is called before the first frame update
    void Start()
    {
        originPosition = transform.position;
        animPosition = transform.position;
        animPosition.y += 0.15f;
    }

    void Update()
    {
        CheckForCollision();
        AnimateUpDown();
    }

    /**
        Checks if player is below a Bonus Blocks bottom_Collision.position
        If so, play its animations and increase the players coin score
    */
    void CheckForCollision()
    {
        if (canAnimate)
        {
            RaycastHit2D hit = Physics2D.Raycast(bottom_Collision.position, Vector2.down, 0.1f, playerLayer);
            if (hit)
            {
                if (hit.collider.gameObject.tag == Tags.PLAYER_TAG)
                {
                    //increase scoree
                    anim.Play("BlockIdle");
                    startAnim = true;
                    canAnimate = false;
                    player.GetComponent<ScoreScript>().IncreaseScore(5);
                }
            }
        }
    }
    
    /**
        Moves the gameobjects transform up and down to give a Bonus Block a 
        moving affect when hit from below.
    */
    void AnimateUpDown()
    {
        if (startAnim)
        {
            transform.Translate(moveDirection * Time.smoothDeltaTime);
            if(transform.position.y >= animPosition.y)
            {
                moveDirection = Vector3.down;
            }else if(transform.position.y <= originPosition.y)
            {
                startAnim = false;
            }
        }
    }
}
