using UnityEngine;
using System.Collections.Generic;
using System.IO; 
using System;

namespace SpiritShardNamespace
{
    public class ShopDatabase : MonoBehaviour
    {
        public static List<Icon> getData()
        {
            string ruta = Application.dataPath + "/JSON/ShopOptions.json";

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

                    int price = UnityEngine.Random.Range(5, 40) * 100;
                    datos.Add(new Icon(sprite, d.nombre, d.descripcion, price));
                }

                //Debug.Log("SpiritShard options cargadas desde JSON");
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