using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimelineUIController : MonoBehaviour
{
    public GameObject openButton;   // Your Open Button
    public GameObject closeButton;  // Your Close Button
    public Animator panelAnimator;  // Animator of the panel
    public float fadeDuration = 0.3f; 

    private CanvasGroup openCanvasGroup;
    private Button openButtonComp;

    private void Awake()
    {
        // Get CanvasGroup
        openCanvasGroup = openButton.GetComponent<CanvasGroup>();
        if (openCanvasGroup == null)
        {
            openCanvasGroup = openButton.AddComponent<CanvasGroup>();
        }

        // Get Button component
        openButtonComp = openButton.GetComponent<Button>();
        if (openButtonComp == null)
        {
            Debug.LogWarning("Open Button does not have a Button component!");
        }
    }

    // Open panel
    public void OpenPanel()
    {
        panelAnimator.SetTrigger("Open");      // Play panel opening animation
        StartCoroutine(FadeOutOpenButton());   // Fade out Open Button
    }

    // Close panel
    public void ClosePanel()
    {
        panelAnimator.SetTrigger("Close");     // Play panel closing animation
        StartCoroutine(FadeInOpenButton());    // Fade in Open Button
    }

    private IEnumerator FadeOutOpenButton()
    {
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            openCanvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            yield return null;
        }
        openCanvasGroup.alpha = 0f;

        // Disable clicks while hidden
        if (openButtonComp != null) openButtonComp.interactable = false;
    }

    private IEnumerator FadeInOpenButton()
    {
        // Enable clicks immediately
        if (openButtonComp != null) openButtonComp.interactable = true;

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            openCanvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / fadeDuration);
            yield return null;
        }
        openCanvasGroup.alpha = 1f;
    }
}
