using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.Mathematics;
using System;


[Serializable] 
public class PrefabsTerrain
{
    public GameObject Prefab;
    public bool Walkable;
    public int2 SizeXZ;
}
public class Map : MonoBehaviour
{
    //initial gridsize
 
    //Actual map
    [Header("Maps")]
    public GridTile[] GridMap; //The current map
    public static Map Instance; //  singleton 

    //Terrain
    [Header("Terrain")]
    public GameObject m_TerrainParent;//Hierarchy Parent
    public List<PrefabsTerrain> m_Terrain; //List of availible terrains
    public ScriptableMap SaveMap; //Load this map

    [Header("Editing")]
    public int2 m_Gridsize;//Gridsize With new map
    public bool EditMode = true; //editing
    public bool SpawnLoadedMap = false;//Load 
    public bool UpdateMap = false;//Update map to new prefabs.
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    private void Start()
    {
        if (SpawnLoadedMap)
        {
            SpawnInLoadedMap(SaveMap);
        }
       // UnityEditor.EditorUtility.SetDirty(SaveMap); // Saves map after runtime.
    }
    public void SpawnNewMap()
    {
        GridMap = new GridTile[m_Gridsize.x * m_Gridsize.y]; //Initialize map
        for (int x = 0; x < m_Gridsize.x; x++)
        {
            for (int y = 0; y < m_Gridsize.y; y++)
            {
                GridTile NewGrid = new GridTile();// Initialize new Tile
                PrefabsTerrain Prefab = m_Terrain[UnityEngine.Random.Range(0, m_Terrain.Count)]; //Randomize tile

                GameObject GO = Instantiate(Prefab.Prefab, new Vector3(3 * x, 0, 3 * y), quaternion.identity); //instantiate Tile
                GO.transform.SetParent(m_TerrainParent.transform);// SetParent so it looks better in hierarchy
                GO.name = NewGrid.x + "," + NewGrid.y; //Name Tile

                GridClass m_GridClass = GO.AddComponent<GridClass>(); //Add Gridclass

                //Assign Value's
                m_GridClass.ThisGrid = NewGrid;
                NewGrid.GridStart(x, y, Prefab.Walkable, Prefab.Prefab, CalculateIndex(x, y, m_Gridsize.x), Prefab.SizeXZ);
                GridMap[CalculateIndex(x, y, m_Gridsize.x)] = NewGrid;
            }
        }
        SaveMap.Pass(m_Gridsize, GridMap); //Pass this map to be saved
    }
    public void SpawnInLoadedMap(ScriptableMap MapObject)
    {
        m_Gridsize = MapObject.GridSize; //Gridsize
        GridMap = MapObject.Requestmap(); //Returns a Map
        for (int x = 0; x < MapObject.GridSize.x; x++)
        {
            for (int y = 0; y < MapObject.GridSize.y; y++)
            {
                GridTile Newgrid = GridMap[CalculateIndex(x,y,m_Gridsize.x)]; //Get Tile
                if (Newgrid.ThisGrid == null)
                {
                    Newgrid.ThisGrid = MapEditor.Instance.m_TerrainPrefabs[0].Prefab; //Assign this tile if the indextile is null to avoid errors.
                }
                GameObject GO = Instantiate(Newgrid.ThisGrid, new Vector3(3 * x, 0, 3 * y), quaternion.identity); //Instantiate
                GO.transform.SetParent(m_TerrainParent.transform);// SetParent so it looks better in hierarchy
                GO.AddComponent<GridClass>().ThisGrid = Newgrid; //Assigning Vale's
                GO.name = x + "," + y; //Name change
                Newgrid.LiveGrid = GO; //Assigns actual Grid in the Scene
                GridMap[Newgrid.indexNumber] = Newgrid; // Updates map.

                if (UpdateMap)
                {
                    foreach (var item in MapEditor.Instance.m_TerrainPrefabs)
                    {
                        if (item.Prefab == Newgrid.ThisGrid)
                        {
                            Newgrid.Walkable = item.Walkable;
                            //updates if tile is walkable.
                        }
                    }
                }
            }
        }
    }
    public void OnGridChange(GridTile grid,PrefabsTerrain Terrain,GameObject NewGrid,GameObject OldGrid)
    {
        GridTile NewTile = new GridTile(); //Grid is new tile

        NewTile.GridStart(grid.x, grid.y, Terrain.Walkable, Terrain.Prefab, CalculateIndex(grid.x, grid.y, m_Gridsize.x), Terrain.SizeXZ); //assigning values
        NewGrid.transform.SetParent(m_TerrainParent.transform);// SetParent so it looks better in hierarchy
        NewGrid.name = OldGrid.name;//setting name
        
        GridClass _Grid = NewGrid.AddComponent<GridClass>(); //Assigning value's
        _Grid.ThisGrid = NewTile; 
        
        GridMap[NewTile.indexNumber] = NewTile;//updates current map
        SaveMap.Pass(m_Gridsize, GridMap); //updates the saved map
        Destroy(OldGrid); // Destroys the old grid.
    }
    public int CalculateIndex(int x , int y, int width)
    {
        return x + y * width; //Not ussing a 2d array.
    }
}
[Serializable]
public class GridTile
{
    public int x;
    public int y;
    public int indexNumber;
    public bool Walkable;
    public GameObject ThisGrid;
    public GameObject LiveGrid;
    public int2 SizeXZ;
    public void GridStart(int m_x, int m_y, bool Iswalkable, GameObject GridPass, int index,int2 SizeXZ_)
    {
        ThisGrid = GridPass;
        x = m_x;
        y = m_y;
        SizeXZ = SizeXZ_;
        Walkable = Iswalkable;
        indexNumber = index;
    }
    public Vector3 ReturnGridWorldLocation()
    {
        return new Vector3(x * 3, 0, y * 3);
    }
    public int2 ReturnGridLocation()
    {
        return new int2(x, y);
    }
}


