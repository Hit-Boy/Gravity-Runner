using UnityEngine;

internal class Constants
{
    public const float Epsilon = 0.05f;

    public const double Difference = 1.5E-10;
    public static readonly float[] Lanes = {-6, 0, 6};

    public static bool IsEqualsFloat(float num1, float num2)
    {
        return Mathf.Abs(num1 - num2) <= Difference;
    }

    public static bool IsEqualVector(Vector3 vec1, Vector3 vec2)
    {
        if (!(Mathf.Abs(vec1.x - vec2.x) <= Difference)) return false;
        if (Mathf.Abs(vec1.y - vec2.y) <= Difference)
            return Mathf.Abs(vec1.z - vec2.z) <= Difference;
        return false;

    }

    public static bool FloatComparison(float num1, float num2, bool firstNumberBigger)
    {
        if (firstNumberBigger)
            return num1 - num2 > Difference;
        else
            return num2 - num1 > Difference;
    }
}