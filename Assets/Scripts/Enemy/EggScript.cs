using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggScript : MonoBehaviour
{
    /**
        Detects collision with the player. If so deal damage to the player and
        disable the gameobject this script is applied to. The egg (stone) in
        this case.
    */
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == Tags.PLAYER_TAG)
        {
            collision.gameObject.GetComponent<PlayerDamage>().DealDamage();
        }
        gameObject.SetActive(false);
    }
}
