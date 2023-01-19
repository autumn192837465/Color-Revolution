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
                    continue;
                }
                var node = Instantiate(nodePrefab, mapRoot);
                node.Initialize(nodeType);
                node.transform.position = new Vector3(x + tempOffset * x, 0, y + tempOffset * y);
                nodeMap[x, y] = node;
            }
        }
        
        // Set neighbor
    }
    
    #region AddUIEvent
    #endregion

    #region RemoveUIEvent
    #endregion
}