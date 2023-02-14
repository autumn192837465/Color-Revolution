using Kinopi.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
using Kinopi.Constants;

namespace Kinopi.Utils
{
    public static class Utils
    {        

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[Random.Range(0, s.Length)]).ToArray());
        }


        public static int GetGreaterThanZero(float value)
        {
            return Mathf.Max(1, Mathf.CeilToInt(value));
        }

        public static bool HitProbability(float chance)
        {
            return Random.Range(0f, 1f) <= chance;
        }

        public static bool HitProbability(int chance)
        {
            return Random.Range(0, 100) < chance;
        }
        
        public static Vector3 GetRandomVector(float length)
        {
            return new Vector3(Random.Range(-length, length), Random.Range(-length, length));
        }

        
    }
}