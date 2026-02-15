using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class HoverShowButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Settings")]
    public CanvasGroup buttonGroup;
    public float fadeDuration = 0.5f;
    public float hideDelay = 5f;

    private Coroutine fadeCoroutine;
    private Coroutine hideCoroutine;

    void Start()
    {
        // Initialize button as hidden and non-interactable
        buttonGroup.alpha = 0f;
        buttonGroup.interactable = false;
        buttonGroup.blocksRaycasts = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Stop hiding coroutine if hovering again
        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
            hideCoroutine = null;
        }

        // Fade in
        StartFade(1f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Start hide delay when pointer exits
        hideCoroutine = StartCoroutine(HideAfterDelay());
    }

    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(hideDelay);
        StartFade(0f);
    }

    private void StartFade(float targetAlpha)
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(Fade(targetAlpha));
    }

    private IEnumerator Fade(float targetAlpha)
    {
        float startAlpha = buttonGroup.alpha;
        float time = 0f;

        // Enable interaction immediately when fading in
        if (targetAlpha == 1f)
        {
            buttonGroup.interactable = true;
            buttonGroup.blocksRaycasts = true;
        }

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            buttonGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration);
            yield return null;
        }

        buttonGroup.alpha = targetAlpha;

        // Disable interaction after fade out
        if (targetAlpha == 0f)
        {
            buttonGroup.interactable = false;
            buttonGroup.blocksRaycasts = false;
        }
    }
}
