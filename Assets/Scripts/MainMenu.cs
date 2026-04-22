using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI.MessageBox;

public class PlayerEntry
{
    public string nombre;
    public float tiempo;     
    public float progreso;   
    public int muertes;

    public float Score()
    {
        return progreso * 2 - tiempo * 0.5f - muertes * 1.5f;
    }
}


public class MainMenu : MonoBehaviour
{
    [SerializeField] private Texture2D controllerTexture;
    [SerializeField] private Texture2D keyboardTexture;
    void RefreshLeaderboard()
    {
        leaderboardContainer.Clear();

        for (int i = 0; i < leaderboard.Count; i++)
        {
            int index = i; 
            var entry = leaderboard[i];

            VisualElement row = new VisualElement();
            row.style.flexDirection = FlexDirection.Row;
            row.AddToClassList("row");

            Label rank = new Label((i + 1).ToString());
            Label name = new Label(entry.nombre);
            Label time = new Label(entry.tiempo.ToString("F2"));
            Label progress = new Label(entry.progreso + "%");
            Label deaths = new Label(entry.muertes.ToString());

            Button deleteBtn = new Button(() =>
            {
                leaderboard.RemoveAt(index);
                RefreshLeaderboard();
            });
            deleteBtn.text = "X";
            deleteBtn.AddToClassList("normalText");
            deleteBtn.style.backgroundColor = new Color(0, 0, 0, 0.5f);
            deleteBtn.style.width = 40;
            deleteBtn.style.unityTextAlign = TextAnchor.MiddleCenter;
            List<Label> labels = new List<Label> { rank, name, time, progress, deaths };

            foreach (var l in labels)
            {
                l.AddToClassList("normalText");
                l.style.width = 100;
                l.style.unityTextAlign = TextAnchor.MiddleCenter;
                row.Add(l);
            }

            deleteBtn.style.width = 40;

            row.Add(deleteBtn);

            leaderboardContainer.Add(row);
        }
    }

    void ShowError(string message)
    {
        errorLabel.text = message;
        errorLabel.style.display = DisplayStyle.Flex;
    }

    void SetupMissionHover(VisualElement mission, string description)
    {
        mission.RegisterCallback<MouseEnterEvent>(evt =>
        {
            misionDescription.text = description;
        });

        mission.RegisterCallback<MouseLeaveEvent>(evt =>
        {
            misionDescription.text = " ";
        });
    }



    VisualElement startButton;
    VisualElement optionsButton;
    VisualElement exitButton;
    VisualElement initMenu;
    VisualElement mainMenu;
    VisualElement optionsMenu;
    VisualElement leaderboardContainer;
    List<PlayerEntry> leaderboard = new List<PlayerEntry>();

    Label errorLabel;
    Label misionDescription;
    void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        initMenu = root.Q<VisualElement>("InitMenu");
        mainMenu = root.Q<VisualElement>("MainMenu");
        optionsMenu = root.Q<VisualElement>("OptionsMenu");
        startButton = root.Q<VisualElement>("Init");
        optionsButton = root.Q<VisualElement>("Options");
        exitButton = root.Q<VisualElement>("Exit");

        startButton.RegisterCallback<ClickEvent>(evt =>
            {
                initMenu.style.display = DisplayStyle.None;
                mainMenu.style.display = DisplayStyle.Flex;
            });

        exitButton.RegisterCallback<ClickEvent>(evt =>
        {
            mainMenu.style.display = DisplayStyle.None;
            initMenu.style.display = DisplayStyle.Flex;
        });

        optionsButton.RegisterCallback<ClickEvent>(evt =>
        {
            initMenu.style.display = DisplayStyle.None;
            optionsMenu.style.display = DisplayStyle.Flex;
        });

        List<VisualElement> options = new List<VisualElement>();
        List<VisualElement> optionsContent = new List<VisualElement>();

        VisualElement settings = optionsMenu.Q<VisualElement>("settings");
        VisualElement settingsContent = optionsMenu.Q<VisualElement>("settingsContent");
        options.Add(settings);
        optionsContent.Add(settingsContent);
        settings.AddToClassList("activeText");

