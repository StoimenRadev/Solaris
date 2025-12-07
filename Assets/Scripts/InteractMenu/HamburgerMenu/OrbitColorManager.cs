using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OrbitColorManager : MonoBehaviour, IDropHandler
{
    [Header("UI References")]
    public GameObject colorPickerObject;     // Whole color picker panel
    public FlexibleColorPicker flexibleColorPicker; // Optional color picker
    public Image colorPreviewImage;          // Current color preview
    public Toggle transparentToggle;         // Transparent checkbox
    public Button applyButton;               // Apply button
    public Button defaultButton;             // Default button
    public InputField hexInput;              // Hex input field for drag

    [Header("Orbit Paths")]
    public OrbitPathColor[] orbitPaths;

    private void Awake()
    {
        if (applyButton != null) applyButton.onClick.AddListener(ApplyColor);
        if (defaultButton != null) defaultButton.onClick.AddListener(RestoreDefaults);
    }

    // ---------------- Apply / Default ----------------
    public void ApplyColor()
    {
        Color colorToApply;

        if (transparentToggle != null && transparentToggle.isOn)
        {
            colorToApply = Color.clear;
        }
        else if (flexibleColorPicker != null)
        {
            colorToApply = flexibleColorPicker.GetColorFullAlpha(); // current color from picker
        }
        else if (hexInput != null && ColorUtility.TryParseHtmlString(hexInput.text, out colorToApply))
        {
            // fallback to hex input if color picker is missing
        }
        else
        {
            Debug.LogWarning("No color source assigned!");
            return;
        }

        foreach (var path in orbitPaths)
            if (path != null)
                path.SetColor(colorToApply);

        if (colorPreviewImage != null)
            colorPreviewImage.color = colorToApply;
    }

    public void RestoreDefaults()
    {
        foreach (var path in orbitPaths)
            if (path != null)
                path.RestoreDefault();

        if (transparentToggle != null)
            transparentToggle.isOn = false;

        // Reset preview
        if (colorPreviewImage != null)
            colorPreviewImage.color = Color.white;

        // Reset color picker
        if (flexibleColorPicker != null)
            flexibleColorPicker.SetColor(Color.white);

        // Reset hex input
        if (hexInput != null)
            hexInput.text = "#FFFFFF";
    }

    // ---------------- Drag-and-Drop ----------------
    public void OnDrop(PointerEventData eventData)
    {
        // Drag from hex input
        if (eventData.pointerDrag == hexInput?.gameObject)
        {
            string hex = hexInput.text;
            Color color;
            if (ColorUtility.TryParseHtmlString(hex, out color))
            {
                if (transparentToggle != null && transparentToggle.isOn)
                    color = Color.clear;

                foreach (var path in orbitPaths)
                    if (path != null)
                        path.SetColor(color);

                if (colorPreviewImage != null)
                    colorPreviewImage.color = color;

                // Also update color picker if assigned
                if (flexibleColorPicker != null)
                    flexibleColorPicker.SetColor(color);
            }
        }
    }
}
