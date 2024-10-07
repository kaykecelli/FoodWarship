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
    [SerializeField] GameObject attackData;
    [SerializeField] private Grid enemyGrid;


    [Header("Bullet Variables")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject firePosition;

    private List<Vector3> attacksPosition =  new();
    private List<GameObject> attackMarkersGameObject = new();
    GameObject attackMarker;
    Vector3 mousePosition;
    Vector3Int gridPosition;
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
        if (attackCounter <= maxAttacksRound)
        {
            if (inputManager.IsPointerOverUI())
            {
                return;
            }


            attackMarker = Instantiate(cellIndicator, attackData.transform);
            attackMarker.transform.position = enemyGrid.CellToWorld(gridPosition);
            Renderer renderer = attackMarker.GetComponentInChildren<Renderer>();
            renderer.material.color = Color.red;
            attacksPosition.Add(enemyGrid.CellToWorld(gridPosition));
            attackCounter++;
        }
       
    }

    public void Attack()
    {
        for(int i = 0; i < attackData.transform.childCount; i++)
        {
           Destroy(attackData.transform.GetChild(i).gameObject);
        }
        attackCounter = 0;
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
                attacksPosition.RemoveAt(i);
                

                if (attacksPosition.Count >= 0)
                {
                    break;
                }

                yield return new WaitForSeconds(1f);
            }
        }
    }

    
}
