using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    //Esse script é um constructor ele cria parametros para serem usados em outras classes, nesse caso para criar grids
    private int width;
    private int height;
    private float cellSize;
    private Vector3 originPosition;
    //um multidimesion array para criar um sistema de coordenadas, para ser usado de base para a grid
    private int[,] gridArray;
    private TextMesh[,] debugTextArray;
    public Grid(int width, int height, float cellSize, Vector3 originPosition)
    {
        this.height = height;
        this.width = width;
        this.cellSize = cellSize;
        this.originPosition = originPosition;   

        gridArray = new int[width, height];
        debugTextArray = new TextMesh[width, height];

        //A função GetLength() serve para multidimesion arrays para escolher a dimension exata que voce quer.
        //Esse for para loopar entre todas as dimensões dos arrays
        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                debugTextArray[x, y] = CreateWorldText(null, gridArray[x, y].ToString(), GetworldPosition(x, y) + new Vector3(cellSize, cellSize) / 2f, 20, Color.white, TextAnchor.MiddleCenter, TextAlignment.Left, 1);
                Debug.DrawLine(GetworldPosition(x, y), GetworldPosition(x, y + 1), Color.white, 100f);
                Debug.DrawLine(GetworldPosition(x, y), GetworldPosition(x + 1, y), Color.white, 100f);
            }
        }
        Debug.DrawLine(GetworldPosition(0, height), GetworldPosition(width, height), Color.white, 100f);
        Debug.DrawLine(GetworldPosition(width, 0), GetworldPosition(width, height), Color.white, 100f);
        SetValue(2, 1, 35);
    }

    private Vector3 GetworldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize + originPosition;
    }

    private void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
    }

    public void SetValue(int x, int y, int value)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y] = value;
            debugTextArray[x, y].text = gridArray[x, y].ToString();
        }
    }
    public void SetValue(Vector3 worldPosition, int value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetValue(x, y, value);
        Debug.Log(x + "," + y);
    }
    public int GetValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }
        else
        {
            return 0;
        }

    }
    public int GetValue(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetValue(x, y);
    }


    private static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder)
    {
        GameObject go = new GameObject("World_text",typeof(TextMesh));
        Transform  transform = go.transform;
        transform.SetParent(parent,false);
        transform.localPosition = localPosition;
        TextMesh textMesh = go.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.fontSize = fontSize;
        textMesh.alignment = textAlignment;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        return textMesh;
    }
   
}
