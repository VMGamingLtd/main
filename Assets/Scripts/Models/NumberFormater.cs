using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;

public class NumberFormater
{
    public string FormatTimeInTens(float timeOutput)
    {
        return timeOutput.ToString("#.0", CultureInfo.InvariantCulture) + " s";
    }
    public string FormatEnergyInThousands(int powerOutput)
    {
        if (powerOutput >= 10000)
        {
            float kWValue = (float)powerOutput / 1000f;
            return kWValue.ToString("#.0", CultureInfo.InvariantCulture) + "kW";
        }
        else
        {
            return powerOutput.ToString("#,##0", CultureInfo.InvariantCulture) + "W";
        }
    }

}
