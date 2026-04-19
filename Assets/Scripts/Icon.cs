using UnityEngine;
using System;

namespace SpiritShardNamespace
{
    [Serializable]
    public class Icon
    {
        public event Action Cambio;

        [SerializeField] private Sprite image;
        public Sprite Image
        {
            get{return image;}
            set
            {
                if(value != image)
                {
                    image = value;
                    Cambio?.Invoke();
                }
            }
        }
        [SerializeField] private string name;
        public string Name
        {
            get{return name; }
            set
            {
                if(value != name)
                {
                    name = value;
                    Cambio?.Invoke();
                }
            }
        }

        [SerializeField] private string info;
        public string Info
        {
            get{return info;}
            set
            {
                if(value != info)
                {
                    info = value;
                    Cambio?.Invoke();
                }
            }
        }

        public Icon(Sprite image, string name, string info)
        {
            this.image = image;
            this.name = name;
            this.info = info;
        }
    }
}
