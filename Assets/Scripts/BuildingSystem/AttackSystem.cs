using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class AttackSystem : MonoBehaviour
{
    [SerializeField] private GameObject mouseIndicator, cellIndicator;
    [SerializeField] private int maxAttacksRound;
    [SerializeField] private InputManager inputManager;
   
    [SerializeField] private Grid enemyGrid;
    [SerializeField] private CinemachineVirtualCamera[] vCameras;


    [Header("Bullet Variables")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject firePosition;

    private List<Vector3> attacksPosition =  new();
    private List<GameObject> attackMarkersGameObject = new();
    GameObject attackMarker;
    Vector3 mousePosition;
    Vector3Int gridPosition;
    private int index = 1;
    private int attackCounter;
    private void OnEnable()
    {
        inputManager.OnClicked += MarkAttackPosition;
    }
    private void OnDisable()
    {
        inputManager.OnClicked -= MarkAttackPosition;
    }


    private void FixedUpdate()
    {
        mousePosition = inputManager.GetAttackMousePosition();
        gridPosition = enemyGrid.WorldToCell(mousePosition);
        mouseIndicator.transform.position = mousePosition;
        cellIndicator.transform.position = enemyGrid.CellToWorld(gridPosition);
    }

    private void MarkAttackPosition()
    {
        if (inputManager.IsPointerOverUI())
        {
            return;
        }

        if (attackCounter <= maxAttacksRound)
        {
            attackMarker = Instantiate(cellIndicator, enemyGrid.CellToWorld(gridPosition), Quaternion.identity);
            Renderer renderer = attackMarker.GetComponentInChildren<Renderer>();
            renderer.material.color = Color.red;
            attacksPosition.Add(enemyGrid.CellToWorld(gridPosition));
            //attackMarkersGameObject.Add(attackMarker);
        }
       
    }

    public void Attack()
    {

        StartCoroutine(RealizeAttacks());
    }

    private IEnumerator RealizeAttacks()
    {
        while(attacksPosition.Count > 0)
        {
            for (int i = 0; i < attacksPosition.Count; i++)
            {
                GameObject bullet = Instantiate(bulletPrefab, firePosition.transform.position, Quaternion.identity);
                bullet.GetComponent<Bullet>().pointB = attacksPosition[i];
                bullet.GetComponent<Bullet>().CallShoot();
                Debug.Log(attacksPosition[i]);
                attacksPosition.RemoveAt(i);
                //Destroy(attackMarkersGameObject[0]);
                // attackMarkersGameObject.RemoveAt(0);

                if (attacksPosition.Count >= 0)
                {
                    break;
                }

                yield return new WaitForSeconds(1f);
            }
        }
    }

    public void ChangeCamera()
    {
        if(index >= vCameras.Length)
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

        index++;
    }
    
}
