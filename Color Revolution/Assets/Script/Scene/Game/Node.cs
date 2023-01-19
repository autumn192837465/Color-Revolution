using System;
using System.Collections;
using System.Collections.Generic;
using Kinopi.Enums;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CR.Game
{
    public class Node : MonoBehaviour
    {
        [SerializeField] private Transform towerRoot;
        [SerializeField] private MeshRenderer _meshRenderer;
        public bool HasTurret = false;
        public Action<Node> OnClickNode;
        public Turret PlacingTurret => placingTurret;
        private Turret placingTurret;
        


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
    
    }
    
}
