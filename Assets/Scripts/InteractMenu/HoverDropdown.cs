using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverDropdown : MonoBehaviour, IPointerEnterHandler
{
    [Header("Dropdown Menu")]
    public GameObject dropdown;

    [Header("Optional Delay")]
    public float hideDelay = 0.2f; // Small delay before hiding

    private Coroutine hideCoroutine;

    // When the pointer enters the button/menu area
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Stop any pending hide
        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
            hideCoroutine = null;
        }

        // Show dropdown
        dropdown.SetActive(true);
    }

    // When the pointer exits the button/menu area
    public void OnPointerExit(PointerEventData eventData)
    {
        // Start hiding after a short delay
        hideCoroutine = StartCoroutine(HideDropdownAfterDelay());
    }

    private IEnumerator HideDropdownAfterDelay()
    {
        yield return new WaitForSeconds(hideDelay);
        dropdown.SetActive(false);
    }

    // Optional: manually hide from code
    public void Hide()
    {
        if (hideCoroutine != null) StopCoroutine(hideCoroutine);
        dropdown.SetActive(false);
    }
}
