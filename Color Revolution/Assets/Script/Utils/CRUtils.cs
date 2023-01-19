﻿using System;
using System.Collections.Generic;
using System.Linq;
using Kinopi.Enums;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Kinopi.Utils
{
    public static class CRUtils
    {
        public static List<List<NodeType>> StringToNodeMap(string rawMap)
        {
            List<List<NodeType>> nodeMap = new List<List<NodeType>>();
            List<NodeType> row = new List<NodeType>();
            
            foreach (var c in rawMap)
            {
                if (c.Equals('\n'))
                {
                    nodeMap.Add(row);
                    row = new List<NodeType>();
                    continue;
                }
                row.Add(CharToNodeType(c));
            }
            nodeMap.Add(row);
            nodeMap.Reverse();
            return nodeMap;
        }
        
        public static NodeType CharToNodeType(char c)
        {
            switch (c)
            {
                case '0':
                    return NodeType.Empty;
                case '1':
                    return NodeType.Normal;
                case '2':
                    return NodeType.Start;
                case '3':
                    return NodeType.End;
                default:
                    throw new NotImplementedException();
            }
        }
    }
    

}