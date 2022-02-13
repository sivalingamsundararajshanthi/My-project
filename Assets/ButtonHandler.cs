using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Web;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    private const string URL = "https://api.darksky.net/forecast/8983bcdcfeb9492ce6ddbf6508428350/37.8267,-122.4233";
    private const string eventsURL = "https://api.seatgeek.com/2/events?client_id=MjU1MTA5MjJ8MTY0MzMxODE0OC4zNzI2MTg&client_secret=fe586f7b07607d5e9d90312bfe79c09b8678821f8e4a17db83925e810c5944c4&lat=38.969730&lon=-77.383873";

    public Text myText;
    public Text dayText;
    public Text currText;
    public Text typeText;
    public Text highText;
    public Text lowText;

    //Foursquare => fsq3FEOVUktJHshhzLfoVoIaFnUdifNt6QrcHFDR2JNgVtM=
    //predicthq => UBrVY-oDsXTvdZcY7EhwbAzkE6CC_cdDdo503Ai5-OI7Zj9QW4ILbg
    //seatgeek => fe586f7b07607d5e9d90312bfe79c09b8678821f8e4a17db83925e810c5944c4

    private GameObject layoutContainer;

    [SerializeField]
    private GameObject itemTemplatePrefab;

    public void click()
    {
        //GetIP address
        string ip = new WebClient().DownloadString("http://icanhazip.com/");

        //Get lat and longitude from IP
        string longurl = "https://api.ip2location.com/v2/?";
        var uriBuilder = new UriBuilder(longurl);
        var query = HttpUtility.ParseQueryString(uriBuilder.Query);
        query["ip"] = ip;
        query["key"] = "XH3T0TAFCC";
        query["package"] = "WS25";
        uriBuilder.Query = query.ToString();
        longurl = uriBuilder.ToString();

        StartCoroutine(GetLatLong(longurl, returnValue =>
                {
                    if(returnValue != null)
                    {
                        Debug.Log("Latitude => " + returnValue.latitude);
                        Debug.Log("Longitude => " + returnValue.longitude);

                        string longurl = "https://api.darksky.net/forecast/8983bcdcfeb9492ce6ddbf6508428350";
                        string url = longurl + "/" + returnValue.latitude + "," + returnValue.longitude;

                        //Set weather data in screen
                        StartCoroutine(GetWeatherData(url));

                        string eventsURL = "https://api.seatgeek.com/2/events?client_id=MjU1MTA5MjJ8MTY0MzMxODE0OC4zNzI2MTg&client_secret=fe586f7b07607d5e9d90312bfe79c09b8678821f8e4a17db83925e810c5944c4";
                        var uriBuilder = new UriBuilder(eventsURL);
                        var query = HttpUtility.ParseQueryString(uriBuilder.Query);
                        query["lat"] = returnValue.latitude.ToString();
                        query["lon"] = returnValue.longitude.ToString();
                        uriBuilder.Query = query.ToString();
                        longurl = uriBuilder.ToString();
                        StartCoroutine(getEvents(eventsURL));
                    }
                }
            )
        );

        //StartCoroutine(GetWeatherData(URL));
        //StartCoroutine(getEvents(eventsURL));
    }

    /**
        string longurl = "https://api.seatgeek.com/2/events?client_id=MjU1MTA5MjJ8MTY0MzMxODE0OC4zNzI2MTg&client_secret=fe586f7b07607d5e9d90312bfe79c09b8678821f8e4a17db83925e810c5944c4";
        var uriBuilder = new UriBuilder(longurl);
        var query = HttpUtility.ParseQueryString(uriBuilder.Query);
        query["lat"] = returnValue.latitude;
        query["lon"] = returnValue.longitude;
        uriBuilder.Query = query.ToString();
        longurl = uriBuilder.ToString();
    */


    private IEnumerator GetLatLong(string uri, System.Action<Location> callback = null)
    {
        Location location = null;
        using (UnityWebRequest request = UnityWebRequest.Get(uri))
        {
            yield return request.SendWebRequest();

            if (request.isNetworkError)
            {
                Debug.Log("Failed to get Weather data => " + request.error);
            }
            else
            {
                string text = request.downloadHandler.text;
                Location l = JsonUtility.FromJson<Location>(text);
                callback.Invoke(l);
            }
        }
    }

    private IEnumerator GetWeatherData(string uri)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(uri))
        { 
            yield return request.SendWebRequest();

            if (request.isNetworkError)
            {
                Debug.Log("Failed to get Weather data => " + request.error);
            }
            else
            {
                string text = request.downloadHandler.text;
                Weather weather = JsonUtility.FromJson<Weather>(text);
                myText.text = "Herndon";
                dayText.text = UnixTimeStampToDateTime(weather.daily.data[0].time).ToString();
                currText.text = weather.currently.apparentTemperature.ToString();
                typeText.text = weather.currently.summary;
                highText.text = weather.daily.data[0].temperatureHigh.ToString();
                lowText.text = weather.daily.data[0].temperatureLow.ToString();
            }
        }
    }

    private IEnumerator getEvents(string URL)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(URL))
        {
            request.SetRequestHeader("x-rapidapi-host", "nearby-tesla-superchargers.p.rapidapi.com");
            request.SetRequestHeader("x-rapidapi-key", "7cc280af9cmsh26e0726e50876e6p1d2259jsncf2caed3c0d1");
            yield return request.SendWebRequest();

            if(request.isNetworkError)
            {
                Debug.Log("Failed to get Events data => " + request.error);
            }
            else
            {
                string text = request.downloadHandler.text;
                Debug.Log("Nearby events => " + text);
                Near near = JsonUtility.FromJson<Near>(text);

                layoutContainer = GameObject.Find("/Canvas/ListContainer/ViewPort/LayoutContainer");

                layoutContainer.transform.DetachChildren();
                
                foreach(Result r in near.events) {
                    GameObject listItem = Instantiate(itemTemplatePrefab) as GameObject;
                    listItem.transform.SetParent(layoutContainer.transform, false);


                    Transform listItemTextTransform = listItem.transform.Find("ListItemText");
                    //Transform listItemVenueTransform = listItem.transform.Find("venueText");
                    //Transform listItemAddressTransform = listItem.transform.Find("addressText");


                    GameObject listItemText = listItemTextTransform.gameObject;
                    //GameObject listItemVenue = listItemVenueTransform.gameObject;
                    //GameObject listItemAddress = listItemAddressTransform.gameObject;


                    Text listItemTextComponent = listItemText.GetComponent<Text>();
                    //Text listItemVenueComponent = listItemVenue.GetComponent<Text>();
                    //Text listItemAddressComponent = listItemAddress.GetComponent<Text>();


                    listItemTextComponent.text = r.title;
                    //listItemVenueComponent.text = r.venue.name_v2;
                    //string address = r.venue.address + " " + r.venue.extended_address;
                    //listItemAddressComponent.text = address;
                }
            }
        }
    }

    /*

        public class ListManager : MonoBehaviour
        {
            private string apiEndpointUri = "https://mywebsite.net/api/GetListItems";
            private GameObject layoutContainer;

            [SerializeField]
            private GameObject itemTemplatePrefab;

            void Start()
            {
                layoutContainer = GameObject.Find("/Canvas/ListContainer/Viewport/LayoutContainer");
            }
        }
         */

    //private void createLayout(Tesla[] teslas)
    //{
    //    for(int i=0;i<teslas.Length;i++)
    //    {

    //    }
    //}

    public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
        return dateTime;
    }
}
