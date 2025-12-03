using System.Collections;
using TMPro;
using UnityEngine;

public class TypewriterEffect : MonoBehaviour
{
    [Header("Assign your TMP Text")]
    public TMP_Text textLabel;

    [Header("Text to display")]
    [TextArea]
    public string fullText = "Earth Simulation";

    [Header("Delay before typing starts (seconds)")]
    public float startDelay = 1.5f;

    [Header("Time between letters (seconds)")]
    public float letterDelay = 0.1f;

    private void OnEnable()
    {
        StartCoroutine(TypeTextCoroutine());
    }

    private IEnumerator TypeTextCoroutine()
    {
        if (textLabel == null)
        {
            yield break;
        }

        textLabel.text = "";
        yield return new WaitForSeconds(startDelay);

        foreach (char letter in fullText)
        {
            textLabel.text += letter;
            yield return new WaitForSeconds(letterDelay);
        }
    }
}
