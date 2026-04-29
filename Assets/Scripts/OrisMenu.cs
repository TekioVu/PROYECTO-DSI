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
        VisualElement m1 = orisMenu.Q<VisualElement>("mission1");
        VisualElement m2 = orisMenu.Q<VisualElement>("mission2");
        VisualElement m3 = orisMenu.Q<VisualElement>("mission3");
        VisualElement m4 = orisMenu.Q<VisualElement>("mission4");
        VisualElement m5 = orisMenu.Q<VisualElement>("mission5");
        VisualElement m6 = orisMenu.Q<VisualElement>("mission6");

        SetupMissionHover(m1, "A spreading corruption must be purified before it consumes the remaining life of the forest.");
        SetupMissionHover(m2, "Ancient mechanisms lie frozen and silent, waiting to be brought back to life.");
        SetupMissionHover(m3, "New paths will open once the skies move again.");
        SetupMissionHover(m4, "A fading source of light must be revived to protect everything that depends on it.");
        SetupMissionHover(m5, "Survival depends on swift movement through collapsing and dangerous terrain.");
        SetupMissionHover(m6, "A lost companion must be located somewhere in an unfamiliar land.");

        MapResize();
        MapItemsConfig();
    }

    void SetupMissionHover(VisualElement mission, string description)
    {
        mission.RegisterCallback<MouseEnterEvent>(_ => misionDescription.text = description);
        mission.RegisterCallback<MouseLeaveEvent>(_ => misionDescription.text = " ");
    }

    void MapItemsConfig()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        VisualElement orisMenu = root.Q<VisualElement>("OrisMenu");

        Label displayItems = orisMenu.Q<Label>("displayMapItems");
        VisualElement mapItems = orisMenu.Q<VisualElement>("mapItems");
        VisualElement mapLegend = orisMenu.Q<VisualElement>("mapLegend");

        mapItems.style.display = DisplayStyle.None;
        mapLegend.style.display = DisplayStyle.None;

        displayItems.text = "Show items";
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

        void SetBackgroundRecursive(VisualElement parent, Texture2D texture)
        {
            Debug.Log("HOVER");

            foreach (var child in parent.Children())
            {
                if (texture == null)
                    child.style.backgroundImage = StyleKeyword.None;
                else
                    child.style.backgroundImage = new StyleBackground(texture);
            }
        }
        Label lifeCellsText = orisMenu.Q<Label>("lifeCellsLegendText");
        VisualElement lifeCellsImages = orisMenu.Q<VisualElement>("lifeCells");

        lifeCellsText.RegisterCallback<MouseEnterEvent>(evt =>
        {
            SetBackgroundRecursive(lifeCellsImages, circleTexture);
        });

        lifeCellsText.RegisterCallback<MouseLeaveEvent>(evt =>
        {
            SetBackgroundRecursive(lifeCellsImages, null);
        });

        Label energyCellsText = orisMenu.Q<Label>("energyCellsLegendText");
        VisualElement energyCellsImages = orisMenu.Q<VisualElement>("energyCells");

        energyCellsText.RegisterCallback<MouseEnterEvent>(evt =>
        {
            SetBackgroundRecursive(energyCellsImages, circleTexture);
        });

        energyCellsText.RegisterCallback<MouseLeaveEvent>(evt =>
        {
            SetBackgroundRecursive(energyCellsImages, null);
        });

        Label abilityText = orisMenu.Q<Label>("abilityPointsLegendText");
        VisualElement abilityPointsImages = orisMenu.Q<VisualElement>("abilityPoints");

        abilityText.RegisterCallback<MouseEnterEvent>(evt =>
        {
            SetBackgroundRecursive(abilityPointsImages, circleTexture);
        });

        abilityText.RegisterCallback<MouseLeaveEvent>(evt =>
        {
            SetBackgroundRecursive(abilityPointsImages, null);
        });

        Label oriText = orisMenu.Q<Label>("oriLegendText");
        VisualElement oriImages = orisMenu.Q<VisualElement>("ori");

        oriText.RegisterCallback<MouseEnterEvent>(evt =>
        {
            SetBackgroundRecursive(oriImages, circleTexture);
        });

        oriText.RegisterCallback<MouseLeaveEvent>(evt =>
        {
            SetBackgroundRecursive(oriImages, null);
        });
    }

    void MapResize()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        VisualElement container = root.Q<VisualElement>("map");
        VisualElement mapImage = root.Q<VisualElement>("mapContent");

        float minZoom = 1f;
        float maxZoom = 5f;

        float zoom = 2f;
        Vector2 offset = Vector2.zero;

        // Guardar estado inicial
        float initialZoom = zoom;
        Vector2 initialOffset = offset;

        ApplyTransform();

        mapImage.AddManipulator(new PanManipulator(() =>
        {
            return new PanData
            {
                offset = offset,
                onMove = delta =>
                {
                    offset += delta;
                    ClampOffset();
                    ApplyTransform();
                }
            };
        }));

        mapImage.AddManipulator(new ZoomManipulator(() =>
        {
            return new ZoomData
            {
                zoom = zoom,
                onZoom = (delta, mousePos) =>
                {
                    float zoomChange = -delta.y * 0.01f;
                    zoom = Mathf.Clamp(zoom + zoomChange, minZoom, maxZoom);

                    ClampOffset();
                    ApplyTransform();
                }
            };
        }));

        void ClampOffset()
        {
            float containerWidth = container.resolvedStyle.width;
            float containerHeight = container.resolvedStyle.height;

            float imageWidth = mapImage.resolvedStyle.width * zoom;
            float imageHeight = mapImage.resolvedStyle.height * zoom;

            float maxX = Mathf.Max(0, (imageWidth - containerWidth) / 2f);
            float maxY = Mathf.Max(0, (imageHeight - containerHeight) / 2f);

            offset.x = Mathf.Clamp(offset.x, -maxX, maxX);
            offset.y = Mathf.Clamp(offset.y, -maxY, maxY);
        }

        void ApplyTransform()
        {
            mapImage.transform.scale = new Vector3(zoom, zoom, 1);
            mapImage.transform.position = offset;
        }

        container.RegisterCallback<AttachToPanelEvent>(_ =>
        {
            zoom = initialZoom;
            offset = initialOffset;
            ApplyTransform();
        });
    }
}

