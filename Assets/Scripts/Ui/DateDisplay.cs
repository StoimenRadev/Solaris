using UnityEngine;
using TMPro;
using System;
using System.Globalization;

public class DateDisplay : MonoBehaviour
{
    private TextMeshProUGUI buttonText;

    // UTC at simulation start
    private DateTime startUtc;
    private DateTime startSystemTime;

    // Timer to update once per second
    private float timer = 0f;

    // English culture
    private CultureInfo englishCulture = new CultureInfo("en-US");

    void Awake()
    {
        buttonText = GetComponentInChildren<TextMeshProUGUI>();

        // Capture start UTC and system time
        startUtc = DateTime.UtcNow;
        startSystemTime = DateTime.Now;

        UpdateDateDisplay();
    }

    void Update()
    {
        timer += Time.unscaledDeltaTime;
        if (timer < 1f) return; // update once per second
        timer = 0f;

        UpdateDateDisplay();
    }

    private void UpdateDateDisplay()
    {
        // Calculate elapsed time since start
        TimeSpan elapsed = DateTime.Now - startSystemTime;
        DateTime simulatedUtc = startUtc.AddSeconds(elapsed.TotalSeconds);
        DateTime localTime = simulatedUtc.ToLocalTime();

        // Format date: "Feb 16, 2026"
        string formatted = localTime.ToString("MMM dd, yyyy", englishCulture);

        // Make the month uppercase
        string[] parts = formatted.Split(' ');
        parts[0] = parts[0].ToUpper();
        buttonText.text = string.Join(" ", parts);
    }
}
