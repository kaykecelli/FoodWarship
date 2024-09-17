using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    // Utilizado para saber em qual posição do plano o mouse esta
    [SerializeField] private GameObject mouseIndicator, cellIndicator;

    [SerializeField] private InputManager inputManager;

    [SerializeField] private Grid grid;

    private void FixedUpdate()
    {
        Vector3 mousePosition = inputManager.GetMousePosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        mouseIndicator.transform.position = mousePosition;
        cellIndicator.transform.position = grid.CellToWorld(gridPosition);
    }


}
