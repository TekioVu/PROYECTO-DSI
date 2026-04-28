using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuController : MonoBehaviour
{
    VisualElement initMenu;
    VisualElement mainMenu;
    VisualElement orisMenu;
    VisualElement weaponMenu;

    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        initMenu = root.Q<VisualElement>("InitMenu");
        mainMenu = root.Q<VisualElement>("MainMenu");
        orisMenu = root.Q<VisualElement>("OrisMenu");
        weaponMenu = root.Q<VisualElement>("Weapon");

        var exitButton = root.Q<VisualElement>("Exit");
        var orisMenuButton = mainMenu.Q<VisualElement>("orisMenu");
        var weaponMenuButton = mainMenu.Q<VisualElement>("weaponSelection");

        weaponMenuButton.RegisterCallback<ClickEvent>(evt =>
        {
            mainMenu.style.display = DisplayStyle.None;
            weaponMenu.style.display = DisplayStyle.Flex;
        });

        exitButton.RegisterCallback<ClickEvent>(evt =>
        {
            mainMenu.style.display = DisplayStyle.None;
            initMenu.style.display = DisplayStyle.Flex;
        });

        orisMenuButton.RegisterCallback<ClickEvent>(evt =>
        {
            mainMenu.style.display = DisplayStyle.None;
            orisMenu.style.display = DisplayStyle.Flex;
        });
    }
}