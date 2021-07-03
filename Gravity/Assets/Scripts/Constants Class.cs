using System;
using UnityEngine;

class Constants
{
    public const float epsilon = 0.05f;
    public readonly float[] lines = {6, 0, -6};

    public const int line1 = 6;
    public const int line2 = 0;
    public const int line3 = -6;

    public const double difference = 1.5E-10;

    public bool IsEqualsFloat(float num1, float num2)
    {
        return Mathf.Abs(num1 - num2) <= difference;
    }

    public bool IsEqualVector(Vector3 vec1, Vector3 vec2)
    {
        if (Mathf.Abs(vec1.x - vec2.x) <= difference)
        {
            if (Mathf.Abs(vec1.y - vec2.y) <= difference)
            {
                return Mathf.Abs(vec1.z - vec2.z) <= difference;
            }
            else
                return false;
        }
        else
            return false;
         
    }
}

