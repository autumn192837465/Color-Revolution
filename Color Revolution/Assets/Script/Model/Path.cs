using System.Collections.Generic;
using CR.Game;
using UnityEngine;


namespace CR.Model
{
    public class Path
    {
        public Path(List<Node> nodes)
        {
            points = nodes;
        }

        public readonly List<Node> points;

    }    
}
