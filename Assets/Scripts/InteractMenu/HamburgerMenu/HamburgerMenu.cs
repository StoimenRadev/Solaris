using System.Collections;
using UnityEngine;

public class HamburgerButton : MonoBehaviour
{
    [Header("UI Elements")]
    public RectTransform topMenu;           // The menu panel
    public RectTransform hamburgerButton;   // The button
    public float slideDistance = 300f;      // How far the button moves
    public float animationTime = 0.5f;      // Duration of slide
    public float rotationTime = 0.25f;      // Duration of rotation

    private bool isOpen = false;
    private bool isAnimating = false;

    private Vector2 buttonStartPos;
    private Vector2 buttonEndPos;

    private Vector2 menuStartSize;
    private Vector2 menuEndSize;

    private Quaternion buttonStartRotation;
    private Quaternion buttonDownRotation;

    void Start()
    {
        // Save button start position
        buttonStartPos = hamburgerButton.anchoredPosition;
        buttonEndPos = buttonStartPos + new Vector2(slideDistance, 0);

        // Menu size
        menuEndSize = topMenu.sizeDelta;               // Full size
        menuStartSize = new Vector2(0, menuEndSize.y); // Hidden behind button
        topMenu.sizeDelta = menuStartSize;            // Start hidden

        // Save rotation
        buttonStartRotation = hamburgerButton.rotation;
        // Rotate 90 degrees on X axis
        buttonDownRotation = buttonStartRotation * Quaternion.Euler(90f, 0f, 0f);
    }

    public void ToggleMenu()
    {
        if (isAnimating) return;
        StartCoroutine(RotateAndAnimate(!isOpen));
        isOpen = !isOpen;
    }

    private IEnumerator RotateAndAnimate(bool opening)
    {
        isAnimating = true;

        // Step 1: Rotate button 90 degrees down (X axis)
        yield return StartCoroutine(RotateButton(opening ? buttonDownRotation : buttonDownRotation));

        // Step 2: Animate slide and reveal
        float elapsed = 0f;

        Vector2 buttonFrom = opening ? buttonStartPos : buttonEndPos;
        Vector2 buttonTo = opening ? buttonEndPos : buttonStartPos;

        Vector2 menuFrom = opening ? menuStartSize : menuEndSize;
        Vector2 menuTo = opening ? menuEndSize : menuStartSize;

        while (elapsed < animationTime)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / animationTime);
            t = t * t * (3f - 2f * t); // Smooth step

            // Move button
            hamburgerButton.anchoredPosition = Vector2.Lerp(buttonFrom, buttonTo, t);

            // Reveal menu gradually
            topMenu.sizeDelta = Vector2.Lerp(menuFrom, menuTo, t);

            yield return null;
        }

        // Snap final values
        hamburgerButton.anchoredPosition = buttonTo;
        topMenu.sizeDelta = menuTo;

        // Step 3: Rotate back to original
        yield return StartCoroutine(RotateButton(opening ? buttonStartRotation : buttonStartRotation));

        isAnimating = false;
    }

    private IEnumerator RotateButton(Quaternion targetRotation)
    {
        Quaternion startRotation = hamburgerButton.rotation;
        float elapsed = 0f;

        while (elapsed < rotationTime)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / rotationTime);
            t = t * t * (3f - 2f * t); // Smooth step
            hamburgerButton.rotation = Quaternion.Lerp(startRotation, targetRotation, t);
            yield return null;
        }

        hamburgerButton.rotation = targetRotation; // Snap final rotation
    }
}
