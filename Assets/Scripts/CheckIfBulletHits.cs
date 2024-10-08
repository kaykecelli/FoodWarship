using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckIfBulletHits : MonoBehaviour
{
    [SerializeField]private LayerMask shipLayer;
    private Bullet bullet;
    private void Start()
    {
        bullet = GetComponentInParent<Bullet>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.gameObject.layer == shipLayer)
        {
            bullet.hasHitAShip = true;
            return;
        }

        bullet.hasHitAShip = false;

    }
}
