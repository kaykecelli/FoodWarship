using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMatchManager : MonoBehaviour
{
    public int turnCounter;
    public bool isMyTurn;
    public Grid grid {  get; private set; }
    [SerializeField] private GameObject target;
    [SerializeField] GameObject ui, placementSystem;
    private PlacementSystem placement;
    [SerializeField] CinemachineVirtualCamera cam;

    private void Start()
    {
        placement = placementSystem.GetComponent<PlacementSystem>();
    }
    [ContextMenu("End Turn")]
    public void EndTurn()
    {
        turnCounter++;
        placement.StopPlacement();
        cam.Priority = 2;
        placementSystem.SetActive(false);
        ui.SetActive(false);
        isMyTurn = false;
        MacthManager.instance.EndTurn(target, gameObject);
    }

    public void PreparationTurnSetup()
    {
        Debug.Log("PreparationTurn");
        ui.SetActive(true);
        placementSystem.SetActive(true);
        isMyTurn = true;
        cam.Priority = 4;
    }

    public void AttackTurnSetup()
    {
        Debug.Log("AttackTurn");
        placementSystem.SetActive(true);
        cam.Priority = 4;
        isMyTurn = true;
    }
    
}
