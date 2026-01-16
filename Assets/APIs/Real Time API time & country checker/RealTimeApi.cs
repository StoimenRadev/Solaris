using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class RealTimeApi : MonoBehaviour
{
    private string IPAddress;
    public LocationInfo Info;
    public float latitude;
    public float longitude;

    void Start()
    {
        StartCoroutine(GetRealTimeData());
    }

    private IEnumerator GetRealTimeData()
    {
        var www = new UnityWebRequest("https://api.ipify.org");
        www.downloadHandler = new DownloadHandlerBuffer();

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            yield break;
        }

        IPAddress = www.downloadHandler.text;
        StartCoroutine(GetCoordinates());
    }
    private IEnumerator GetCoordinates()
    {
        var www = new UnityWebRequest($"http://ip-api.com/json/" + IPAddress)
        {
            downloadHandler = new DownloadHandlerBuffer()
        };
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            yield break;
        }

        Info = JsonUtility.FromJson<LocationInfo>(www.downloadHandler.text);
        latitude = Info.lat;
        longitude = Info.lon;
    }
}

[Serializable]
public class  LocationInfo
{
    public string status;
    public string country;
    public string countryCode;
    public string region;
    public string regionName;
    public string city;
    public string zip;
    public float lat;
    public float lon;
    public string timezone;
    public string isp;
    public string org;
    public string asname;
    public string query;
}
