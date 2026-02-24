using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class TimelineManager : MonoBehaviour
{
    [Header("References")]
    public TimeDisplay timeDisplay;
    public DateDisplay dateDisplay;
    public Slider timelineSlider;
    public TextMeshProUGUI rateText;

    [Header("Buttons")]
    public Button pastButton;
    public Button pauseButton;
    public Button futureButton;
    public Button resetButton;

    [Header("Planets (optional)")]
    public Transform[] planets;       // Assign Earth, Jupiter, Saturn, etc.
    public float[] orbitalPeriods;    // in seconds for one full orbit

    private DateTime currentSimTime;
    private bool isPaused = false;

    // Base time speeds in seconds per real second
    private float[] baseRates = new float[]
    {
        60f,       // 1 MIN
        300f,      // 5 MIN
        900f,      // 15 MIN
        1800f,     // 30 MIN
        3600f,     // 1 H
        43200f,    // 12 H
        86400f,    // 1 DAY
        2.628e6f,  // 1 MONTH
        3.154e7f   // 1 YEAR
    };

    private string[] labels = new string[]
    {
        "1 MIN",
        "5 MIN",
        "15 MIN",
        "30 MIN",
        "1 H",
        "12 H",
        "1 DAY",
        "1 MONTH",
        "1 YEAR"
    };

    private int sliderMiddleIndex;    // Index of REAL RATE
    private int currentSliderValue;   // Slider current value

    private void Start()
    {
        currentSimTime = DateTime.Now;

        // Slider setup
        sliderMiddleIndex = 9; // REAL RATE at center
        if (timelineSlider != null)
        {
            timelineSlider.minValue = 0;
            timelineSlider.maxValue = 18; // 9 past + REAL RATE + 9 future
            timelineSlider.wholeNumbers = true;
            timelineSlider.value = sliderMiddleIndex;
            currentSliderValue = sliderMiddleIndex;
            timelineSlider.onValueChanged.AddListener(OnSliderChanged);
        }

        // Button listeners
        if (pastButton != null) pastButton.onClick.AddListener(() => MoveSlider(-1));
        if (futureButton != null) futureButton.onClick.AddListener(() => MoveSlider(1));
        if (pauseButton != null) pauseButton.onClick.AddListener(TogglePause);
        if (resetButton != null) resetButton.onClick.AddListener(ResetTimeline);

        UpdateRateText();
        UpdatePlanets(currentSimTime); // initialize planet positions
    }

    private void Update()
    {
        if (!isPaused)
        {
            // Advance simulation time
            float speed = GetCurrentSpeed();
            currentSimTime = currentSimTime.AddSeconds(speed * Time.unscaledDeltaTime);

            // Update displays
            if (timeDisplay != null) timeDisplay.OverrideTime(currentSimTime);
            if (dateDisplay != null) dateDisplay.OverrideDate(currentSimTime);

            // Update planets
            UpdatePlanets(currentSimTime);
        }
    }

    private void MoveSlider(int delta)
    {
        currentSliderValue += delta;
        currentSliderValue = Mathf.Clamp(currentSliderValue, 0, 18);
        if (timelineSlider != null)
            timelineSlider.SetValueWithoutNotify(currentSliderValue);

        UpdateRateText();
    }

    private void TogglePause()
    {
        isPaused = !isPaused;
    }

    private void ResetTimeline()
    {
        currentSimTime = DateTime.Now;
        currentSliderValue = sliderMiddleIndex;
        if (timelineSlider != null)
            timelineSlider.SetValueWithoutNotify(currentSliderValue);

        UpdateRateText();
        UpdatePlanets(currentSimTime); // reset planet positions
    }

    private void OnSliderChanged(float value)
    {
        currentSliderValue = Mathf.RoundToInt(value);
        UpdateRateText();
    }

    private void UpdateRateText()
    {
        if (rateText == null) return;

        if (currentSliderValue == sliderMiddleIndex)
        {
            rateText.text = "REAL RATE";
        }
        else
        {
            int index = Mathf.Abs(currentSliderValue - sliderMiddleIndex) - 1;
            string label = labels[index];
            rateText.text = currentSliderValue < sliderMiddleIndex ? "-" + label + "/S" : "+" + label + "/S";
        }
    }

    private float GetCurrentSpeed()
    {
        if (currentSliderValue == sliderMiddleIndex) return 1f; // REAL RATE

        int index = Mathf.Abs(currentSliderValue - sliderMiddleIndex) - 1;
        float speed = baseRates[index];
        return currentSliderValue < sliderMiddleIndex ? -speed : speed;
    }

    // Example planet update: rotates planets around Y-axis based on orbital period
    private void UpdatePlanets(DateTime simTime)
    {
        if (planets == null || orbitalPeriods == null) return;

        for (int i = 0; i < planets.Length; i++)
        {
            if (planets[i] == null || orbitalPeriods.Length <= i) continue;

            float orbitSeconds = orbitalPeriods[i];
            float orbitProgress = (float)((simTime - DateTime.MinValue).TotalSeconds % orbitSeconds) / orbitSeconds;
            float angle = orbitProgress * 360f;

            planets[i].localRotation = Quaternion.Euler(0f, angle, 0f);
        }
    }
}