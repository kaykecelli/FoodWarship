using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShipPartsManager : MonoBehaviour
{
    private ShipsManager shipsManager;
    private Renderer render;
    private bool hasBeenHit;
    private void Start()
    {
        shipsManager = GetComponentInParent<ShipsManager>(); 
        render = gameObject.GetComponent<Renderer>();
    }
    private void OnTriggerEnter(Collider other)
    {
        Bullet bulletScript = other.GetComponentInParent<Bullet>();
        if (bulletScript != null && !hasBeenHit)
        {
            hasBeenHit = true;
            Canvas hitCanvas = Instantiate(shipsManager.hitUI, other.transform.position, Quaternion.identity);
            hitCanvas.GetComponent<Canvas>().worldCamera = Camera.main;
            TextMeshProUGUI textMeshProUGUI = hitCanvas.GetComponentInChildren<TextMeshProUGUI>();
            textMeshProUGUI.transform.LookAt(-Camera.main.transform.position);
            textMeshProUGUI.text = "HIT";
            textMeshProUGUI.color = Color.red;
            shipsManager.shipLife--;
            render.material = shipsManager.hitMaterial;
            shipsManager.CheckIsAlive();
        }
    }
}
