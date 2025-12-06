using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(InputField))]
public class InputFieldSetup : MonoBehaviour
{
    public int characterLimit = 3;

    void Awake()
    {
        InputField input = GetComponent<InputField>();
        if (input != null)
        {
            // Set to integer only
            input.contentType = InputField.ContentType.IntegerNumber;

            // Limit characters
            input.characterLimit = characterLimit;
        }
    }
}
