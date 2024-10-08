using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShipPartsManager : MonoBehaviour
{
    private ShipsManager shipsManager;
    private Renderer render;
    private bool hasBeenHit;
    Collider partcolider;
    private void Start()
    {
        shipsManager = GetComponentInParent<ShipsManager>(); 
        render = gameObject.GetComponent<Renderer>();
       partcolider =  gameObject.GetComponent<Collider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        Bullet bulletScript = other.GetComponentInParent<Bullet>();
        if (bulletScript != null && !hasBeenHit)
        {
            partcolider.enabled = false;
            hasBeenHit = true;
            GameObject hitCanvas = Instantiate(shipsManager.hitUI, other.transform.position, Quaternion.identity);
            hitCanvas.GetComponent<Canvas>().worldCamera = Camera.main;
            TextMeshProUGUI textMeshProUGUI = hitCanvas.GetComponentInChildren<TextMeshProUGUI>();
            textMeshProUGUI.transform.LookAt(Camera.main.transform.position);
            textMeshProUGUI.text = "TIH";
            textMeshProUGUI.color = Color.red;
            shipsManager.shipLife--;
            render.material = shipsManager.hitMaterial;
            shipsManager.CheckIsAlive();
            Destroy(hitCanvas, 3f);
        }
    }
}
