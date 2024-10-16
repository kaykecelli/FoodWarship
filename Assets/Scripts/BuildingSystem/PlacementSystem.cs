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
    [SerializeField] private int maxShipsQuantity;

    private int currentShipsQuantity;
    private PlayerMatchManager playerMatchManager;

    private bool startPrevisualization;
    private GameObject preVisualizationObj;
    private Vector3Int gridPosition;

    private DataGrid shipsData;

    private Renderer previewRender;

    private List<GameObject> placedObjects = new List<GameObject>();
    private void Awake()
    {
        StopPlacement();
        SetUp();
    }
    private void OnEnable()
    {
        inputManager.OnRemoveStructure += RemoveStructure;
    }
    private void OnDisable()
    {
        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;
        inputManager.OnRemoveStructure -= RemoveStructure;
    }
    private void SetUp()
    {
        shipsData = new DataGrid();
        previewRender = cellIndicator.GetComponentInChildren<Renderer>();
        playerMatchManager = gameObject.GetComponentInParent<PlayerMatchManager>();
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
        startPrevisualization = true;
        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
        PreVisualization();
    }

    private void PlaceStructure()
    {
        if (currentShipsQuantity <= maxShipsQuantity)
        {
            if (!playerMatchManager.isMyTurn && !MacthManager.instance.canPlaceShips)
            {
                return;
            }
            if (inputManager.IsPointerOverUI())
            {
                return;
            }
            Vector3 mousePosition = inputManager.GetMousePosition();
            Vector3Int gridPosition = grid.WorldToCell(mousePosition);

            bool placementValidity = GetPlacementValidity(gridPosition, selectedObjectIndex);
            if (placementValidity == false)
            {
                return;
            }

            GameObject newObj = Instantiate(databaseSO.objectsData[selectedObjectIndex].Prefab,transform.parent);
            newObj.GetComponent<ShipsManager>().InPlacement();    
            newObj.transform.position = grid.CellToWorld(gridPosition);
            placedObjects.Add(newObj);
            shipsData.AddObjectAt(gridPosition, databaseSO.objectsData[selectedObjectIndex].Size, databaseSO.objectsData[selectedObjectIndex].ID, placedObjects.Count - 1);
            playerMatchManager.shipCounter++;
            currentShipsQuantity++;
            playerMatchManager.AtualizeUICounter(maxShipsQuantity);
        }
       
    }
    private void RemoveStructure()
    {
        StopPlacement();
        if (inputManager.IsPointerOverUI())
        {
            return;
        }
        GameObject selectedObj = inputManager.GetShipToRemove();
        int selectedObjectIndex = selectedObj.GetComponentInParent<ShipsManager>().index;
        Vector3 mousePosition = inputManager.GetMousePosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        bool placementValidity = GetPlacementValidity(gridPosition, selectedObjectIndex);
        if (placementValidity == false)
        {
            
            shipsData.RemoveObjectAt(gridPosition, databaseSO.objectsData[selectedObjectIndex].Size);
            Destroy(selectedObj);
            currentShipsQuantity--;
            playerMatchManager.shipCounter--;
            playerMatchManager.AtualizeUICounter(maxShipsQuantity);
        }
    }

    private bool GetPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        return shipsData.CanPlaceObjectAt(gridPosition, databaseSO.objectsData[selectedObjectIndex].Size);
    }

    public void StopPlacement()
    {
        selectedObjectIndex =  -1;
        Destroy(preVisualizationObj);
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
        
        bool placementValidity = GetPlacementValidity(gridPosition, selectedObjectIndex);
        previewRender.material.color = placementValidity ? Color.white : Color.red;

        mouseIndicator.transform.position = mousePosition;
        cellIndicator.transform.position = grid.CellToWorld(gridPosition);
    }

}
