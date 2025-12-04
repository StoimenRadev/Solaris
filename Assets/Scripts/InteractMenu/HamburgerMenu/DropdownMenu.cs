using UnityEngine;
using UnityEngine.UI;

public class DropdownMenu : MonoBehaviour
{
    [System.Serializable]
    public class MenuOption
    {
        public Button optionButton;       // The clickable button
        public GameObject dropdownPanel;  // The panel to show/hide
    }

    public MenuOption[] options;

    void Start()
    {
        // Make sure all panels start hidden
        foreach (MenuOption option in options)
        {
            if (option.dropdownPanel != null)
                option.dropdownPanel.SetActive(false);

            if (option.optionButton != null)
                option.optionButton.onClick.AddListener(() => ToggleDropdown(option));
        }
    }

    void ToggleDropdown(MenuOption option)
    {
        foreach (MenuOption o in options)
        {
            if (o != option && o.dropdownPanel != null)
                o.dropdownPanel.SetActive(false); // Close others
        }

        if (option.dropdownPanel != null)
            option.dropdownPanel.SetActive(!option.dropdownPanel.activeSelf); // Toggle this
    }
}
