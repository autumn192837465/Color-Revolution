using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private bool isDragging = false;
    public Action<PointerEventData> OnBeginDragObject;
    public Action<PointerEventData> OnDraggingObject;
    public Action<PointerEventData> OnEndDragObject;
    

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        OnBeginDragObject?.Invoke(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(!isDragging) return;
        OnDraggingObject?.Invoke(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(!isDragging)  return;
        OnEndDragObject?.Invoke(eventData);
        isDragging = false;
    }
}