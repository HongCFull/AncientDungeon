using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathfExtension 
{
    public static float RoundFloatToDecimal(float num, int precision)
    {
        if (precision < 0)
            throw new Exception("Precision can't be negative");

        int truncateHelper = (int)Math.Pow(10, precision);
        return Mathf.Round(num * truncateHelper) / truncateHelper;
    }
}
