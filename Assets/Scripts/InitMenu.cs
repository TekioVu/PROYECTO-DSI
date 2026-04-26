using UnityEngine;
using UnityEngine.UIElements;

public class InitMenu : MonoBehaviour
{
    VisualElement initMenu;
    VisualElement mainMenu;
    VisualElement optionsMenu;

    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        initMenu = root.Q<VisualElement>("InitMenu");
        mainMenu = root.Q<VisualElement>("MainMenu");
        optionsMenu = root.Q<VisualElement>("OptionsMenu");

        var startButton = root.Q<VisualElement>("Init");
        var optionsButton = root.Q<VisualElement>("Options");

        startButton.RegisterCallback<ClickEvent>(evt =>
        {
            initMenu.style.display = DisplayStyle.None;
            mainMenu.style.display = DisplayStyle.Flex;
        });

        optionsButton.RegisterCallback<ClickEvent>(evt =>
        {
            initMenu.style.display = DisplayStyle.None;
            optionsMenu.style.display = DisplayStyle.Flex;
        });
    }
}