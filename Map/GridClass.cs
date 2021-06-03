using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
public class GridClass : MonoBehaviour
{
  
    public GridTile ThisGrid;
    public void NewGrid()
    {
        GameObject GO = MapEditor.Instance.m_TerrainPrefabs[MapEditor.Instance.m_Selected].Prefab;
        GameObject NewGrid = Instantiate(GO, transform.position,quaternion.identity);
        Map.Instance.OnGridChange(ThisGrid, MapEditor.Instance.m_TerrainPrefabs[MapEditor.Instance.m_Selected], NewGrid,gameObject);
    }
}
