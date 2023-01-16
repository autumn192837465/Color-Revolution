using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Kinopi.Utils
{
    public static class CRUtils
    {
        public static Color ToColor(RGB rgb)
        {
            int sum = rgb.Sum;
            return new Color((float)rgb.RedValue / sum, (float)rgb.GreenValue / sum, (float)rgb.BlueValue / sum);
        }
        
    }
}