using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathHelpers
{
    public static float ConvertRange(
        float originalStart, float originalEnd, // original range
        float newStart, float newEnd, // desired range
        float value) // value to convert
    {
        float scale = (newEnd - newStart) / (originalEnd - originalStart);
        return newStart + ((value - originalStart) * scale);
    }
}
