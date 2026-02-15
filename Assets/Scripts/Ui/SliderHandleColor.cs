using UnityEngine;
using UnityEngine.UI;

public class SliderHandleColor : MonoBehaviour
{
    public Slider slider;          // Your slider
    public Image handleImage;      // The Image component of the handle
    public Color defaultColor = Color.white; // Color when at default
    public Color changedColor = Color.green; // Color when moved
    private float defaultValue;    // Store the default slider value

    void Start()
    {
        if (slider == null) slider = GetComponent<Slider>();
        if (handleImage == null) handleImage = slider.handleRect.GetComponent<Image>();

        defaultValue = slider.value;

        slider.onValueChanged.AddListener(UpdateHandleColor);
        UpdateHandleColor(slider.value); // Set initial color
    }

    void UpdateHandleColor(float value)
    {
        // Compare with default value
        if (Mathf.Approximately(value, defaultValue))
        {
            handleImage.color = defaultColor;
        }
        else
        {
            handleImage.color = changedColor;
        }
    }
}
