using System;
using System.Collections;
using System.Collections.Generic;
using CR.Game;
using Kinopi.Enums;
using Kinopi.Utils;
using UnityEngine;

public class MapManager : Singleton<MapManager>
{

    [SerializeField] private Node nodePrefab;
    [SerializeField] private Transform mapRoot;

    private const float tempOffset = 0.1f; 
    private int mapHeight;
    private int mapWidth;

    private Node[,] nodeMap;
    
    
    protected override void Awake()
    {
        base.Awake();
        if (isDuplicate) return;        
    }
    
    /// <summary>
    ///
    ///  length
    ///  y3
    ///  y2
    ///  y1
    ///  0 x1 x2 x3 x4 x5 width 
    /// </summary>
    /// <param name="rawMapData"></param>
    public void CreateMap(MapDataScriptableObject rawMapData)
    {
        foreach (Transform tf in mapRoot)
        {
            Destroy(tf.gameObject);
        }
        
        var nodeTypeMap = CRUtils.StringToNodeMap(rawMapData.mapData);
        mapHeight = nodeTypeMap.Count;
        mapWidth = nodeTypeMap[0].Count;
        

        nodeMap = new Node[mapWidth, mapHeight];

        
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                NodeType nodeType = nodeTypeMap[y][x]; 
                if (nodeType == NodeType.Empty)
                {
                    nodeMap[x, y] = null;
                    continue;
                }
                var node = Instantiate(nodePrefab, mapRoot);
                node.Initialize(nodeType);
                node.transform.position = new Vector3(x + tempOffset * x, 0, y + tempOffset * y);
                nodeMap[x, y] = node;
            }
        }
        
        // Set neighbor
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                if(nodeMap[x, y] is null)   continue;
                // up
                Node upNeighbor = IsAvailableCoordinate(x, y + 1) ? nodeMap[x, y + 1] : null;
                Node leftNeighbor = IsAvailableCoordinate(x - 1, y) ? nodeMap[x - 1, y] : null;
                Node rightNeighbor = IsAvailableCoordinate(x + 1, y) ? nodeMap[x + 1, y] : null;
                Node downNeighbor = IsAvailableCoordinate(x, y - 1) ? nodeMap[x, y - 1] : null;
                nodeMap[x, y].SetNeighbors(new NodeNeighbors(upNeighbor, leftNeighbor, rightNeighbor, downNeighbor));
            }
        }
    }

    private bool IsAvailableCoordinate(int x, int y)
    {
        if (x >= mapWidth) return false;
        if (x < 0) return false;
        if (y >= mapHeight) return false;
        if (y < 0) return false;
        return true;
    }
    
    #region AddUIEvent
    #endregion

    #region RemoveUIEvent
    #endregion
}