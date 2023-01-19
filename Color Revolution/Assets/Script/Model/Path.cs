using System.Collections.Generic;
using System.Collections.ObjectModel;
using CR.Game;
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

    }    
}
