using System.Collections.Generic;
using System.Collections.ObjectModel;
using CR.Game;
using Kinopi.Utils;
using Unity.VisualScripting;
using UnityEngine;


namespace CR.Model
{
    public class Path
    {
        public Path(List<Node> nodes)
        {
            Nodes = nodes.AsReadOnly();
        }

        
        public ReadOnlyCollection<Node> Nodes;
        public Node StartNode => Nodes[0];
        public Node EndNode => Nodes[^1];
        
        public Vector3 GetDirection(int startIndex)
        {
            if (startIndex + 1 >= Nodes.Count) return Vector3.zero;

            return CRUtils.GetXZDirection(Nodes[startIndex].transform.position, Nodes[startIndex + 1].transform.position);
        }

    }    
}
