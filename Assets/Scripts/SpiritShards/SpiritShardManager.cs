using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;

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

            for(int i = 0; i < 8; i++)
            {
                Sprite sprite = Resources.Load<Sprite>("Icons/Ori2/Skills/Flap");
                Icon emptyIcon = new Icon(sprite, "Empty", "Select a spirit shard");
                CrearSelectedIcons(emptyIcon);
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

        void CrearSelectedIcons(Icon icon)
        {
            IconElement elemento = new IconElement(icon);
            elemento.DisplayIcon = true;
            selectedIcons.Add(elemento);

            elemento.OnClicked += OnIconClicked;
            elemento.OnHovered += OnIconHovered;

            selectedContainer.Add(elemento);
        }

        void OnIconHovered(Icon icon)
        {
            currentImage.style.backgroundImage = new StyleBackground(icon.Image);
            currentName.text = icon.Name;
            currentDesc.text = icon.Info;
        }

       void OnIconClicked(Icon icon, bool selected)
        {
            if (selected)
            {
                foreach (IconElement slot in selectedIcons)
                {
                    if (!slot.Selected)
                    {
                        slot.SetIcon(icon);
                        return;
                    }
                }
            }
            else
            {
                foreach (IconElement slot in selectedIcons)
                {
                    if (slot.Data.Name == icon.Name)
                    {
                        slot.SetEmpty();
                        return;
                    }
                }
            }
        }
    }
}