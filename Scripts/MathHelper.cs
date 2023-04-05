using System;
using UnityEngine;

public class MathHelper
{
    public static Vector2 FromDistanceAndAngle(float distance, Vector2 from, float radians)
    {
        var x = distance * Math.Cos(radians);
        var y = distance * Math.Sin(radians);

        return new Vector2((float)x, (float)y);
    }
}