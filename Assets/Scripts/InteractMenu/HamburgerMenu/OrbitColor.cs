using UnityEngine;
using UnityEngine.UI;

public class OrbitPathColorPicker : MonoBehaviour
{
    [Header("RGB Inputs")]
    public InputField redInput;
    public InputField greenInput;
    public InputField blueInput;

    [Header("Color Preview Images")]
    public Image redImage;
    public Image greenImage;
    public Image blueImage;

    [Header("Buttons")]
    public Button applyButton;
    public Button defaultButton;

    [Header("Transparency")]
    public Toggle transparentToggle;

    [Header("Orbit Path")]
    private LineRenderer orbitPath; // Currently selected planet's orbit

    [Header("Default Color")]
    public Color defaultColor = Color.white;

    void Start()
    {
        // Assign button callbacks
        applyButton.onClick.AddListener(ApplyColor);
        defaultButton.onClick.AddListener(ResetToDefault);
        transparentToggle.onValueChanged.AddListener(delegate { UpdateTransparency(); });

        // Update preview images in case user types numbers manually
        redInput.onValueChanged.AddListener(delegate { UpdatePreviewColors(); });
        greenInput.onValueChanged.AddListener(delegate { UpdatePreviewColors(); });
        blueInput.onValueChanged.AddListener(delegate { UpdatePreviewColors(); });
    }

    /// <summary>
    /// Selects which planet's orbit path will be modified
    /// </summary>
    public void SetPlanetOrbit(LineRenderer path)
    {
        orbitPath = path;

        if (orbitPath != null)
        {
            Color c = orbitPath.startColor;

            redInput.text = Mathf.RoundToInt(c.r * 255).ToString();
            greenInput.text = Mathf.RoundToInt(c.g * 255).ToString();
            blueInput.text = Mathf.RoundToInt(c.b * 255).ToString();
            transparentToggle.isOn = c.a < 1f;

            UpdatePreviewColors();
        }
    }

    public void ApplyColor()
    {
        if (orbitPath == null) return;

        byte r = ParseInput(redInput.text);
        byte g = ParseInput(greenInput.text);
        byte b = ParseInput(blueInput.text);

        float alpha = transparentToggle.isOn ? 0f : 1f;

        Color newColor = new Color(r / 255f, g / 255f, b / 255f, alpha);
        orbitPath.startColor = newColor;
        orbitPath.endColor = newColor;
    }

    public void ResetToDefault()
    {
        if (orbitPath == null) return;

        orbitPath.startColor = defaultColor;
        orbitPath.endColor = defaultColor;

        redInput.text = Mathf.RoundToInt(defaultColor.r * 255).ToString();
        greenInput.text = Mathf.RoundToInt(defaultColor.g * 255).ToString();
        blueInput.text = Mathf.RoundToInt(defaultColor.b * 255).ToString();
        transparentToggle.isOn = defaultColor.a < 1f;

        UpdatePreviewColors();
    }

    public void UpdateTransparency()
    {
        if (orbitPath == null) return;

        Color c = orbitPath.startColor;
        c.a = transparentToggle.isOn ? 0f : 1f;
        orbitPath.startColor = c;
        orbitPath.endColor = c;
    }

    private void UpdatePreviewColors()
    {
        redImage.color = new Color(ParseInput(redInput.text) / 255f, 0, 0);
        greenImage.color = new Color(0, ParseInput(greenInput.text) / 255f, 0);
        blueImage.color = new Color(0, 0, ParseInput(blueInput.text) / 255f);
    }

    private byte ParseInput(string s)
    {
        if (byte.TryParse(s, out byte value))
            return (byte)Mathf.Clamp(value, 0, 255);
        return 0;
    }
}
