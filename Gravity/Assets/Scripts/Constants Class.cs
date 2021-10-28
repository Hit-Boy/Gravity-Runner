using UnityEngine;

internal class Constants
{
    public const float Epsilon = 0.05f;

    public const int Line1 = 6;
    public const int Line2 = 0;
    public const int Line3 = -6;

    public const double Difference = 1.5E-10;
    public readonly float[] Lines = {6, 0, -6};

    public bool IsEqualsFloat(float num1, float num2)
    {
        return Mathf.Abs(num1 - num2) <= Difference;
    }

    public bool IsEqualVector(Vector3 vec1, Vector3 vec2)
    {
        if (!(Mathf.Abs(vec1.x - vec2.x) <= Difference)) return false;
        if (Mathf.Abs(vec1.y - vec2.y) <= Difference)
            return Mathf.Abs(vec1.z - vec2.z) <= Difference;
        return false;

    }
}