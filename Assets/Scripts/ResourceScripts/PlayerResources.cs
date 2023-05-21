using UnityEngine;
public static class PlayerResources
{
    public static string PlayerOxygen = "00:00:00:00";
    public static string PlayerWater = "00:00:00:00";
    public static string PlayerEnergy = "00:00:00:00";
    public static string PlayerHunger = "00:00:00:00";

    public static string FormatTime(int days, int hours, int minutes, int seconds)
    {
        return string.Format("{0:00}:{1:00}:{2:00}:{3:00}", days, hours, minutes, seconds);
    }

    public static string GetCurrentResourceTime(string currentResourceSet)
    {
        return FormatTimer(currentResourceSet);
    }

    public static string ReduceCurrentResourceTime(ref string currentResourceSet)
    {
        string[] currentTimeParts = currentResourceSet.Split(':');
        int currentDays = int.Parse(currentTimeParts[0]);
        int currentHours = int.Parse(currentTimeParts[1]);
        int currentMinutes = int.Parse(currentTimeParts[2]);
        int currentSeconds = int.Parse(currentTimeParts[3]);

        currentSeconds--;

        if (currentSeconds < 0)
        {
            currentMinutes--;
            currentSeconds = 59;

            if (currentMinutes < 0)
            {
                currentHours--;
                currentMinutes = 59;

                if (currentHours < 0)
                {
                    currentDays--;
                    currentHours = 23;

                    if (currentDays < 0)
                    {
                        currentDays = 0;
                        currentHours = 0;
                        currentMinutes = 0;
                        currentSeconds = 0;
                    }
                }
            }
        }

        currentResourceSet = FormatTime(currentDays, currentHours, currentMinutes, currentSeconds);
        return currentResourceSet;
    }

    public static string AddCurrentResourceTime(ref string currentResourceSet, int days, int hours, int minutes, int seconds)
    {
        string[] currentTimeParts = currentResourceSet.Split(':');
        int currentDays = int.Parse(currentTimeParts[0]);
        int currentHours = int.Parse(currentTimeParts[1]);
        int currentMinutes = int.Parse(currentTimeParts[2]);
        int currentSeconds = int.Parse(currentTimeParts[3]);

        currentDays += days;
        currentHours += hours;
        currentMinutes += minutes;
        currentSeconds += seconds;

        if (currentSeconds >= 60)
        {
            currentMinutes += currentSeconds / 60;
            currentSeconds %= 60;
        }
        if (currentMinutes >= 60)
        {
            currentHours += currentMinutes / 60;
            currentMinutes %= 60;
        }
        if (currentHours >= 24)
        {
            currentDays += currentHours / 24;
            currentHours %= 24;
        }

        currentResourceSet = FormatTime(currentDays, currentHours, currentMinutes, currentSeconds);
        return currentResourceSet;
    }

    public static string ReduceCurrentResourceTime(ref string currentResourceSet, int days, int hours, int minutes, int seconds)
    {
        string[] currentTimeParts = currentResourceSet.Split(':');
        int currentDays = int.Parse(currentTimeParts[0]);
        int currentHours = int.Parse(currentTimeParts[1]);
        int currentMinutes = int.Parse(currentTimeParts[2]);
        int currentSeconds = int.Parse(currentTimeParts[3]);

        currentDays -= days;
        currentHours -= hours;
        currentMinutes -= minutes;
        currentSeconds -= seconds;

        if (currentSeconds < 0)
        {
            currentMinutes -= Mathf.CeilToInt(Mathf.Abs(currentSeconds) / 60f);
            currentSeconds = 60 - Mathf.Abs(currentSeconds) % 60;
        }
        if (currentMinutes < 0)
        {
            currentHours -= Mathf.CeilToInt(Mathf.Abs(currentMinutes) / 60f);
            currentMinutes = 60 - Mathf.Abs(currentMinutes) % 60;
        }
        if (currentHours < 0)
        {
            currentDays -= Mathf.CeilToInt(Mathf.Abs(currentHours) / 24f);
            currentHours = 24 - Mathf.Abs(currentHours) % 24;
        }

        if (currentDays < 0)
        {
            currentDays = 0;
            currentHours = 0;
            currentMinutes = 0;
            currentSeconds = 0;
        }

        currentResourceSet = FormatTime(currentDays, currentHours, currentMinutes, currentSeconds);
        return currentResourceSet;
    }
    private static string FormatTimer(string timer)
    {
        string[] timeParts = timer.Split(':');
        int days = int.Parse(timeParts[0]);
        int hours = int.Parse(timeParts[1]);
        int minutes = int.Parse(timeParts[2]);
        int seconds = int.Parse(timeParts[3]);

        string formattedTimer = "";

        if (days > 0)
            formattedTimer += string.Format("{0}d ", days);

        if (hours > 0 || days > 0)
            formattedTimer += string.Format("{0}h ", hours);

        if (minutes > 0 || hours > 0 || days > 0)
            formattedTimer += string.Format("{0}m ", minutes);

        if (seconds > 0 || minutes > 0 || hours > 0 || days > 0)
            formattedTimer += string.Format("{0}s", seconds);

        return formattedTimer.Trim();
    }
}
