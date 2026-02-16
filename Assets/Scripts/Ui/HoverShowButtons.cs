using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverShowButtons : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public RectTransform textTransform;
    public TextMeshProUGUI textComponent;

    public CanvasGroup[] buttons;

    public float fadeDuration = 0.5f;
    public float moveDuration = 0.5f;
    public float hideDelay = 5f;

    public float moveAmount = 50f;

    public float smallFontSize = 28f;
    public float bigFontSize = 40f;

    private Vector3 textOriginalPos;
    private Coroutine fadeCoroutine;
    private Coroutine hideCoroutine;
    private Coroutine moveCoroutine;

    void Start()
    {
        textOriginalPos = textTransform.localPosition;
        textComponent.fontSize = smallFontSize;

        foreach (var btn in buttons)
        {
            btn.alpha = 0;
            btn.interactable = false;
            btn.blocksRaycasts = false;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
            hideCoroutine = null;
        }

        AnimateText(textOriginalPos + Vector3.up * moveAmount, bigFontSize);
        StartFade(1);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hideCoroutine = StartCoroutine(HideAfterDelay());
    }

    IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(hideDelay);

        AnimateText(textOriginalPos, smallFontSize);
        StartFade(0);
    }

    void AnimateText(Vector3 targetPos, float targetFontSize)
    {
        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);

        moveCoroutine = StartCoroutine(MoveAndResize(targetPos, targetFontSize));
    }

    IEnumerator MoveAndResize(Vector3 targetPos, float targetFontSize)
    {
        float time = 0;

        Vector3 startPos = textTransform.localPosition;
        float startFont = textComponent.fontSize;

        while (time < moveDuration)
        {
            time += Time.deltaTime;
            float t = time / moveDuration;

            textTransform.localPosition = Vector3.Lerp(startPos, targetPos, t);
            textComponent.fontSize = Mathf.Lerp(startFont, targetFontSize, t);

            yield return null;
        }

        textTransform.localPosition = targetPos;
        textComponent.fontSize = targetFontSize;
    }

    void StartFade(float targetAlpha)
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeButtons(targetAlpha));
    }

    IEnumerator FadeButtons(float targetAlpha)
    {
        float time = 0;
        float[] startAlphas = new float[buttons.Length];

        for (int i = 0; i < buttons.Length; i++)
            startAlphas[i] = buttons[i].alpha;

        if (targetAlpha == 1)
        {
            foreach (var btn in buttons)
            {
                btn.interactable = true;
                btn.blocksRaycasts = true;
            }
        }

        while (time < fadeDuration)
        {
            time += Time.deltaTime;

            for (int i = 0; i < buttons.Length; i++)
                buttons[i].alpha = Mathf.Lerp(startAlphas[i], targetAlpha, time / fadeDuration);

            yield return null;
        }

        for (int i = 0; i < buttons.Length; i++)
            buttons[i].alpha = targetAlpha;

        if (targetAlpha == 0)
        {
            foreach (var btn in buttons)
            {
                btn.interactable = false;
                btn.blocksRaycasts = false;
            }
        }
    }
}
