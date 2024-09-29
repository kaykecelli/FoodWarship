using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSystem : MonoBehaviour
{
    [SerializeField] private GameObject mouseIndicator, cellIndicator;

    [SerializeField] private InputManager inputManager;
   
    [SerializeField] private Grid enemyGrid;
    [SerializeField] private CinemachineVirtualCamera[] vCameras;

    private List<Vector3Int> attacksPosition =  new();
    private List<GameObject> attackMarkersGameOnject;
    Vector3 mousePosition;
    Vector3Int gridPosition;
     private int index = 1;
    private void OnEnable()
    {
        inputManager.OnClicked += MarkAttackPosition;
    }
    private void OnDisable()
    {
        inputManager.OnClicked -= MarkAttackPosition;
    }


    private void FixedUpdate()
    {
        mousePosition = inputManager.GetAttackMousePosition();
        gridPosition = enemyGrid.WorldToCell(mousePosition);
        mouseIndicator.transform.position = mousePosition;
        cellIndicator.transform.position = enemyGrid.CellToWorld(gridPosition);
    }

    private void MarkAttackPosition()
    {
        GameObject attackMarker = Instantiate(cellIndicator, gridPosition, Quaternion.identity);
        Renderer renderer = attackMarker.GetComponent<Renderer>();
        renderer.material.color = Color.red;
        attacksPosition.Add(gridPosition);
        attackMarkersGameOnject.Add(attackMarker);
    }

    public void Attack()
    {

    }

    public void ChangeCamera()
    {
        if(index >= vCameras.Length)
        {
            index = 0;
        }

        vCameras[index].Priority = 5;
        for (int i = 0; i < vCameras.Length; i++)
        {
            if (vCameras[i] != vCameras[index])
            {
                vCameras[i].Priority = 0;
            }
        }

        index++;
    }
    
}
