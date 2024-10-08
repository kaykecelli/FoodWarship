using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.VFX;

public class ShipsManager : MonoBehaviour
{
    private MeshRenderer[] meshRenderers;
    private PlayerMatchManager playerMatchManager;
    private int currentCookingCounter;
    [SerializeField] private int maxCookingReady;
    [SerializeField] private bool hasEspecialBehaviour;
    [HideInInspector]public int shipLife;
    [SerializeField] private VisualEffect smokeEffect;
    [SerializeField] private GameObject leafObj;
    public GameObject spawnIteractionPos;
    public int index;
    public GameObject hitUI;
    private bool canCook = true;
    public Material hitMaterial;

    private void Start()
    {
        meshRenderers = new MeshRenderer[transform.childCount];
        shipLife = transform.childCount;
    }
    public void InPlacement()
    {
        playerMatchManager = GetComponentInParent<PlayerMatchManager>();
        playerMatchManager.OnEnableShips += EnableShips;
        playerMatchManager.OnDisableShips += DisableShips;
    }
    private void OnDisable()
    {
        if(playerMatchManager != null)
        {
            playerMatchManager.OnEnableShips -= EnableShips;
            playerMatchManager.OnDisableShips -= DisableShips;
        }
    }
    private void GetAllMeshRender()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            meshRenderers[i] = transform.GetChild(i).GetComponent<MeshRenderer>();
        }
      
    }
    public void DisableShips()
    {
        GetAllMeshRender();

        for (int i = 0; i < meshRenderers.Length; i++)
        {
            meshRenderers[i].enabled = false;
        }
        spawnIteractionPos.SetActive(false);
    }
    public void EnableShips()
    {
        GetAllMeshRender();
        for (int i = 0; i < meshRenderers.Length; i++)
        {
            meshRenderers[i].enabled = true;
        }
        spawnIteractionPos.SetActive(true);
        if(canCook)
        PreparePasta();
    }
    private void PreparePasta()
    {
        currentCookingCounter++;
        
        if(currentCookingCounter == maxCookingReady - 1)
        {
          VisualEffect smokeObj = Instantiate(smokeEffect, spawnIteractionPos.transform);
          smokeObj.transform.position = transform.position;
        }
        if (currentCookingCounter >= maxCookingReady)
        {
            GameObject leaf = Instantiate(leafObj, spawnIteractionPos.transform);
            leaf.transform.position = spawnIteractionPos.transform.position;
            canCook = false;
            if (hasEspecialBehaviour)
            {
                EspecialBehaviour();
            }
        }

    }

    private void EspecialBehaviour()
    {
        StartCoroutine(GrowPasta());
    }
    IEnumerator GrowPasta()
    {
        float posToGrow = 0.71f;

        while(transform.localScale.z != posToGrow)
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, Mathf.Lerp(transform.localScale.z, posToGrow, 0.5f * Time.deltaTime));
            if(transform.localScale.z >= 0.70f)
            {
                break;
            }
            yield return null;
        }
    }
    public void CheckIsAlive()
    {
     

        if (shipLife <= 0)
        {
            playerMatchManager.shipCounter--;
            playerMatchManager.IsAlive();
            Destroy(gameObject);
        }
    }
}
