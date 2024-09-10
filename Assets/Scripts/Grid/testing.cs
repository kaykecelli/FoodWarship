using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testing : MonoBehaviour
{
    private Grid grid;
    void Start()
    {
        grid = new Grid(4, 2, 10f,new Vector3(0,0));
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 vec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            vec.z = 0;
            grid.SetValue(vec, 56);
        }
    }



}
