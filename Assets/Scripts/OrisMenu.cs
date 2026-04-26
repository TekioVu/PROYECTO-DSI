using UnityEngine;
using UnityEngine.UIElements;

public class OrisMenu : MonoBehaviour
{
    [SerializeField] private Texture2D circleTexture;

    Label misionDescription;

    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        var mainMenu = root.Q<VisualElement>("MainMenu");
        var orisMenu = root.Q<VisualElement>("OrisMenu");

        VisualElement exitOrisMenu = orisMenu.Q<VisualElement>("exit");
        VisualElement map = orisMenu.Q<VisualElement>("Map");
        VisualElement inventory = orisMenu.Q<VisualElement>("Inventory");
        VisualElement spiritShards = orisMenu.Q<VisualElement>("SpiritShards");

        VisualElement leftArrow = orisMenu.Q<VisualElement>("leftArrow");
        VisualElement rightArrow = orisMenu.Q<VisualElement>("rightArrow");

        VisualElement currentMenu = map;

        map.style.display = DisplayStyle.Flex;
        inventory.style.display = DisplayStyle.None;
        spiritShards.style.display = DisplayStyle.None;
        leftArrow.style.display = DisplayStyle.None;

        rightArrow.RegisterCallback<ClickEvent>(evt =>
        {
            if (currentMenu == map)
            {
                currentMenu = inventory;
                map.style.display = DisplayStyle.None;
                inventory.style.display = DisplayStyle.Flex;
                spiritShards.style.display = DisplayStyle.None;
                leftArrow.style.display = DisplayStyle.Flex;
            }
            else if (currentMenu == inventory)
            {
                currentMenu = spiritShards;
                map.style.display = DisplayStyle.None;
                inventory.style.display = DisplayStyle.None;
                spiritShards.style.display = DisplayStyle.Flex;
                rightArrow.style.display = DisplayStyle.None;
            }
        });

        leftArrow.RegisterCallback<ClickEvent>(evt =>
        {
            if (currentMenu == spiritShards)
            {
                currentMenu = inventory;
                map.style.display = DisplayStyle.None;
                inventory.style.display = DisplayStyle.Flex;
                spiritShards.style.display = DisplayStyle.None;
                rightArrow.style.display = DisplayStyle.Flex;
            }
            else if (currentMenu == inventory)
            {
                currentMenu = map;
                map.style.display = DisplayStyle.Flex;
                inventory.style.display = DisplayStyle.None;
                spiritShards.style.display = DisplayStyle.None;
                leftArrow.style.display = DisplayStyle.None;
            }
        });

        exitOrisMenu.RegisterCallback<ClickEvent>(evt =>
        {
            mainMenu.style.display = DisplayStyle.Flex;
            orisMenu.style.display = DisplayStyle.None;
        });

        misionDescription = orisMenu.Q<Label>("missionDescription");

        SetupMissionHover(orisMenu.Q("mission1"), "A spreading corruption...");
        SetupMissionHover(orisMenu.Q("mission2"), "Ancient mechanisms...");
        SetupMissionHover(orisMenu.Q("mission3"), "New paths...");
        SetupMissionHover(orisMenu.Q("mission4"), "A fading source...");
        SetupMissionHover(orisMenu.Q("mission5"), "Survival depends...");
        SetupMissionHover(orisMenu.Q("mission6"), "A lost companion...");

        Label displayItems = orisMenu.Q<Label>("displayMapItems");
        VisualElement mapItems = orisMenu.Q<VisualElement>("mapItems");
        VisualElement mapLegend = orisMenu.Q<VisualElement>("mapLegend");

        mapItems.style.display = DisplayStyle.None;
        mapLegend.style.display = DisplayStyle.None;

        displayItems.RegisterCallback<ClickEvent>(evt =>
        {
            if (mapItems.style.display == DisplayStyle.None)
            {
                mapItems.style.display = DisplayStyle.Flex;
                mapLegend.style.display = DisplayStyle.Flex;
                displayItems.text = "Hide items";
            }
            else
            {
                mapItems.style.display = DisplayStyle.None;
                mapLegend.style.display = DisplayStyle.None;
                displayItems.text = "Show items";
            }
        });
    }

    void SetupMissionHover(VisualElement mission, string description)
    {
        mission.RegisterCallback<MouseEnterEvent>(_ => misionDescription.text = description);
        mission.RegisterCallback<MouseLeaveEvent>(_ => misionDescription.text = " ");
    }
}