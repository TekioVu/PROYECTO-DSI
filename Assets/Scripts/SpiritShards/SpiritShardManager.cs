using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;
using System;

namespace SpiritShardNamespace
{
    public class SpiritShardManager : MonoBehaviour
    {
        VisualElement optionsContainer;
        VisualElement displayContainer;
        VisualElement selectedContainer;
        List<VisualElement> rows;

        int iconsPerRow = 8;
        int currentIndex = 0;

        VisualElement currentImage;
        Label currentName;
        Label currentDesc;
        List<IconElement> selectedIcons;

        private void OnEnable()
        {
            VisualElement root = GetComponent<UIDocument>().rootVisualElement;

            optionsContainer = root.Q<VisualElement>("Options");
            displayContainer = root.Q<VisualElement>("Info");
            selectedContainer = root.Q<VisualElement>("Selected");

            currentImage = displayContainer.Q<VisualElement>("SkillImage");
            currentName = displayContainer.Q<Label>("SkillName");
            currentDesc = displayContainer.Q<Label>("SkillInfo");

            selectedIcons = new List<IconElement>();

            rows = new List<VisualElement>
            {
                optionsContainer.Q<VisualElement>("Row1"),
                optionsContainer.Q<VisualElement>("Row2"),
                optionsContainer.Q<VisualElement>("Row3"),
                optionsContainer.Q<VisualElement>("Row4"),
            };

            List<Icon> listaIconos = SpiritShardDatabase.getData();

            foreach (Icon icon in listaIconos)
            {
                CrearIcono(icon);
            }

            List<Icon> selectedIconList = SpiritShardDatabase.getSelectedData();
            if(selectedIconList.Count == 0)
            {
                for(int i = 0; i < 8; i++)
                {
                    Sprite sprite = Resources.Load<Sprite>("Icons/Ori2/Skills/Flap");
                    Icon emptyIcon = new Icon(sprite, "Empty", "Select a spirit shard");
                    CrearSelectedIcons(emptyIcon, false);
                }
            }
            else
            {
                foreach (Icon icon in selectedIconList)
                {
                    CrearSelectedIcons(icon, true);
                }
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

            row.Add(elemento);

            currentIndex++;
        }

        void CrearSelectedIcons(Icon icon, bool JSON)
        {
            IconElement elemento = new IconElement(icon);
            elemento.DisplayIcon = true;
            elemento.Selected = JSON && icon.Name != "Empty" ? true : false;
            selectedIcons.Add(elemento);

            elemento.OnClicked += OnIconClicked;
            elemento.OnHovered += OnIconHovered;

            selectedContainer.Add(elemento);
        }

        void GuardarDatos()
        {
            List<IconData> dataToSave = new List<IconData>();

            foreach (IconElement elem in selectedIcons)
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
            currentImage.style.backgroundImage = new StyleBackground(icon.Image);
            currentName.text = icon.Name;
            currentDesc.text = icon.Info;
        }

       void OnIconClicked(Icon icon, bool selected)
        {
            if (selected){
                foreach (IconElement slot in selectedIcons)
                {
                    if (!slot.Selected)
                    {
                        slot.SetIcon(icon);
                        GuardarDatos();
                        return;
                    }
                }
            }
            else{
                foreach (IconElement slot in selectedIcons)
                {
                    if (slot.Data.Name == icon.Name)
                    {
                        slot.SetEmpty();
                        GuardarDatos();
                        return;
                    }
                }
            }

        }
    }
}