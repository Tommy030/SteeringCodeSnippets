using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using System;
using UnityEngine.UI;
using UnityEditor;
public class MapEditor : MonoBehaviour
{
    [Header("MapVariables")]
    public List<PrefabsTerrain> m_TerrainPrefabs;
    public int m_Selected;
    public Action<int> m_OnIndexChanged;
    public RawImage m_Image;

    public Camera m_camera;

    public static MapEditor Instance; //singleton

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    private void Start()
    {
        //Change de ui naar de Huidige Prefab die wordt gebruikt voor de map
        ChangeUI(m_Selected);
        //Subscribe aan de action
        m_OnIndexChanged += ChangeUI;
    }
    public void Update()
    {
        //Moet in edit mode zitten.
        if (Map.Instance.EditMode)
        {
            if (Input.GetMouseButtonDown(0))
            {
               //Normale Raycast 
                RaycastHit hit;
                Ray ray = m_camera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
      
                    Transform objectHit = hit.transform;
                    if(objectHit.GetComponent<GridClass>() != null)
                    {
                        //Verander de grid naar de geselecteerde prefab als het geraakt wordt.
                        objectHit.GetComponent<GridClass>().NewGrid();
                    }
                }
            }
            //Verander Prefab
            if (Input.GetKeyDown(KeyCode.A))
            {
                m_Selected--;
                m_Selected= Mathf.Clamp(m_Selected, 0, m_TerrainPrefabs.Count -1);
                ChangeUI(m_Selected);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                m_Selected++;
                m_Selected = Mathf.Clamp(m_Selected,0, m_TerrainPrefabs.Count -1);
                ChangeUI(m_Selected);
            }
        }
    }
    public void ChangeUI(int Indexnumber)
    {
        //Verander naar de een 2D view van unity van de asset (dus de preview)
       m_Image.texture = AssetPreview.GetAssetPreview(m_TerrainPrefabs[Indexnumber].Prefab);
    }
}
