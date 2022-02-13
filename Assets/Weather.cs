using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Weather
{
    public float latitude;
    public float langitude;
    public string timeZone;
    public Current currently;
    public Minute minutely;
    public Hour hourly;
    public Daily daily;
    //public Flags flags;
}

[Serializable]
public class Current
{
    public long time;
    public string summary;
    public string icon;
    public int nearestStormDistance;
    public int nearestStormBearing;
    public int precipIntensity;
    public int precipProbability;
    public float temperature;
    public float apparentTemperature;
    public float dewPoint;
    public float humidity;
    public float pressure;
    public float windSpeed;
    public float windGust;
    public int cloudCover;
    public int uvIndex;
    public int visibility;
    public float ozone;
}

[Serializable]
public class Flags
{
    //public Sources sources;
    //public float nearest-station;
    public string units;
}

[Serializable]
public class Minute
{
    public string summary;
    public string icon;
    public MinuteData[] data;
}

[Serializable]
public class MinuteData
{
    public long time;
    public int precipIntensity;
    public int precipProbability;
}

[Serializable]
public class Hour
{
    public string summary;
    public string icon;
    public HourData[] data;
}

[Serializable]
public class HourData
{
    public long time;
    public string summary;
    public string icon;
    public int precipIntensity;
    public int precipProbability;
    public float temperature;
    public float apparentTemperature;
    public float dewPoint;
    public float humidity;
    public float pressure;
    public float windSpeed;
    public float windGust;
    public int windBearing;
    public int cloudCover;
    public int uvIndex;
    public int visibility;
    public float ozone;
}

[Serializable]
public class Daily
{
    public string summary;
    public string icon;
    public DailyData[] data;
}

[Serializable]
public class DailyData
{
    public long time;
    public string summary;
    public string icon;
    public long sunriseTime;
    public long sunsetTime;
    public float moonPhase;
    public int precipIntensity;
    public float precipIntensityMax;
    public long precipIntensityMaxTime;
    public int precipProbability;
    public float temperatureHigh;
    public long temperatureHighTime;
    public float temperatureLow;
    public long temperatureLowTime;
    public int apparentTemperatureHigh;
    public long apparentTemperatureHighTime;
    public int apparentTemperatureLow;
    public long apparentTemperatureLowTime;
    public float dewPoint;
    public float humidity;
    public float pressure;
    public float windSpeed;
    public float windGust;
    public int windBearing;
    public int cloudCover;
    public int uvIndex;
    public int visibility;
    public float ozone;
}