        VisualElement controls = optionsMenu.Q<VisualElement>("controls");
        VisualElement controlsContent = optionsMenu.Q<VisualElement>("controlsContent");
        options.Add(controls);
        optionsContent.Add(controlsContent);

        VisualElement leaderboards = optionsMenu.Q<VisualElement>("leaderboards");
        VisualElement leaderboardsContent = optionsMenu.Q<VisualElement>("leaderboardsContent");
        options.Add(leaderboards);
        optionsContent.Add(leaderboardsContent);

        VisualElement achievments = optionsMenu.Q<VisualElement>("achievments");
        VisualElement achievmentsContent = optionsMenu.Q<VisualElement>("achievmentsContent");
        options.Add(achievments);
        optionsContent.Add(achievmentsContent);

        VisualElement exit = optionsMenu.Q<VisualElement>("exitOptions");
        exit.RegisterCallback<ClickEvent>(evt =>
        {
            for (int i = 0; i < options.Count; i++)
            {
                foreach (var opt in options)
                    opt.RemoveFromClassList("activeText");

                foreach (var content in optionsContent)
                    content.style.display = DisplayStyle.None;

                settings.AddToClassList("activeText");
                settingsContent.style.display = DisplayStyle.Flex;
            }
            optionsMenu.style.display = DisplayStyle.None;
            initMenu.style.display = DisplayStyle.Flex;
        });

        for (int i = 0; i < options.Count; i++)
        {
            int index = i;

            options[i].RegisterCallback<ClickEvent>(evt =>
            {
                foreach (var opt in options)
                    opt.RemoveFromClassList("activeText");

                foreach (var content in optionsContent)
                    content.style.display = DisplayStyle.None;

                options[index].AddToClassList("activeText");
                optionsContent[index].style.display = DisplayStyle.Flex;
            });
        }

        void StyleSlider(Slider slider)
        {
            slider.AddToClassList("slider-custom");
        }

        StyleSlider(root.Q<Slider>("soundEffectSlider"));
        StyleSlider(root.Q<Slider>("musicSlider"));
        StyleSlider(root.Q<Slider>("vibrationSlider"));
        StyleSlider(root.Q<Slider>("brightnessSlider"));

        void StyleToggle(Toggle toggle)
        {
            toggle.AddToClassList("toggle-custom");
        }

        StyleToggle(root.Q<Toggle>("damageValueToggle"));
        StyleToggle(root.Q<Toggle>("ShowMapTrailToggle"));

        TextField nameInput;
        TextField timeInput;
        TextField progressInput;
        TextField deathsInput;
        Button addButton;


        nameInput = root.Q<TextField>("nameInput");
        nameInput.Q("unity-text-input").style.color = Color.black;
        timeInput = root.Q<TextField>("timeInput");
        timeInput.Q("unity-text-input").style.color = Color.black;
        progressInput = root.Q<TextField>("progressInput");
        progressInput.Q("unity-text-input").style.color = Color.black;
        deathsInput = root.Q<TextField>("deathsInput");
        deathsInput.Q("unity-text-input").style.color = Color.black;


        addButton = root.Q<Button>("addButton");
        leaderboardContainer = root.Q<VisualElement>("players");
        errorLabel = root.Q<Label>("errorText");
        addButton.clicked += () =>
        {
            float tiempo;
            float progreso;
            int muertes;

            bool tiempoOk = float.TryParse(timeInput.value, out tiempo);
            bool progresoOk = float.TryParse(progressInput.value, out progreso);
            bool muertesOk = int.TryParse(deathsInput.value, out muertes);

            if (leaderboard.Count >= 8)
            {
                ShowError("Maximum 8 players allowed");
                return;
            }

            if (!tiempoOk)
            {
                ShowError("Time  must be a number");
                return;
            }
            if (!progresoOk)
            {
                ShowError("Progress  must be a number");
                return;
            }
            if (!muertesOk)
            {
                ShowError("Deaths  must be a number");
                return;
            }
            if (progreso < 0 || progreso > 100)
            {
                ShowError("Progress must be a number between 0 and 100");
                return;
            }

            if (tiempo < 0 || muertes < 0)
            {
                ShowError("Deaths and time must be positive numbers");
                return;
            }

          
            ShowError(" "); 

            PlayerEntry entry = new PlayerEntry
            {
                nombre = nameInput.value,
                tiempo = tiempo,
                progreso = progreso,
                muertes = muertes
            };

            leaderboard.Add(entry);
            leaderboard.Sort((a, b) => b.Score().CompareTo(a.Score()));

            RefreshLeaderboard();
        };

