using System.Collections.Generic;
using UIToolkitDemo;
using UnityEngine;
using UnityEngine.UIElements;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private Texture2D controllerTexture;
    [SerializeField] private Texture2D keyboardTexture;

    VisualElement optionsMenu;
    VisualElement leaderboardContainer;
    Label errorLabel;

    List<PlayerEntry> leaderboard = new List<PlayerEntry>();

    void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        optionsMenu = root.Q<VisualElement>("OptionsMenu");

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
            root.Q<VisualElement>("InitMenu").style.display = DisplayStyle.Flex;
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

        void StyleSlider(Slider slider) => slider.AddToClassList("slider-custom");

        StyleSlider(root.Q<Slider>("soundEffectSlider"));
        StyleSlider(root.Q<Slider>("musicSlider"));
        StyleSlider(root.Q<Slider>("vibrationSlider"));
        StyleSlider(root.Q<Slider>("brightnessSlider"));

        void StyleToggle(Toggle toggle) => toggle.AddToClassList("toggle-custom");

        StyleToggle(root.Q<Toggle>("damageValueToggle"));
        StyleToggle(root.Q<Toggle>("ShowMapTrailToggle"));

        var nameInput = root.Q<TextField>("nameInput");
        var timeInput = root.Q<TextField>("timeInput");
        var progressInput = root.Q<TextField>("progressInput");
        var deathsInput = root.Q<TextField>("deathsInput");

        nameInput.Q("unity-text-input").style.color = Color.black;
        timeInput.Q("unity-text-input").style.color = Color.black;
        progressInput.Q("unity-text-input").style.color = Color.black;
        deathsInput.Q("unity-text-input").style.color = Color.black;

        var addButton = root.Q<Button>("addButton");
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

            if (!tiempoOk) { ShowError("Time  must be a number"); return; }
            if (!progresoOk) { ShowError("Progress  must be a number"); return; }
            if (!muertesOk) { ShowError("Deaths  must be a number"); return; }

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

        Achievments();
    }

    void Achievments()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        var oldContainer = root.Q<VisualElement>("achievmentsContent");
        ScrollView scroll = new ScrollView(ScrollViewMode.Vertical);
        scroll.style.flexGrow = 1;
        scroll.AddToClassList("my-scroll");

        oldContainer.Clear();
        oldContainer.Add(scroll);
        VisualTreeAsset achievementTemplate =
            Resources.Load<VisualTreeAsset>("Templates/achievementTemplate");

        int columns = 3;
        int rows = 5;

        VisualElement grid = new VisualElement();
        grid.style.flexDirection = FlexDirection.Column;

        for (int r = 0; r < rows; r++)
        {
            VisualElement row = new VisualElement();
            row.style.flexDirection = FlexDirection.Row;

            for (int c = 0; c < columns; c++)
            {
                VisualElement card = achievementTemplate.Instantiate();

                card.style.paddingRight = 20;
                card.style.paddingBottom = 20;
                card.style.paddingTop = 20;
                card.style.paddingLeft = 20;
                card.style.width = 300;
                card.style.height = 200;
                row.Add(card);
            }

            grid.Add(row);


        }

        scroll.Add(grid);

        foreach (var row in grid.Children())
        {
            foreach (var card in row.Children())
            {
                var progressButton = card.Q<Label>("progressButton");
                var radial = card.Q<RadialProgress>();

                if (radial != null)
                    radial.Progress = 0;

                if (progressButton != null && radial != null)
                {
                    progressButton.RegisterCallback<ClickEvent>(evt =>
                    {
                        radial.Progress = Mathf.Clamp(radial.Progress + 20f, 0f, 100f);
                    });
                }
            }
        }
    }

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
                    l.style.width = 90;
                    l.style.unityTextAlign = TextAnchor.MiddleCenter;
                    row.Add(l);
                }

                row.Add(deleteBtn);
                leaderboardContainer.Add(row);
            }
        
    }

        void ShowError(string message)
        {
            errorLabel.text = message;
            errorLabel.style.display = DisplayStyle.Flex;
        }
    
}