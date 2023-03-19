using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CR.Game
{
    public class CameraControllerUI : MonoBehaviour
    {
        [SerializeField] private Camera camera;
        [SerializeField] private Slider zoomSlider;
        [SerializeField] private MMTouchJoystick cameraJoyStick;

        private Vector3 cameraInitPosition;
        private float cameraMoveSpeed = 3f;
        private bool dragging = false;
        
        private void Awake()
        {
            zoomSlider.onValueChanged.AddListener(OnZoomSliderValueChange);
            cameraInitPosition = camera.transform.position;
        }

        

        void Start()
        {
        
        }

        public void InitializeUI()
        {

        }

        private void OnZoomSliderValueChange(float value)
        {
            camera.fieldOfView = zoomSlider.value;
        }

        public void OnBeginDragJoystick()
        {
            dragging = true;
        }
        
        public void OnEndDragJoystick()
        {
            dragging = false;
        }
        
        public void JoystickMovement(Vector2 movement)
        {
            if (!dragging) return;   
            movement *= cameraMoveSpeed * Time.deltaTime;
            camera.transform.position += new Vector3(movement.x, 0,  movement.y) ;

        }
       
    }    
}