        VisualElement leftArrow = controlsContent.Q<VisualElement>("leftArrow");
        VisualElement rightArrow = controlsContent.Q<VisualElement>("rightArrow");
        Image controlsImage = controlsContent.Q<Image>("controlsImage");
        Label controlText = controlsContent.Q<Label>("controllerTypeText");
        leftArrow.style.display = DisplayStyle.None;
        rightArrow.style.display = DisplayStyle.Flex;
        controlsImage.image = controllerTexture;
        controlText.text = "Controller";

        leftArrow.RegisterCallback<ClickEvent>(evt =>
        {
            leftArrow.style.display = DisplayStyle.None;
            rightArrow.style.display = DisplayStyle.Flex;
            controlsImage.image = controllerTexture;
            controlText.text = "Controller";
        });
        rightArrow.RegisterCallback<ClickEvent>(evt =>
        {
            leftArrow.style.display = DisplayStyle.Flex;
            rightArrow.style.display = DisplayStyle.None;
            controlsImage.image = keyboardTexture;
            controlText.text = "Keyboard";
        });

        VisualElement orisMenuButton = mainMenu.Q<VisualElement>("orisMenu");
        VisualElement orisMenu = root.Q<VisualElement>("OrisMenu");

        VisualElement exitOrisMenu = orisMenu.Q<VisualElement>("exit");
        VisualElement map = orisMenu.Q<VisualElement>("Map");
        VisualElement inventory = orisMenu.Q<VisualElement>("Inventory");
        VisualElement spiritShards = orisMenu.Q<VisualElement>("SpiritShards");

        VisualElement leftArrowOrisMenu = orisMenu.Q<VisualElement>("leftArrow");
        VisualElement rightArrowOrisMenu = orisMenu.Q<VisualElement>("rightArrow");

        VisualElement currentMenu = map;

        orisMenuButton.RegisterCallback<ClickEvent>(evt =>
        {
            mainMenu.style.display = DisplayStyle.None;
            orisMenu.style.display = DisplayStyle.Flex;

            map.style.display = DisplayStyle.Flex;
            inventory.style.display = DisplayStyle.None;
            spiritShards.style.display = DisplayStyle.None;
            leftArrowOrisMenu.style.display = DisplayStyle.None;
            rightArrowOrisMenu.style.display = DisplayStyle.Flex;
            currentMenu = map;
        });

        rightArrowOrisMenu.RegisterCallback<ClickEvent>(evt =>
        {
            if(currentMenu == map)
            {
                currentMenu = inventory;
                map.style.display = DisplayStyle.None;
                inventory.style.display = DisplayStyle.Flex;
                spiritShards.style.display = DisplayStyle.None;
                leftArrowOrisMenu.style.display = DisplayStyle.Flex;
            }
            else if (currentMenu == inventory)
            {
                currentMenu = spiritShards;
                map.style.display = DisplayStyle.None;
                inventory.style.display = DisplayStyle.None;
                spiritShards.style.display = DisplayStyle.Flex;
                rightArrowOrisMenu.style.display = DisplayStyle.None;
            }
        });

        leftArrowOrisMenu.RegisterCallback<ClickEvent>(evt =>
        {
            if (currentMenu == spiritShards)
            {
                currentMenu = inventory;
                map.style.display = DisplayStyle.None;
                inventory.style.display = DisplayStyle.Flex;
                spiritShards.style.display = DisplayStyle.None;
                rightArrowOrisMenu.style.display = DisplayStyle.Flex;
            }
            else if (currentMenu == inventory)
            {
                currentMenu = map;
                map.style.display = DisplayStyle.Flex;
                inventory.style.display = DisplayStyle.None;
                spiritShards.style.display = DisplayStyle.None;
                leftArrowOrisMenu.style.display = DisplayStyle.None;
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
    }
}
