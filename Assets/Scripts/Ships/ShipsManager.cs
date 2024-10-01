using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipsManager : MonoBehaviour
{
    private MeshRenderer[] meshRenderers;
    private PlayerMatchManager playerMatchManager;
    public int shipLife;
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
        playerMatchManager.OnEnableShips -= EnableShips;
        playerMatchManager.OnDisableShips -= DisableShips;
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
    }
    public void EnableShips()
    {
        GetAllMeshRender();
        for (int i = 0; i < meshRenderers.Length; i++)
        {
            meshRenderers[i].enabled = true;
        }
    }
    public void CheckIsAlive()
    {
        if(shipLife == 0)
        {
            Destroy(gameObject);
        }
    }
}
