using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;
using Kinopi.Enums;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using CR.Model;

namespace Kinopi.Extensions
{
    public static class CRExtensions
    {
        public static void Sub(this RGB currentRgb, RGB rgb)
        {
            currentRgb.RedValue -= rgb.RedValue;
            currentRgb.GreenValue -= rgb.GreenValue;
            currentRgb.BlueValue -= rgb.BlueValue;
            if (currentRgb.RedValue < 0) currentRgb.RedValue = 0;
            if (currentRgb.GreenValue < 0) currentRgb.GreenValue = 0;
            if (currentRgb.BlueValue < 0) currentRgb.BlueValue = 0;
        }

        public static Vector3 XZPosition(this Vector3 vec) => new Vector3(vec.x, 0, vec.z);
    }
}

