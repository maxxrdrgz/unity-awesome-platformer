using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBoss : MonoBehaviour
{
    public GameObject Boss;

    private float x_pos = 107.9466f;
    private float y_pos = -2.94f;

    /** 
        Detects collision with player. If true, destory this gameobject and
        instantiate the boss gameobject.
    */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == Tags.PLAYER_TAG)
        {
            Instantiate(Boss, new Vector3(x_pos, y_pos, 0f), Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
