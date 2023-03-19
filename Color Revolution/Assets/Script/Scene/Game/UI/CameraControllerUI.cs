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
        private float moveSpeed = 0.055f;
        
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

        public void JoystickMovement(Vector2 movement)
        {
            
             movement *= moveSpeed;
            camera.transform.position += new Vector3(movement.x, movement.y);

        }
       
    }    
}
