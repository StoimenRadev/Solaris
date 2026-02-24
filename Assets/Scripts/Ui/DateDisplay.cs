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

    // English culture for consistent formatting
    private CultureInfo englishCulture = new CultureInfo("en-US");

    private void Awake()
    {
        buttonText = GetComponentInChildren<TextMeshProUGUI>();

        // Capture start UTC and system time
        startUtc = DateTime.UtcNow;
        startSystemTime = DateTime.Now;

        UpdateDateDisplay();
    }

    private void Update()
    {
        timer += Time.unscaledDeltaTime;
        if (timer < 1f) return; // update once per second

        timer = 0f;
        UpdateDateDisplay();
    }

    private void UpdateDateDisplay()
    {
        if (buttonText == null) return;

        // Calculate elapsed time
        TimeSpan elapsed = DateTime.Now - startSystemTime;
        DateTime simulatedUtc = startUtc.AddSeconds(elapsed.TotalSeconds);
        DateTime localTime = simulatedUtc.ToLocalTime();

        // Format: "MMM dd, yyyy" (e.g., "FEB 16, 2026")
        string formatted = localTime.ToString("MMM dd, yyyy", englishCulture);

        // Uppercase month
        string[] parts = formatted.Split(' ');
        parts[0] = parts[0].ToUpper();

        buttonText.text = string.Join(" ", parts);
    }

    // Override display with specific DateTime
    public void OverrideDate(DateTime newTime)
    {
        if (buttonText == null) return;

        DateTime localTime = newTime.ToLocalTime();
        string formatted = localTime.ToString("MMM dd, yyyy", englishCulture);

        string[] parts = formatted.Split(' ');
        parts[0] = parts[0].ToUpper();

        buttonText.text = string.Join(" ", parts);
    }
}