using System;
using System.Collections;
using System.Collections.Generic;
using CR.Game;
using UnityEngine;

public class ObjectWorldCanvas : MonoBehaviour
{
    [SerializeField] private Canvas canvas;

    private void Awake()
    {
        canvas.worldCamera = GameManager.Instance.MainCamera;
    }

    void Start()
    {
            
    }
    
    void Update()
    {
        
    }
    

    private void OnValidate()
    {
        if (canvas == null) canvas = GetComponent<Canvas>();
    }
}