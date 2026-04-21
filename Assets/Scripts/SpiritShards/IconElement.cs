using UnityEngine;
using UnityEngine.UIElements;
using System;
using System.Runtime.CompilerServices;

namespace SpiritShardNamespace
{
    public class IconElement : VisualElement
    {
        public event Action<Icon, bool> OnClicked;
        public event Action<Icon> OnHovered;

        private VisualElement icono;
        private Icon data;
        private bool selected = false;
        private bool displayIcon = false;

        private VisualElement background;
        public Icon Data
        {
            get{return data;}
        }
        public bool Selected
        {
            get{return selected;}
            set{selected = value; 
            }
        }

        public bool DisplayIcon
        {
            get{return displayIcon;}
            set{displayIcon = value;
            style.width = 60;
            style.height = 60;}
        }

        public IconElement(Icon icon)
        {
            data = icon;

            style.width = 50;
            style.height = 50;

            background = new VisualElement();
            background.style.position = Position.Absolute;
            background.style.top = -8;
            background.style.left = -8;
            background.style.right = -8;
            background.style.bottom = -8;

            background.style.backgroundImage = new StyleBackground(
                Resources.Load<Sprite>("Icons/SkillBackground")
            );

            background.style.unityBackgroundScaleMode = ScaleMode.ScaleToFit;

            icono = new VisualElement();
            icono.style.position = Position.Absolute;
            icono.style.top = 2;
            icono.style.left = 2;
            icono.style.right = 2;
            icono.style.bottom = 2;

            icono.style.unityBackgroundImageTintColor = new Color(227f/255f, 245f/255f, 255f/255f);
            icono.style.unityBackgroundScaleMode = ScaleMode.ScaleToFit;

            if (icon.Image != null)
            {
                icono.style.backgroundImage = new StyleBackground(icon.Image);
            }

            Add(background); // abajo
            Add(icono);      // encima

            // Eventos
            RegisterCallback<MouseDownEvent>(OnMouseDown);
            RegisterCallback<MouseEnterEvent>(OnMouseEnter);
            RegisterCallback<MouseLeaveEvent>(OnMouseLeave);
        }

        public void SetIcon(Icon icon)
        {
            data = icon;
            selected = true;
            icono.style.backgroundImage = icon.Image != null ? new StyleBackground(icon.Image) : null;
        }

        public void SetEmpty()
        {
            Sprite sprite = Resources.Load<Sprite>("Icons/Ori2/Skills/Flap");
            data = new Icon(sprite, "Empty", "Select a spirit shard");
            icono.style.backgroundImage = new StyleBackground(sprite);

            selected = false;
        }

        void OnMouseDown(MouseDownEvent evt)
        {
            if(!selected && displayIcon) return;

            selected = !selected;
            OnClicked?.Invoke(data, selected);
        }

        void OnMouseEnter(MouseEnterEvent evt)
        {
            OnHovered?.Invoke(data);
        }

        void OnMouseLeave(MouseLeaveEvent evt)
        {
        }
    }
}