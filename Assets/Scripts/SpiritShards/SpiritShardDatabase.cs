using UnityEngine;
using System.Collections.Generic;
using System.IO; 
using System;

namespace SpiritShardNamespace
{
    [Serializable]
    public class IconData
    {
        public string imagen;
        public string nombre;
        public string descripcion;
    }

    public class SpiritShardDatabase : MonoBehaviour
    {
        public static List<Icon> getData()
        {
            string ruta = Application.dataPath + "/JSON/SpiritShardsOptions.json";

            if (File.Exists(ruta))
            {
                string json = File.ReadAllText(ruta);

                List<IconData> datosJson = JsonHelperIcon.FromJson<IconData>(json);

                List<Icon> datos = new List<Icon>();

                foreach (var d in datosJson)
                {
                    Sprite sprite = Resources.Load<Sprite>(d.imagen);
                    if (sprite == null)
                    {
                        Debug.LogWarning("Sprite no encontrado: " + d.imagen);
                    }

                    datos.Add(new Icon(sprite, d.nombre, d.descripcion));
                }

                Debug.Log("SpiritShard options cargadas desde JSON");
                return datos;
            }
            else
            {
                Debug.Log("No se encontró el JSON");
            }

            return new List<Icon>();
        }

        public static List<Icon> getSelectedData()
        {
            string ruta = Application.dataPath + "/JSON/SpiritShardsSelected.json";

            if (!File.Exists(ruta))
            {
                Debug.Log("No se encontró el JSON");
                return new List<Icon>();
            }

            string json = File.ReadAllText(ruta);

            List<IconData> selectedJson = JsonHelperIcon.FromJson<IconData>(json);

            List<Icon> allIcons = getData();

            List<Icon> selectedIcons = new List<Icon>();

            foreach (var selected in selectedJson)
            {
                Icon match = allIcons.Find(i => i.Name == selected.nombre);

                if (match != null)
                {
                    selectedIcons.Add(match);
                }else if(selected.nombre == "Empty")
                {
                    Sprite sprite = Resources.Load<Sprite>("Icons/Ori2/Skills/Flap");
                    selectedIcons.Add(new Icon(sprite, "Empty", "Select a spirit shard"));
                }
                else
                {
                    Debug.LogWarning("Icon no encontrado: " + selected.nombre);
                }
            }

            Debug.Log("Selected cargados correctamente");
            return selectedIcons;
        }
    }
}