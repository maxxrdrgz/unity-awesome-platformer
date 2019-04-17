using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneScript : MonoBehaviour
{



    // Start is called before the first frame update
    void Start()
    {
        Invoke("Deactivate", 4f);
        // invoke calls the specifed function after time specified
    }

    void Deactivate()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == Tags.PLAYER_TAG)
        {
            gameObject.SetActive(false);
            collision.GetComponent<PlayerDamage>().DealDamage();
        }
    }
}
