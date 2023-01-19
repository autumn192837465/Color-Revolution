using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CR.Game;
using CR.Model;
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

    public Node[,] nodeMap;
    public Node startNode;
    public Node endNode;
    public ReadOnlyCollection<Path> AllPaths => allPaths.AsReadOnly();
    private List<Path> allPaths = new List<Path>();
    
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
                node.Coord = (x, y);
                
                if (nodeType == NodeType.Start)
                {
                    startNode = node;
                }
                else if (nodeType == NodeType.End)
                {
                    endNode = node;
                }
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
        
        SetNodeCost();
        foreach (var node in nodeMap)
        {
            node?.ShowCost();
        }
        
        CalculateAllPossiblePath();
    }

    private void CalculateAllPossiblePath()
    {
        allPaths = new List<Path>();
        Stack<Node> stack = new Stack<Node>();
        FindPath(startNode, stack, allPaths);
    }
    
    private void SetNodeCost()
    {
        HashSet<Node> visited = new HashSet<Node>();
        SortedList<int, Node> sortedList = new SortedList<int, Node>(new CRUtils.DuplicateKeyComparator<int>());
        
        startNode.RouteCost = 0;
        sortedList.Add(0, startNode);
        while (sortedList.Count > 0)
        {
            Node visitingNode = sortedList.First().Value;
            sortedList.RemoveAt(0);
            if (visitingNode == endNode) continue;
            if (visited.Contains(visitingNode)) continue;
            visited.Add(visitingNode);
            
            int cost = visitingNode.RouteCost + 1;

            

            foreach (var neighborNode in visitingNode.Neighbors.AllNeighbors)
            {
                if (neighborNode != null && !visited.Contains(neighborNode))
                {
                    neighborNode.RouteCost = neighborNode.RouteCost == Int32.MaxValue ? cost : Mathf.Min(cost, neighborNode.RouteCost);
                    sortedList.Add(neighborNode.RouteCost, neighborNode);
                }
            }
        }
    }


    private void FindPath(Node currentNode, Stack<Node> stack, List<Path> possiblePath)
    {
        stack.Push(currentNode);
        if (currentNode == endNode)
        {
            Path path = new Path(stack.ToList());
            possiblePath.Add(path);
            return;
        }

        int cost = currentNode.RouteCost;

        foreach (var neighborNode in currentNode.Neighbors.AllNeighbors)
        {
            if (neighborNode != null && neighborNode.RouteCost ==  cost + 1)
            {
                FindPath(neighborNode, stack, possiblePath);
            }    
        }
        

        stack.Pop();

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