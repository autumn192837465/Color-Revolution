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
        public bool HasTower = false;
        public Action<Node> OnClickNode;


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


        public void PlaceTower(Tower tower)
        {
            tower.transform.SetParent(towerRoot);
            tower.transform.localPosition = Vector3.zero;
            HasTower = true;
        }
    
    }
    
}
