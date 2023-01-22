using System;
using System.Collections;
using System.Collections.Generic;
using Kinopi.Enums;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CR.Game
{
    public class NodeNeighbors
    {
        public NodeNeighbors(Node up, Node left, Node right, Node down)
        {
            UpNode = up;
            LeftNode = left;
            RightNode = right;
            DownNode = down;
        }
        public readonly Node UpNode;
        public readonly Node LeftNode;
        public readonly Node RightNode;
        public readonly Node DownNode;

        public List<Node> AllNeighbors => new() { UpNode, LeftNode, RightNode, DownNode };

    }
    
    public class Node : MonoBehaviour
    {
        [SerializeField] private Transform turretRoot;
        

        public TextMeshPro costText;
        public bool CanPlace;
        public int RouteCost = 0;
        public bool HasTurret => placingTurret != null;
        public Action<Node> OnClickNode;
        public Turret PlacingTurret => placingTurret;
        private Turret placingTurret;

        public NodeNeighbors Neighbors => neighbors;
        private NodeNeighbors neighbors;

        public (int, int) Coord;

        
        private void Awake()
        {

        }

        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void Initialize(NodeType nodeType)
        {
            switch (nodeType)
            {
                case NodeType.Empty:
                    Destroy(gameObject);
                    break;
                case NodeType.Normal:
                    SetColor(Color.white);
                    break;
                case NodeType.Start:
                    SetColor(Color.red);
                    break;
                case NodeType.End:
                    SetColor(Color.blue);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(nodeType), nodeType, null);
            }
        }


        private void OnMouseDown()
        {
            OnClickNode?.Invoke(this);
        }


        public void PlaceTower(Turret turret)
        {
            placingTurret = turret;
            turret.transform.SetParent(turretRoot);
            turret.transform.localPosition = Vector3.zero;
            //SetColor(Color.cyan);
        }

        public void SetNeighbors(NodeNeighbors nodeNeighbors)
        {
            neighbors = nodeNeighbors;
        }

        public void ShowCost()
        {
            costText.text = RouteCost.ToString();
        }

        public void ShowPlaceable()
        {
            SetColor(CanPlace ? Color.white : Color.gray);
        }
        
        public void HidePlaceable()
        {
            SetColor(Color.white);
        }
        


        private void SetColor(Color color)
        {
            foreach (var renderer in GetComponentsInChildren<Renderer>())
            {
                renderer.material.color = color;
            }
        }
        
    }
    
}
