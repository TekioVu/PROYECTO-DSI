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

                Debug.Log("Datos cargados desde JSON");
                return datos;
            }
            else
            {
                Debug.Log("No se encontró el JSON");
            }

            return new List<Icon>();
        }
    }
}