using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataGrid 
{
    Dictionary<Vector3Int, PlacementData> placedObjects = new Dictionary<Vector3Int, PlacementData>();
    public void AddObjectAt(Vector3Int gridPosition, Vector2 objectSize, int ID, int placedObjectIndex)
    {
        List<Vector3Int> positionToOccupy = CalculatePosition(gridPosition, objectSize);
        PlacementData data = new PlacementData(positionToOccupy,ID,placedObjectIndex);
        foreach (var pos in positionToOccupy)
        {
            if (placedObjects.ContainsKey(pos))
            {
                throw new Exception($"Dictionary already contains this cell position");
            }
            placedObjects[pos] = data;
        }

    }

    private List<Vector3Int> CalculatePosition(Vector3Int gridPosition, Vector2 objectSize)
    {
       List<Vector3Int> returnVal = new List<Vector3Int>();
        for (int x = 0; x < objectSize.x; x++)
        {
            for (int y = 0; y < objectSize.y; y++)
            {
                returnVal.Add(gridPosition + new Vector3Int(x, 0, y));
            }
        }
        return returnVal;
    }
    public bool CanPlaceObjectAt(Vector3Int gridPosition, Vector2 objectSize)
    {
        List<Vector3Int> positionToOccupy = CalculatePosition(gridPosition,objectSize);
        foreach (var pos in positionToOccupy)
        {
            if (placedObjects.ContainsKey(pos))
            {
                return false;
            }
        }
        return true;
    }
}

public class PlacementData
{
    public List<Vector3Int> occupiedPosition;
    public int ID { get; private set; }
    public int PlacedObjectIndex{ get; private set; }

    public PlacementData (List<Vector3Int> occupiedPosition, int iD, int placedObjectIndex)
    {
        this.occupiedPosition = occupiedPosition;
        ID = iD;
        PlacedObjectIndex = placedObjectIndex;
    }
}