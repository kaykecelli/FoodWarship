using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MacthManager : MonoBehaviour
{
    public static MacthManager instance;

   [SerializeField] private GameObject startTarget, startPlayer;

    private GameObject target, currentPlayer;
    private PlayerMatchManager currentPlayerMatchManager, targetPlayerMatchManager;
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
       ChooseTypeOfTurn();
    }
    private void ChooseTypeOfTurn()
    {
        currentPlayerMatchManager = currentPlayer.GetComponent<PlayerMatchManager>();

        if(currentPlayerMatchManager.turnCounter <= 0)
        {
            PreparationTurn();
            return;
        }
        AttackTurn();
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
    }
  
}
