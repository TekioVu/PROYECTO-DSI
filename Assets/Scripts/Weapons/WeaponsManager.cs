using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;
using System;
using UnityEngine.InputSystem;
using Unity.Properties;

namespace SpiritShardNamespace
{
    public class WeaponsManager : MonoBehaviour
    {
        int currentIndex = 0;
        int currentSelected = 0;
        List<WeaponIcon> options;

        VisualElement initMenu;

        //Display
        VisualElement menu;
        VisualElement displayContainer;
        VisualElement optionsDisplay;
        Label displayName;
        Label displayDesc;
        VisualElement displayArrow;

        //Selected
        VisualElement selectedWeapon;
        VisualElement selectedImage;
        Label selectedName;
        Label selectedDesc;

        Label exit;
        private void OnEnable()
        {
            VisualElement root = GetComponent<UIDocument>().rootVisualElement;
            menu = root.Q<VisualElement>("Weapon");

            initMenu = root.Q<VisualElement>("InitMenu");

            displayContainer = menu.Q<VisualElement>("Body");
            displayName = displayContainer.Q<Label>("Name");
            displayDesc = displayContainer.Q<Label>("Desc");
            displayArrow = displayContainer.Q<VisualElement>("Arrow");
            optionsDisplay = displayContainer.Q<VisualElement>("Elements");

            selectedWeapon = menu.Q<VisualElement>("SelectedWeapon");
            selectedImage = selectedWeapon.Q<VisualElement>("SelectedImg");
            selectedName = selectedWeapon.Q<Label>("SelectedName");
            selectedDesc = selectedWeapon.Q<Label>("SelectedDesc");
            exit = menu.Q<Label>("exit");


            exit.RegisterCallback<ClickEvent>(evt =>
            {
                menu.style.display = DisplayStyle.None;
                initMenu.style.display = DisplayStyle.Flex;
            });

            options = new List<WeaponIcon>();

            List<Icon> listaIconos = WeaponsDatabase.getData();

            foreach (Icon icon in listaIconos)
            {
                AddIconToLayout(icon);
            }
        }

        void Update()
        {
            if (Keyboard.current.qKey.wasPressedThisFrame)
            {
                ShowMenu();
            }
        }

       void AddIconToLayout(Icon icon)
        {
            float radius = 150f;
            int count = 12;

            float angleStep = 360f / count;
            float angle = currentIndex * angleStep * Mathf.Deg2Rad;

            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;

            WeaponIcon elemento = new WeaponIcon(icon);

            elemento.AddToClassList("weapon");

            elemento.style.left = Length.Percent(47);
            elemento.style.top = Length.Percent(30);
            elemento.style.translate = new Translate(x, y);

            options.Add(elemento);

            elemento.OnClicked += OnIconClicked;
            elemento.OnHovered += OnIconHovered;

            optionsDisplay.Add(elemento);
            currentIndex++;
        }
        void GuardarDatos()
        {
            List<IconData> dataToSave = new List<IconData>();

            foreach (WeaponIcon elem in options)
            {
                if (elem.Data != null)
                {
                    dataToSave.Add(new IconData
                    {
                        nombre = elem.Data.Name,
                    });
                }
            }

            string json = JsonHelperIcon.ToJson(dataToSave, true);

            string ruta = Application.dataPath + "/JSON/Inventory.json";

            System.IO.File.WriteAllText(ruta, json);

            //Debug.Log("Guardado correcto");
        }

        void OnIconHovered(Icon icon)
        {
            displayName.text = icon.Name;
            displayDesc.text = icon.Info;
            
            int i = 0;
            foreach (WeaponIcon elem in options)
            {
                if(elem.Data.Name == icon.Name)
                {
                    currentSelected = i;
                    break;
                }
                i++;
            }
            UpdateArrow();
        }

       void UpdateArrow()
        {
            float radius = 90f;
            int count = 12;

            float angleStep = 360f / count;

            float angleDeg = currentSelected * angleStep;

            float angle = angleDeg * Mathf.Deg2Rad;

            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;

            displayArrow.style.left = Length.Percent(43);
            displayArrow.style.top = Length.Percent(28);
            displayArrow.style.translate = new Translate(x, y);

            displayArrow.style.rotate = new Rotate(new Angle(angleDeg - 90f, AngleUnit.Degree));
        }

        void OnIconExit()
        {
        }


       void OnIconClicked(Icon icon)
        {
            selectedImage.style.backgroundImage = icon.Image != null ? new StyleBackground(icon.Image) : null;
            selectedName.text = icon.Name;
            selectedDesc.text = icon.Info;
            ShowMenu();
        }

        void ShowMenu()
        {
          displayContainer.style.visibility = displayContainer.style.visibility == Visibility.Hidden
          ? Visibility.Visible : Visibility.Hidden;

          selectedWeapon.style.visibility = displayContainer.style.visibility == Visibility.Hidden
          ? Visibility.Visible : Visibility.Hidden;

          menu.style.unityBackgroundImageTintColor = displayContainer.style.visibility == Visibility.Hidden ? new Color(0.87f, 0.87f, 87f) : new Color(0.45f, 0.45f, 0.45f);
        }

    }
}