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
    [SerializeField] GameObject placementUI,attackUI, placementObj, gridVisualization, generalUI;
    private PlacementSystem placementSystem;
    private AttackSystem attackSystem;
    [SerializeField] CinemachineVirtualCamera cam;

    private void Awake()
    {
        placementSystem = placementObj.GetComponent<PlacementSystem>();
        attackSystem = placementObj.GetComponent<AttackSystem>();
    }
    [ContextMenu("End Turn")]
    public void EndTurn()
    {
        turnCounter++;
        placementSystem.StopPlacement();
        cam.Priority = 0;
        placementSystem.enabled = false;
        attackSystem.enabled = false;
        placementUI.SetActive(false);
        attackUI.SetActive(false);
        generalUI.SetActive(false);
        isMyTurn = false;
        MacthManager.instance.EndTurn(target, gameObject);
        ChangeLayer();
    }

    public void PreparationTurnSetup()
    {
        Debug.Log("PreparationTurn");
        placementUI.SetActive(true);
        generalUI.SetActive(true);
        attackUI.SetActive(false);
        placementSystem.enabled = true;
        attackSystem.enabled = false;
        isMyTurn = true;
        cam.Priority = 4;
    }

    public void AttackTurnSetup()
    {
        Debug.Log("AttackTurn");
        attackUI.SetActive(true);
        generalUI.SetActive(true);
        attackSystem.enabled = true;
        cam.Priority = 4;
        isMyTurn = true;
        ChangeLayer();
    }

    private void ChangeLayer()
    {
        if (isMyTurn)
        {
            gridVisualization.layer = LayerMask.NameToLayer("Placement");
        }
        else
        {
            gridVisualization.layer = LayerMask.NameToLayer("Target");
        }
       Debug.Log(LayerMask.NameToLayer("Target"));
    }
    
}
