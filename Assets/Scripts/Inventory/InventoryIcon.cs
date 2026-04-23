using UnityEngine;
using UnityEngine.UIElements;
using System;
using System.Runtime.CompilerServices;

namespace SpiritShardNamespace
{
    public class InventoryIcon : VisualElement
    {
        public event Action<InventoryIcon, Icon> OnClicked;
        public event Action<InventoryIcon, Icon> OnHovered;
        public event Action OnExit;

        private VisualElement icono;
        private Icon data;

        private VisualElement background;
        public Icon Data
        {
            get{return data;}
        }

        public InventoryIcon(Icon icon)
        {
            data = icon;

            style.width = 60;
            style.height = 60;

            background = new VisualElement();
            background.style.position = Position.Absolute;
            background.style.top = -8;
            background.style.left = -8;
            background.style.right = -8;
            background.style.bottom = -8;

            background.style.backgroundImage = new StyleBackground(Resources.Load<Sprite>("Icons/SkillBackground"));

            background.style.unityBackgroundScaleMode = ScaleMode.ScaleToFit;
            background.style.unityBackgroundImageTintColor = new Color(0.25f, 0.25f, 0.25f, 1f);

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

            Add(background); 
            Add(icono);      

            // Eventos
            RegisterCallback<MouseDownEvent>(OnMouseDown);
            RegisterCallback<MouseEnterEvent>(OnMouseEnter);
            RegisterCallback<MouseLeaveEvent>(OnMouseLeave);
        }

        public void SetIcon(Icon icon)
        {
            data = icon;
            icono.style.backgroundImage = icon.Image != null ? new StyleBackground(icon.Image) : null;
        }

        public void SetEmpty()
        {
            Sprite sprite = Resources.Load<Sprite>("Icons/Ori1/AbilityTree/Combat/ChargeFlame");
            data = new Icon(sprite, "Empty", "Select a spirit shard");
            icono.style.backgroundImage = new StyleBackground(sprite);
        }

        void OnMouseDown(MouseDownEvent evt)
        {
            OnClicked?.Invoke(this, data);
        }

        void OnMouseEnter(MouseEnterEvent evt)
        {
            OnHovered?.Invoke(this, data);

            background.style.unityBackgroundImageTintColor =new Color(1, 1, 1, 1);
        }

        void OnMouseLeave(MouseLeaveEvent evt)
        {
            OnExit?.Invoke();
            background.style.unityBackgroundImageTintColor =  new Color(0.25f, 0.25f, 0.25f, 1f);
        }
    }
}