using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CR.Game
{
    public class CameraControllerUI : MonoBehaviour
    {
        [SerializeField] private Camera camera;
        [SerializeField] private Slider zoomSlider;
        

        
        
        private void Awake()
        {
            zoomSlider.onValueChanged.AddListener(OnZoomSliderValueChange);
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
    }    
}
