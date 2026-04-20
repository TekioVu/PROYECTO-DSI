using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace SpiritShardNamespace
{
    public class SpiritShardManager : MonoBehaviour
    {
        VisualElement contenedor;
        List<VisualElement> rows;

        int iconsPerRow = 8;
        int currentIndex = 0;

        private void OnEnable()
        {
            VisualElement root = GetComponent<UIDocument>().rootVisualElement;

            contenedor = root.Q<VisualElement>("Options");

            rows = new List<VisualElement>
            {
                contenedor.Q<VisualElement>("Row1"),
                contenedor.Q<VisualElement>("Row2"),
                contenedor.Q<VisualElement>("Row3"),
                contenedor.Q<VisualElement>("Row4"),
            };

            List<Icon> listaIconos = SpiritShardDatabase.getData();

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

            VisualTreeAsset plantilla = Resources.Load<VisualTreeAsset>("Icon");
            VisualElement tarjeta = plantilla.Instantiate();

            VisualElement iconoReal = tarjeta[0];
            if (icon.Image != null) {
                iconoReal.style.backgroundImage = new StyleBackground(icon.Image);
            }

            row.Add(iconoReal);

            currentIndex++;
        }
    }
}