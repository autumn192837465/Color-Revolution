using System;
using System.Collections;
using System.Collections.Generic;
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
