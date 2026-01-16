using UnityEngine;
using UnityEngine.UI;

public class CurvedSlider : MonoBehaviour
{
    public Slider slider;              // the hidden real slider
    public RectTransform handle;       // the visible handle

    public float width = 400f;          // horizontal size of curve
    public float height = 120f;         // how tall the curve is

    void Update()
    {
        float t = slider.value; // 0 → 1

        // Move left to right
        float x = Mathf.Lerp(-width / 2f, width / 2f, t);

        // Create ⌒ curve using sine
        float y = Mathf.Sin(t * Mathf.PI) * height;

        handle.anchoredPosition = new Vector2(x, y);
    }
}
