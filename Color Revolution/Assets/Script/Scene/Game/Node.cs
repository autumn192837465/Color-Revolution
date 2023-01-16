using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        print("Down");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        print("up");
    }

    private void OnMouseDown()
    {
        print("Down");
    }
    
}
