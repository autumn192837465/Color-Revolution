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
         
    }
    
    public class Node : MonoBehaviour
    {
        [SerializeField] private Transform towerRoot;
        [SerializeField] private MeshRenderer _meshRenderer;

        public TextMeshPro costText;
        public int RouteCost = 0;
        public bool HasTurret = false;
        public Action<Node> OnClickNode;
        public Turret PlacingTurret => placingTurret;
        private Turret placingTurret;

        public NodeNeighbors Neighbors => neighbors;
        private NodeNeighbors neighbors;
        

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
                    
                    break;
                case NodeType.Start:
                    _meshRenderer.material.color = Color.white;
                    break;
                case NodeType.End:
                    _meshRenderer.material.color = Color.red;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(nodeType), nodeType, null);
            }

            RouteCost = int.MaxValue;
        }


        private void OnMouseDown()
        {
            OnClickNode?.Invoke(this);
        }


        public void PlaceTower(Turret turret)
        {
            placingTurret = turret;
            turret.transform.SetParent(towerRoot);
            turret.transform.localPosition = Vector3.zero;
            HasTurret = true;
            _meshRenderer.material.color = Color.black;
        }

        public void SetNeighbors(NodeNeighbors nodeNeighbors)
        {
            neighbors = nodeNeighbors;
        }

        public void ShowCost()
        {
            costText.text = RouteCost.ToString();
        }
    }
    
}
