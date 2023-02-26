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
        [SerializeField] private List<Renderer> rendererList;

        public TextMeshPro costText;
        public bool CanPlace;
        public int RouteCost = 0;
        public bool HasTurret => placingTurret != null;
        public Action<Node> OnClickNode;
        public Turret PlacingTurret => placingTurret;
        private Turret placingTurret;

        public NodeNeighbors Neighbors => neighbors;
        private NodeNeighbors neighbors;
        
         


        public Action<Node> OnMouseEnterNode;
        public Action<Node> OnMouseExitNode;
        
        public (int, int) Coord;


        public void Initialize(NodeType nodeType)
        {
            switch (nodeType)
            {
                
            }
        }


        private void OnMouseDown()
        {
            OnClickNode?.Invoke(this);
        }

        public void PlaceTurret(Turret turret)
        {
            placingTurret = turret;
            placingTurret.transform.SetParent(turretRoot);
            placingTurret.transform.localPosition = Vector3.zero;
            //SetColor(Color.cyan);
        }

        public void DestroyTurret()
        {
            Destroy(placingTurret.gameObject);
            placingTurret = null;
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
            if (!CanPlace)
            {
                foreach (var renderer in rendererList)
                {
                    renderer.material.color = Color.red;
                }
            }
            else
            {
                foreach (var renderer in rendererList)
                {
                    renderer.material.color = Color.white;
                }
            }
        }
        
        public void HidePlaceable()
        {
            foreach (var renderer in rendererList)
            {
                renderer.material.color = Color.white;
            }
        }


        public void SetHighlight()
        {
            foreach (var renderer in rendererList)
            {
                renderer.material.color = Color.gray;
            }
        }

        public void SetNormal()
        {
            foreach (var renderer in rendererList)
            {
                renderer.material.color = Color.white;
            }
        }



        public void ShowAttackRange()
        {
            if (placingTurret == null || placingTurret.TurretType != TurretType.Offensive) return;
            ((OffensiveTurret)PlacingTurret).ShowAttackRange();
        }
        
        public void HideAttackRange()
        {
            if (placingTurret == null || placingTurret.TurretType != TurretType.Offensive) return;
            ((OffensiveTurret)PlacingTurret).HideAttackRange();
        }

        private void OnMouseEnter()
        {
            OnMouseEnterNode?.Invoke(this);
        }

        private void OnMouseExit()
        {
            OnMouseExitNode?.Invoke(this);
        }
    }
    
}
