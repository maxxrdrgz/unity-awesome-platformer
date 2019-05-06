using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneScript : MonoBehaviour
{
    /**
        Once started, this script will call the Deactivate() function after
        4 seconds.
    */
    void Start()
    {
        Invoke("Deactivate", 4f);
        // invoke calls the specifed function after time specified
    }

    /**
        Disables the gameobject that this script is applied to.
        (Stone in this case)
    */
    void Deactivate()
    {
        gameObject.SetActive(false);
    }

    /**
        Detects collision with the player. If player has been collided with
        disable the gameobject and deal damage to the player.
    */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == Tags.PLAYER_TAG)
        {
            gameObject.SetActive(false);
            collision.GetComponent<PlayerDamage>().DealDamage();
        }
    }
}
