using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonGlow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI text;

    [Header("Colors")]
    public Color normalColor = new Color(0.7f, 0.7f, 0.7f, 1f);
    public Color hoverColor = Color.white;

    [Header("Animation")]
    public float fadeSpeed = 6f;

    bool isHovering = false;

    void Start()
    {
        if (text == null)
            text = GetComponent<TextMeshProUGUI>();

        text.color = normalColor;
    }

    void Update()
    {
        // Smooth fade
        Color targetColor = isHovering ? hoverColor : normalColor;
        text.color = Color.Lerp(text.color, targetColor, Time.deltaTime * fadeSpeed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
    }
}
