using UnityEngine;
using System.Collections.Generic;
using System.IO; 
using System;

namespace SpiritShardNamespace
{
    public class InventoryDatabase : MonoBehaviour
    {
        public static List<Icon> getInfo()
        {
            string ruta = Application.dataPath + "/JSON/InventoryInfo.json";

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

                //Debug.Log("Inventory cargadas desde JSON");
                return datos;
            }
            else
            {
                Debug.Log("No se encontró el JSON");
            }

            return new List<Icon>();
        }

        public static List<Icon> getData()
        {
            string ruta = Application.dataPath + "/JSON/Inventory.json";

            if (!File.Exists(ruta))
            {
                Debug.Log("No se encontró el JSON");
                return new List<Icon>();
            }

            string json = File.ReadAllText(ruta);

            List<IconData> selectedJson = JsonHelperIcon.FromJson<IconData>(json);

            List<Icon> allIcons = getInfo();

            List<Icon> selectedIcons = new List<Icon>();

            foreach (var selected in selectedJson)
            {
                Icon match = allIcons.Find(i => i.Name == selected.nombre);

                if (match != null)
                {
                    selectedIcons.Add(match);
                }else if(selected.nombre == "Empty")
                {
                    Sprite sprite = Resources.Load<Sprite>("Icons/Ori1/AbilityTree/Combat/ChargeFlame");
                    selectedIcons.Add(new Icon(sprite, "Empty", "empty"));
                }
                else
                {
                    Debug.LogWarning("Icon no encontrado: " + selected.nombre);
                }
            }

            //Debug.Log("Selected cargados correctamente");
            return selectedIcons;
        }

       
    }
}