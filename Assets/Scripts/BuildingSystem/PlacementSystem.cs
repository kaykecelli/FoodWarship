using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    // Utilizado para saber em qual posi��o do plano o mouse esta
    [SerializeField] private GameObject mouseIndicator, cellIndicator;

    [SerializeField] private InputManager inputManager;

    [SerializeField] private Grid grid;

    [SerializeField] private ObjectsDatabaseSO databaseSO;
    private int selectedObjectIndex = -1;

    [SerializeField] private GameObject gridVisualization;

    private bool startPrevisualization;
    private GameObject preVisualizationObj;
    private Vector3Int gridPosition;
    private void Start()
    {
        StopPlacement();
    }

    public void StartPlacement(int ID)
    {
        StopPlacement();
        selectedObjectIndex = databaseSO.objectsData.FindIndex(data => data.ID == ID); 
        if(selectedObjectIndex < 0)
        {
            Debug.LogError($"No ID Found {ID}");
            return;
        }

        gridVisualization.SetActive(true);
        cellIndicator.SetActive(true);
        mouseIndicator.SetActive(true);
        startPrevisualization = true;
        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
        PreVisualization();
    }

    private void PlaceStructure()
    {
        if (inputManager.IsPointerOverUI())
        {
            return;
        }
        Vector3 mousePosition = inputManager.GetMousePosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        GameObject newObj = Instantiate(databaseSO.objectsData[selectedObjectIndex].Prefab); 
        newObj.transform.position = grid.CellToWorld(gridPosition);
    }

    private void StopPlacement()
    {
        selectedObjectIndex =  -1;
        Destroy(preVisualizationObj);
        gridVisualization.SetActive(false);
        cellIndicator.SetActive(false);
        mouseIndicator.SetActive(false);
        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;
        startPrevisualization = false;
    }

    private void FixedUpdate()
    {
        if(selectedObjectIndex < 0)
            return;
        GridIndicatorSetup();

        if (startPrevisualization)
        {
            preVisualizationObj.transform.position = grid.CellToWorld(gridPosition);
        }
    }

    private void PreVisualization()
    {
        Vector3 mousePosition = inputManager.GetMousePosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        preVisualizationObj = Instantiate(databaseSO.objectsData[selectedObjectIndex].Prefab);
    }

    private void GridIndicatorSetup()
    {
        Vector3 mousePosition = inputManager.GetMousePosition();
        gridPosition = grid.WorldToCell(mousePosition);
        mouseIndicator.transform.position = mousePosition;
        cellIndicator.transform.position = grid.CellToWorld(gridPosition);
    }

}
