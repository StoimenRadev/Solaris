using UnityEngine;
using TMPro;
using System;

public class TimeDisplay : MonoBehaviour
{
    private TextMeshProUGUI buttonText;
    private DateTime startUtc;
    private DateTime startSystemTime;
    private float timer = 0f;

    [Header("Options")]
    public bool showText = true;

    // Exposed values for other scripts
    public float DayOfYear { get; private set; } // for orbit calculations
    public float FractionOfDay { get; private set; } // 0..1

    void Awake()
    {
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
        startUtc = DateTime.UtcNow;
        startSystemTime = DateTime.Now;
        UpdateTimeValues();
    }

    void Update()
    {
        timer += Time.unscaledDeltaTime;
        if (timer < 1f) return; // update once per second
        timer = 0f;

        UpdateTimeValues();
    }

    void UpdateTimeValues()
    {
        TimeSpan elapsed = DateTime.Now - startSystemTime;
        DateTime simulatedUtc = startUtc.AddSeconds(elapsed.TotalSeconds);
        DateTime localTime = simulatedUtc.ToLocalTime();

        // Only time fraction for smooth orbit
        FractionOfDay = (localTime.Hour + localTime.Minute / 60f + localTime.Second / 3600f) / 24f;
        DayOfYear = localTime.DayOfYear + FractionOfDay;

        // UI only shows HH:MM:SS
        if (showText && buttonText != null)
        {
            buttonText.text = localTime.ToString("HH:mm:ss");
        }
    }

    // Optional: force update externally
    public void ForceUpdate()
    {
        UpdateTimeValues();
    }
}
