
using System;
using UnityEngine;
using Kinopi.Utils;

namespace CR.Model
{
    [Serializable]
    public class RGB
    {
    
        /*public RGB(int r, int g, int b)
        {
            OriginRedValue = RedValue = r;
            OriginGreenValue = GreenValue = g;
            OriginBlueValue = BlueValue = b;
        }*/
        public Color GetColor()
        {
            float sum = RedValue + GreenValue + BlueValue;
            if(sum == 0)    return  Color.black;
            
            float redRatio = RedValue / sum;
            float greenRatio = GreenValue / sum;
            float blueRatio = BlueValue / sum;
            return new Color(redRatio, greenRatio, blueRatio);
        }
        
        public int RedValue;
        public int GreenValue;
        public int BlueValue;
    }
    
    [Serializable]
    public class RGBHealth
    {
        public Color GetColor()
        {
            float redRatio = MaxHealth.RedValue == 0 ? 0 : (float)CurrentHealth.RedValue / MaxHealth.RedValue;
            float greenRatio = MaxHealth.RedValue == 0 ? 0 : (float)CurrentHealth.GreenValue / MaxHealth.GreenValue;
            float blueRatio = MaxHealth.RedValue == 0 ? 0 : (float)CurrentHealth.BlueValue / MaxHealth.BlueValue;
            return new Color(redRatio, blueRatio, greenRatio);
        }

        public bool IsDead =>
            CurrentHealth.RedValue == 0 && CurrentHealth.GreenValue == 0 && CurrentHealth.BlueValue == 0;
        public RGB CurrentHealth;
        public RGB MaxHealth;

        public void ReduceHealth(RGB damage)
        {
            CurrentHealth.RedValue -= damage.RedValue;
            CurrentHealth.GreenValue -= damage.GreenValue;
            CurrentHealth.BlueValue -= damage.BlueValue;
            if (CurrentHealth.RedValue < 0) CurrentHealth.RedValue = 0;
            if (CurrentHealth.GreenValue < 0) CurrentHealth.GreenValue = 0;
            if (CurrentHealth.BlueValue < 0) CurrentHealth.BlueValue = 0;
        }
    }
}
