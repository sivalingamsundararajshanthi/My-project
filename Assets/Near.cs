using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Near
{
    public Result[] events;
}

[Serializable]
public class Result
{
    public string type;
    public string datetime_utc;
    public Venue venue;
    public string datetime_local;
    public string title;
}

[Serializable]
public class Venue
{
    public string state;
    public string name_v2;
    public string postal_code;
    public string address;
    public string country;
    public string city;
    public string display_location;
    public string extended_address;
}