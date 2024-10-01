using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipPartsManager : MonoBehaviour
{
    ShipsManager shipsManager;
    private void Start()
    {
        shipsManager = GetComponentInParent<ShipsManager>();   
    }
    private void OnTriggerEnter(Collider other)
    {
        Bullet bulletScrit = other.gameObject.GetComponent<Bullet>();
        if (bulletScrit != null)
        {
            shipsManager.shipLife--;
            shipsManager.CheckIsAlive();
        }
    }
}
