using System.Globalization;
using UnityEngine;

public class NumberFormater
{
    public string FormatTimeInTens(float timeOutput)
    {
        int minutes = Mathf.FloorToInt(timeOutput / 60);
        int seconds = Mathf.FloorToInt(timeOutput % 60);

        return string.Format("{00:00}:{01:00}", minutes, seconds);
    }
    public string FormatEnergyInThousands(int powerOutput)
    {
        if (powerOutput < 0)
        {
            // Handle negative values separately
            int absPowerOutput = Mathf.Abs(powerOutput);
            string formattedValue = FormatPositiveEnergyInThousands(absPowerOutput);
            return "-" + formattedValue;
        }
        else
        {
            // Handle positive values using existing method
            return FormatPositiveEnergyInThousands(powerOutput);
        }
    }
    public string FormatPositiveEnergyInThousands(int powerOutput)
    {
        if (powerOutput >= 1000000)
        {
            float MWValue = (float)powerOutput / 100000f;
            string formattedValue = MWValue.ToString("#.00", CultureInfo.InvariantCulture);

            if (formattedValue[^1] == '0')
            {
                formattedValue = formattedValue[..^1];

                // Check if the first decimal is also '0' and remove it
                if (formattedValue[^1] == '0')
                {
                    formattedValue = formattedValue[..^2];
                }
            }

            return formattedValue + " MW";
        }
        else if (powerOutput >= 1000)
        {
            float kWValue = (float)powerOutput / 1000f;
            string formattedValue = kWValue.ToString("#.00", CultureInfo.InvariantCulture);

            if (formattedValue[^1] == '0')
            {
                formattedValue = formattedValue[..^1];

                // Check if the first decimal is also '0' and remove it
                if (formattedValue[^1] == '0')
                {
                    formattedValue = formattedValue[..^2];
                }
            }

            return formattedValue + " kW";
        }
        else
        {
            return powerOutput.ToString("0", CultureInfo.InvariantCulture) + " W";
        }
    }

    public string FormatNumberInThousands(float quantity)
    {
        if (quantity >= 1000000)
        {
            float milionValue = quantity / 100000f;
            string formattedValue = milionValue.ToString("#.00", CultureInfo.InvariantCulture);

            if (formattedValue[^1] == '0')
            {
                formattedValue = formattedValue[..^1];

                // Check if the first decimal is also '0' and remove it
                if (formattedValue[^1] == '0')
                {
                    formattedValue = formattedValue[..^2];
                }
            }

            return formattedValue + " M";
        }
        else if (quantity >= 1000)
        {
            float thousandValue = quantity / 1000f;
            string formattedValue = thousandValue.ToString("#.00", CultureInfo.InvariantCulture);

            if (formattedValue[^1] == '0')
            {
                formattedValue = formattedValue[..^1];

                // Check if the first decimal is also '0' and remove it
                if (formattedValue[^1] == '0')
                {
                    formattedValue = formattedValue[..^2];
                }
            }

            return formattedValue + " K";
        }
        else
        {
            return quantity.ToString("0", CultureInfo.InvariantCulture);
        }
    }

}
