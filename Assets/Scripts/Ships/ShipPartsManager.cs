using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipPartsManager : MonoBehaviour
{
    private ShipsManager shipsManager;
    private Renderer render;
    private void Start()
    {
        shipsManager = GetComponentInParent<ShipsManager>(); 
        render = gameObject.GetComponent<Renderer>();
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);
        Bullet bulletScript = other.GetComponentInParent<Bullet>();
        if (bulletScript != null)
        {
            shipsManager.shipLife--;
            render.material = shipsManager.hitMaterial;
            shipsManager.CheckIsAlive();
        }
    }
}
