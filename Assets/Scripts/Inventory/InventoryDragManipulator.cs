using UnityEngine;
using UnityEngine.UIElements;
using System;

namespace SpiritShardNamespace
{
    public class InventoryDragManipulator : PointerManipulator
    {
        public Action<InventoryIcon, Vector2> OnDrop;

        private bool m_Active;
        private int m_PointerId;

        private InventoryIcon source;

        private VisualElement ghost;
        private VisualElement root;

        public InventoryDragManipulator(InventoryIcon icon)
        {
            source = icon;

            activators.Add(new ManipulatorActivationFilter
            {
                button = MouseButton.LeftMouse
            });
        }

        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<PointerDownEvent>(OnPointerDown);
            target.RegisterCallback<PointerMoveEvent>(OnPointerMove);
            target.RegisterCallback<PointerUpEvent>(OnPointerUp);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<PointerDownEvent>(OnPointerDown);
            target.UnregisterCallback<PointerMoveEvent>(OnPointerMove);
            target.UnregisterCallback<PointerUpEvent>(OnPointerUp);
        }

        void OnPointerDown(PointerDownEvent e)
        {
            if (!CanStartManipulation(e))
                return;

            m_Active = true;
            m_PointerId = e.pointerId;

            target.CapturePointer(m_PointerId);

            root = target.panel.visualTree;

            ghost = new VisualElement();
            ghost.style.position = Position.Absolute;

            ghost.style.width = 50;
            ghost.style.height = 50;

            Sprite sprite = source.GetSprite();

            if (sprite != null)
            {
                ghost.style.backgroundImage = new StyleBackground(sprite);
            }

            ghost.style.unityBackgroundScaleMode = ScaleMode.ScaleToFit;
            ghost.style.opacity = 0.9f;
            ghost.pickingMode = PickingMode.Ignore;

            root.Add(ghost);

            ghost.style.left = e.position.x - 25;
            ghost.style.top = e.position.y - 25;

            target.style.opacity = 0.3f;

            e.StopPropagation();
        }

        void OnPointerMove(PointerMoveEvent e)
        {
            if (!m_Active || !target.HasPointerCapture(m_PointerId))
                return;

            if (ghost != null)
            {
                ghost.style.left = e.position.x - 25; // centro
                ghost.style.top = e.position.y - 25;
            }

            e.StopPropagation();
        }

        void OnPointerUp(PointerUpEvent e)
        {
            if (!m_Active || !target.HasPointerCapture(m_PointerId))
                return;

            m_Active = false;

            target.ReleasePointer(m_PointerId);

            if (ghost != null && root != null)
            {
                root.Remove(ghost);
                ghost = null;
            }

            // restaurar original
            target.style.opacity = 1f;

            OnDrop?.Invoke(source, e.position);

            e.StopPropagation();
        }
    }
}