public struct PanData
{
    public Vector2 offset;
    public System.Action<Vector2> onMove;
}
public class PanManipulator : PointerManipulator
{
 

    private bool active;
    private Vector2 lastPos;

    private System.Func<PanData> getData;

    public PanManipulator(System.Func<PanData> data)
    {
        getData = data;
        activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse });
    }

    protected override void RegisterCallbacksOnTarget()
    {
        target.RegisterCallback<PointerDownEvent>(OnDown);
        target.RegisterCallback<PointerMoveEvent>(OnMove);
        target.RegisterCallback<PointerUpEvent>(OnUp);
    }

    protected override void UnregisterCallbacksFromTarget()
    {
        target.UnregisterCallback<PointerDownEvent>(OnDown);
        target.UnregisterCallback<PointerMoveEvent>(OnMove);
        target.UnregisterCallback<PointerUpEvent>(OnUp);
    }

    void OnDown(PointerDownEvent evt)
    {
        if (!CanStartManipulation(evt)) return;
        active = true;
        lastPos = evt.position;
        target.CapturePointer(evt.pointerId);
    }

    void OnMove(PointerMoveEvent evt)
    {
        if (!active || !target.HasPointerCapture(evt.pointerId)) return;

        var delta = (Vector2)evt.position - lastPos;
        lastPos = evt.position;

        var data = getData();
        data.onMove(delta);
    }

    void OnUp(PointerUpEvent evt)
    {
        if (!active || !target.HasPointerCapture(evt.pointerId)) return;

        active = false;
        target.ReleasePointer(evt.pointerId);
    }
}

public struct ZoomData
{
    public float zoom;
    public System.Action<Vector2, Vector2> onZoom;
}

public class ZoomManipulator : PointerManipulator
{
    

    private System.Func<ZoomData> getData;

    public ZoomManipulator(System.Func<ZoomData> data)
    {
        getData = data;
    }

    protected override void RegisterCallbacksOnTarget()
    {
        target.RegisterCallback<WheelEvent>(OnWheel);
    }

    protected override void UnregisterCallbacksFromTarget()
    {
        target.UnregisterCallback<WheelEvent>(OnWheel);
    }

    void OnWheel(WheelEvent evt)
    {
        var data = getData();
        data.onZoom(evt.delta, evt.mousePosition);
        evt.StopPropagation();
    }
}