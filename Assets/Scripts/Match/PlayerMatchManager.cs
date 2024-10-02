using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] private CinemachineVirtualCamera[] vCameras;
    private int index;

    [Header("Ship counter")]
    [HideInInspector]public int shipCounter;
    [SerializeField] private TextMeshProUGUI counterUI;

    public event Action OnEnableShips, OnDisableShips;
    private void Awake()
    {
        placementSystem = placementObj.GetComponent<PlacementSystem>();
        attackSystem = placementObj.GetComponent<AttackSystem>();
    }
    public void EndTurn()
    {
        OnDisableShips?.Invoke();
        turnCounter++;
        placementSystem.StopPlacement();
        vCameras[index].Priority = 0;
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
        vCameras[index].Priority = 4;
    }

    public void AttackTurnSetup()
    {
        OnEnableShips?.Invoke();
        Debug.Log("AttackTurn");
        attackUI.SetActive(true);
        generalUI.SetActive(true);
        attackSystem.enabled = true;
        vCameras[index].Priority = 4;
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
    public void ChangeCamera()
    {
        
        index++;
        if (index >= vCameras.Length)
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
    }
    public void IsAlive()
    {
        if(shipCounter <= 0)
        {
            MacthManager.instance.Win(target);
        }
    }

    public void AtualizeUICounter(int maxValue)
    {
        counterUI.text = shipCounter.ToString() + "/" + maxValue.ToString();
    }
}
