using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SpiritShardNamespace
{
    
    public class JsonHelperIcon : MonoBehaviour
    {
       public static List<IconData> FromJson<IconData>(string json)
        {
            ListaIndividuo<IconData> listaIndividuo = JsonUtility.FromJson<ListaIndividuo<IconData>>(json);
            return listaIndividuo.icons;
        }

        public static string ToJson<IconData>(List<IconData> lista)
        {
            ListaIndividuo<IconData> listaIndividuo = new ListaIndividuo<IconData>();
            listaIndividuo.icons = lista;
            return JsonUtility.ToJson(listaIndividuo);
        }

        public static string ToJson<IconData>(List<IconData> lista, bool prettyPrint)
        {
            ListaIndividuo<IconData> listaIndividuo = new ListaIndividuo<IconData>();
            listaIndividuo.icons = lista;
            return JsonUtility.ToJson(listaIndividuo, prettyPrint);
        }

        [Serializable]
        private class ListaIndividuo<IconData>
        {
            public List<IconData> icons;
        }
    }
}
