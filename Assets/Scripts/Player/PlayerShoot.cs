using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject fireBullet;

    private void Update()
    {
        ShootBullet();
    }

    /** 
        Detects input from user. If user presses the J key, instantiate the
        bullet gameobject and fire the bullet either in the positive or negative
        determined by the gameobjects scale.
    */
    void ShootBullet()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            GameObject bullet = Instantiate(fireBullet, transform.position, Quaternion.identity);
            bullet.GetComponent<FireBullet>().Speed *= transform.localScale.x;
        }
    }
}
