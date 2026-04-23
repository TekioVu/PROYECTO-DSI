using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;
using System;

namespace SpiritShardNamespace
{
    public class InventoryManager : MonoBehaviour
    {
        VisualElement displayContainer;
        List<VisualElement> rows;
        List<InventoryIcon> options;

        int[] rowCapacities = new int[] { 8, 7, 6 };

        //Vetnana de info 
        private VisualTreeAsset infoTemplate;
        private VisualElement currentInfoWindow;
        private VisualElement root;

        private void OnEnable()
        {
            root = GetComponent<UIDocument>().rootVisualElement;
            infoTemplate = Resources.Load<VisualTreeAsset>("Templates/InfoWindow");
            VisualElement menu = root.Q<VisualElement>("Inventory");

            displayContainer = menu.Q<VisualElement>("Elements");

            options = new List<InventoryIcon>();

            rows = new List<VisualElement>
            {
                displayContainer.Q<VisualElement>("Row1"),
                displayContainer.Q<VisualElement>("Row2"),
                displayContainer.Q<VisualElement>("Row3"),
            };

            List<Icon> listaIconos = InventoryDatabase.getData();

            int index = 0;

            foreach (Icon icon in listaIconos)
            {
                AddIconToLayout(icon, ref index);
            }

            Sprite sprite = Resources.Load<Sprite>("Icons/Ori1/AbilityTree/Combat/ChargeFlame");

            while (index < 21)
            {
                Icon emptyIcon = new Icon(sprite, "Empty", "Select a spirit shard");
                AddIconToLayout(emptyIcon, ref index);
            }
        }
        void AddIconToLayout(Icon icon, ref int index)
        {
            int rowIndex = 0;
            int count = index;

            // calcular fila correcta
            while (count >= rowCapacities[rowIndex])
            {
                count -= rowCapacities[rowIndex];
                rowIndex++;
            }

            VisualElement row = rows[rowIndex];

            InventoryIcon elemento = new InventoryIcon(icon);

            elemento.OnClicked += OnIconClicked;
            elemento.OnHovered += OnIconHovered;
            elemento.OnExit += OnIconExit;

            options.Add(elemento);
            row.Add(elemento);

            index++;
        }
        void GuardarDatos()
        {
            List<IconData> dataToSave = new List<IconData>();

            foreach (InventoryIcon elem in options)
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

        void OnIconHovered(InventoryIcon inventoryIcon, Icon icon)
        {
            ShowInfoWindow(inventoryIcon, icon);
        }

        void OnIconExit()
        {
            HideInfoWindow();
        }


       void OnIconClicked(InventoryIcon ie, Icon icon)
        {
            
        }

        private void ShowInfoWindow(InventoryIcon icon, Icon data)
        {
            if (currentInfoWindow != null)
                root.Remove(currentInfoWindow);

            currentInfoWindow = infoTemplate.Instantiate();

            currentInfoWindow.Q<Label>("Title").text = data.Name;
            currentInfoWindow.Q<Label>("Desc").text = data.Info;

            // Posición
            Vector2 pos = icon.worldBound.position;

            currentInfoWindow.style.position = Position.Absolute;
            currentInfoWindow.style.left = pos.x + icon.layout.width + - 375;
            currentInfoWindow.style.top = pos.y - 20;

            root.Add(currentInfoWindow);
        }

        private void HideInfoWindow()
        {
            if (currentInfoWindow != null)
            {
                root.Remove(currentInfoWindow);
                currentInfoWindow = null;
            }
        }

    }
}