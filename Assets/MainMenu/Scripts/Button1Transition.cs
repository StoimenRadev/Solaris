using UnityEngine;
using UnityEngine.SceneManagement;

public class Button1Transition : MonoBehaviour
{
    [Header("Name of the scene to load")]
    public string nameScene;

    // This function is called when the button is clicked
    public void LoadScene()
    {
        if (!string.IsNullOrEmpty(nameScene))
        {
            SceneManager.LoadScene(nameScene);
        }
        else
        {
            Debug.LogWarning("Scene name not set!");
        }
    }
}
