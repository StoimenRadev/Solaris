using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements; // For UI Toolkit

[Serializable]
public class TimeZoneDBResponse
{
    public string status;
    public string message;
    public string formatted;
}

public class TimeFromCoordinatesUIToolkit : MonoBehaviour
{
    public string apiKey = "YOUR_API_KEY";
    public float latitude;
    public float longitude;

    private DateTime currentTime;
    private bool timeLoaded = false;
    private Label timeLabel;

    void Start()
    {
        // Get the root VisualElement from the UIDocument
        var root = GetComponent<UIDocument>().rootVisualElement;

        // Find your Label by name (set it in the UXML)
        timeLabel = root.Q<Label>("TimeLabel");

        // Start the API request
        StartCoroutine(GetTimeFromCoordinates(latitude, longitude));
    }

    void Update()
    {
        if (timeLoaded && timeLabel != null)
        {
            currentTime = currentTime.AddSeconds(Time.deltaTime);
            timeLabel.text = currentTime.ToString("HH:mm:ss");
        }
    }

    IEnumerator GetTimeFromCoordinates(float lat, float lon)
    {
        string url = $"http://api.timezonedb.com/v2.1/get-time-zone?key={apiKey}&format=json&by=position&lat={lat}&lng={lon}";

        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                if (timeLabel != null) timeLabel.text = "Failed to get time!";
                yield break;
            }

            TimeZoneDBResponse response = JsonUtility.FromJson<TimeZoneDBResponse>(www.downloadHandler.text);

            if (response.status == "OK")
            {
                currentTime = DateTime.Parse(response.formatted);
                timeLoaded = true;
            }
            else
            {
                if (timeLabel != null) timeLabel.text = "API error: " + response.message;
            }
        }
    }
}
