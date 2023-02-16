
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

        public RGB(int red, int green, int blue)
        {
            RedValue = red;
            GreenValue = green;
            BlueValue = blue;
        }
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


        public static RGB operator *(RGB rgb, float a)
        {
            return new RGB((int)(rgb.RedValue * a), (int)(rgb.GreenValue * a),(int)(rgb.BlueValue * a));
        }
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

        public RGB ReduceHealth(RGB damage)
        {
            int redAmount = Mathf.Min(damage.RedValue, CurrentHealth.RedValue);
            int greenAmount = Mathf.Min(damage.RedValue, CurrentHealth.GreenValue);
            int blueAmount = Mathf.Min(damage.RedValue, CurrentHealth.BlueValue);
            CurrentHealth.RedValue -= damage.RedValue;
            CurrentHealth.GreenValue -= damage.GreenValue;
            CurrentHealth.BlueValue -= damage.BlueValue;
            if (CurrentHealth.RedValue < 0) CurrentHealth.RedValue = 0;
            if (CurrentHealth.GreenValue < 0) CurrentHealth.GreenValue = 0;
            if (CurrentHealth.BlueValue < 0) CurrentHealth.BlueValue = 0;

            return new RGB(redAmount, greenAmount, blueAmount);
        }
    }
}
