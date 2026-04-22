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
        List<IconElement> options;

        int iconsPerRow = 8;
        int currentIndex = 0;

        private void OnEnable()
        {
            VisualElement root = GetComponent<UIDocument>().rootVisualElement;
            VisualElement menu = root.Q<VisualElement>("SpiritShards");

            displayContainer = menu.Q<VisualElement>("Elements");

            options = new List<IconElement>();

            rows = new List<VisualElement>
            {
                displayContainer.Q<VisualElement>("Row1"),
                displayContainer.Q<VisualElement>("Row2"),
                displayContainer.Q<VisualElement>("Row3"),
            };

            List<Icon> listaIconos = InventoryDatabase.getData();

            foreach (Icon icon in listaIconos)
            {
                CrearIcono(icon);
            }
        }

        void CrearIcono(Icon icon)
        {
            int rowIndex = currentIndex / iconsPerRow;

            if (rowIndex >= rows.Count)
            {
                Debug.LogWarning("No hay más espacio en las filas");
                return;
            }

            VisualElement row = rows[rowIndex];

            IconElement elemento = new IconElement(icon);

            elemento.OnClicked += OnIconClicked;
            elemento.OnHovered += OnIconHovered;

            options.Add(elemento);
            row.Add(elemento);

            currentIndex++;
        }

        void GuardarDatos()
        {
            List<IconData> dataToSave = new List<IconData>();

            foreach (IconElement elem in options)
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

            string ruta = Application.dataPath + "/JSON/SpiritShardsSelected.json";

            System.IO.File.WriteAllText(ruta, json);

            //Debug.Log("Guardado correcto");
        }

        void OnIconHovered(Icon icon)
        {
            
        }

       void OnIconClicked(IconElement ie, Icon icon, bool selected)
        {
            

        }

    }
}