using System;
using System.Globalization;
using TMPro;
using UnityEngine;

public class TrackManager : MonoBehaviour
{
    public TextMeshProUGUI CurrentProgress;
    public TextMeshProUGUI Distance;
    public TextMeshProUGUI ArrivalTime;
    public TextMeshProUGUI CurrentTravelSpeed;
    public TextMeshProUGUI Visibility;
    public TextMeshProUGUI ExploreRadius;
    public TextMeshProUGUI PickupRadius;
    public TextMeshProUGUI MovingInfo;

    public GameObject Loader;


    public void ResetAllData()
    {
        CurrentTravelSpeed.text = string.Empty;
        ArrivalTime.text = string.Empty;
        CurrentProgress.text = "100%";
        Distance.text = string.Empty;
    }
    public void UpdateTravelSpeed(float speed)
    {
        string unit = DetermineSpeedUnit();
        string formattedSpeed = $"{ConvertSpeed(speed, unit)} {unit}";
        CurrentTravelSpeed.text = formattedSpeed;
    }

    public void UpdateDistance(float distance)
    {
        string unit = DetermineDistanceUnit();
        string formattedDistance = $"{ConvertDistance(distance, unit):F2} {unit}";
        Distance.text = formattedDistance;
    }

    public void UpdateArrivalTime(float arrivalSeconds)
    {
        string formattedTime = ConvertSecondsToTime(arrivalSeconds);
        ArrivalTime.text = formattedTime;
    }

    private string ConvertSecondsToTime(float seconds)
    {
        TimeSpan time = TimeSpan.FromSeconds(seconds);
        return string.Format("{0:D2}:{1:D2}:{2:D2}", time.Hours, time.Minutes, time.Seconds);

    }
    private string DetermineSpeedUnit()
    {
        CultureInfo cultureInfo = CultureInfo.CurrentCulture;
        string countryCode = cultureInfo.Name.Split('-')[1];

        // Check if the country code represents US or UK for mph, else default to km/h
        if (countryCode.Equals("US", StringComparison.OrdinalIgnoreCase) || countryCode.Equals("GB", StringComparison.OrdinalIgnoreCase))
        {
            return "mph";
        }

        return "km/h";
    }

    private string DetermineDistanceUnit()
    {
        CultureInfo cultureInfo = CultureInfo.CurrentCulture;
        string countryCode = cultureInfo.Name.Split('-')[1];

        // Check if the country code represents US or UK for miles, else default to kilometers
        if (countryCode.Equals("US", StringComparison.OrdinalIgnoreCase) || countryCode.Equals("GB", StringComparison.OrdinalIgnoreCase))
        {
            return "mi";
        }

        return "km";

    }
    private float ConvertSpeed(float speed, string unit)
    {
        if (unit == "mph")
        {
            return speed * 0.621371f;
        }
        return speed;
    }

    private float ConvertDistance(float distance, string unit)
    {
        if (unit == "miles")
        {
            return distance * 0.621371f;
        }
        return distance;
    }

}
