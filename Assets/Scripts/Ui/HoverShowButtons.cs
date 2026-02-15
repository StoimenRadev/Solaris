using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class HoverShowButtons : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public RectTransform textTransform;       // The text that moves up
    public CanvasGroup[] buttons;             // The buttons that fade in/out
    public float fadeDuration = 0.5f;         // Buttons fade speed
    public float moveDuration = 0.5f;         // Text move speed
    public float moveAmount = 50f;            // How far text moves up
    public float hideDelay = 5f;              // Delay before buttons hide

    private Vector3 textOriginalPos;
    private Coroutine fadeCoroutine;
    private Coroutine hideCoroutine;
    private Coroutine moveCoroutine;

    void Start()
    {
        textOriginalPos = textTransform.localPosition;

        // Hide buttons at start
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

        // Move text up
        if (moveCoroutine != null) StopCoroutine(moveCoroutine);
        moveCoroutine = StartCoroutine(MoveText(textTransform.localPosition, textOriginalPos + Vector3.up * moveAmount));

        // Show buttons
        StartFade(1);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hideCoroutine = StartCoroutine(HideAfterDelay());
    }

    IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(hideDelay);

        // Move text back down
        if (moveCoroutine != null) StopCoroutine(moveCoroutine);
        moveCoroutine = StartCoroutine(MoveText(textTransform.localPosition, textOriginalPos));

        // Hide buttons
        StartFade(0);
    }

    void StartFade(float targetAlpha)
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeButtons(targetAlpha));
    }

    IEnumerator FadeButtons(float targetAlpha)
    {
        float time = 0;
        float[] startAlphas = new float[buttons.Length];
        for (int i = 0; i < buttons.Length; i++) startAlphas[i] = buttons[i].alpha;

        // Enable interaction immediately when fading in
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

        // Disable interaction after fade out
        if (targetAlpha == 0)
        {
            foreach (var btn in buttons)
            {
                btn.interactable = false;
                btn.blocksRaycasts = false;
            }
        }
    }

    IEnumerator MoveText(Vector3 from, Vector3 to)
    {
        float time = 0;
        while (time < moveDuration)
        {
            time += Time.deltaTime;
            textTransform.localPosition = Vector3.Lerp(from, to, time / moveDuration);
            yield return null;
        }
        textTransform.localPosition = to;
    }
}
