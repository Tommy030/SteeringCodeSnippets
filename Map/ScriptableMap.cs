using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using System;
[CreateAssetMenu(fileName = "Map", menuName = "Create New Map")]
[Serializable]
public class ScriptableMap : ScriptableObject
{
    public int2 GridSize;
    public GridTile[] Map;
    public void Pass(int2 aGridSize, GridTile[] TheMap)
    {
        GridSize = aGridSize;
        Map = TheMap;
    }
    public GridTile[] Requestmap()
    {
        return Map;
    }
}
