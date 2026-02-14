using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class HoverShowButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public CanvasGroup buttonGroup;
    public float fadeDuration = 0.5f;
    public float hideDelay = 5f;

    private Coroutine fadeCoroutine;
    private Coroutine hideCoroutine;

    void Start()
    {
        buttonGroup.alpha = 0;
        buttonGroup.interactable = false;
        buttonGroup.blocksRaycasts = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
            hideCoroutine = null;
        }

        StartFade(1);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hideCoroutine = StartCoroutine(HideAfterDelay());
    }

    IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(hideDelay);
        StartFade(0);
    }

    void StartFade(float targetAlpha)
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(Fade(targetAlpha));
    }

    IEnumerator Fade(float targetAlpha)
    {
        float startAlpha = buttonGroup.alpha;
        float time = 0;

        // Enable interaction immediately when fading in
        if (targetAlpha == 1)
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
        if (targetAlpha == 0)
        {
            buttonGroup.interactable = false;
            buttonGroup.blocksRaycasts = false;
        }
    }
}
