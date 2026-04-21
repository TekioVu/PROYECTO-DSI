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
        public Icon Data
        {
            get{return data;}
        }
        public bool Selected
        {
            get{return selected;}
            set{selected = value; }
        }

        public bool DisplayIcon
        {
            get{return displayIcon;}
            set{displayIcon = value;}
        }

        public IconElement(Icon icon)
        {
            data = icon;

            icono = new VisualElement();

            icono.style.flexGrow = 1;
            icono.style.aspectRatio = 1;
            icono.style.unityBackgroundImageTintColor = new Color(227f/255f, 245f/255f, 255f/255f);

            icono.style.width = 50;
            icono.style.height = 50;

            //icono.style.unityBackgroundScaleMode = ScaleMode.ScaleToFit;

            if (icon.Image != null)
            {
                icono.style.backgroundImage = new StyleBackground(icon.Image);
            }

            Add(icono);

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