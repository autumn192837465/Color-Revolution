using System;
using CB.Model;
using Kinopi.Utils;
using UnityEngine;


[CreateAssetMenu(fileName = "MapData", menuName = "ScriptableObjects/Map Data", order = 1)]
public class MapDataScriptableObject : ScriptableObject
{
    [TextArea(20,20)]
    public string mapData;


    [ContextMenu("Check Map")]
    private void CheckMap()
    {
        var nodeMap = CRUtils.StringToNodeMap(mapData);

        if (nodeMap.Count == 0)
        {
            Debug.LogError("Map is empty!");
            return;
        }

        int length = nodeMap[0].Count;
        int index = 0;
        foreach (var row in nodeMap)
        {
            if (length != row.Count)
            {
                Debug.LogError($"Row length isn't the same at {index} row!");
                return;
            }

            index++;

            length = row.Count;
        }
        Debug.Log("Perfect!");
    }
}