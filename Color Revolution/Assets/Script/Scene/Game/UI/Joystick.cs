using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour
{
    [SerializeField] private RectTransform root;
    [SerializeField] private Draggable handle;

    private void Awake()
    {
        handle.OnBeginDragObject = OnBeginDragHandle;
        handle.OnDraggingObject = OnBeginDragHandle;
        handle.OnEndDragObject = OnBeginDragHandle;
    }

    private void OnBeginDragHandle(PointerEventData pointerEventData)
    {
        
    }
    
    private void OnDraggingHandle(PointerEventData pointerEventData)
    {
        
    }
    
    private void OnEndDragHandle(PointerEventData pointerEventData)
    {
        
    }
}