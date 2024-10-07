using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MacthManager : MonoBehaviour
{
    public static MacthManager instance;

   [SerializeField] private GameObject startTarget, startPlayer;
   [SerializeField] private CinemachineVirtualCamera transitionCamera;
   [SerializeField] private GameObject winUI;
    private GameObject target, currentPlayer;
    private PlayerMatchManager currentPlayerMatchManager, targetPlayerMatchManager;

    [SerializeField] private GameObject ui;
    private string turnToCall;
    public bool canPlaceShips {  get; private set; }
    public bool canAttack {  get; private set; }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        
    }
    private void Start()
    {
        currentPlayer = startPlayer;
        target = startTarget;
        currentPlayerMatchManager = currentPlayer.GetComponent<PlayerMatchManager>();
        PreparationTurn();
    }

    private void ChooseTypeOfTurn()
    {
        currentPlayerMatchManager = currentPlayer.GetComponent<PlayerMatchManager>();

        if(currentPlayerMatchManager.turnCounter <= 0)
        {
            turnToCall = "PreparationTurn";
            return;
        }
        turnToCall = "AttackTurn";
    }

    private void AttackTurn()
    {
        currentPlayerMatchManager.AttackTurnSetup();
        canPlaceShips = false;
        canAttack = true;
    }

    private void PreparationTurn()
    {
        currentPlayerMatchManager.PreparationTurnSetup();
        canPlaceShips = true;
    }

    public void EndTurn(GameObject target, GameObject currentPlayer)
    {
       this.target = currentPlayer;
       this.currentPlayer = target;

        targetPlayerMatchManager = target.GetComponent<PlayerMatchManager>();

        ChooseTypeOfTurn();
        transitionCamera.Priority = 4;
        ui.SetActive(true);
    }
  
    public void StartNextRound()
    {
        Invoke(turnToCall, 0f);
        transitionCamera.Priority = 0;
        ui.SetActive(false);
    }
    public void Win(GameObject winner)
    {
        currentPlayerMatchManager = currentPlayer.GetComponent<PlayerMatchManager>();
        currentPlayerMatchManager.FinishGame();
        targetPlayerMatchManager.FinishGame();
        winUI.SetActive(true);
        TextMeshProUGUI winnerText = winUI.GetComponentInChildren<TextMeshProUGUI>();
        winnerText.text = "Winner: " + winner.name;
    }
}
