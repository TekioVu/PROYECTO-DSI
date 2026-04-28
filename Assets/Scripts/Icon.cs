using UnityEngine;
using System;

namespace SpiritShardNamespace
{
    [Serializable]
    public class Icon
    {
        [SerializeField] private Sprite image;
        public Sprite Image
        {
            get{return image;}
            set
            {
                if(value != image)
                {
                    image = value;
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
                }
            }
        }

        [SerializeField] private int price;
        public int Price
        {
            get{return price;}
            set
            {
                if(value != price)
                {
                    price = value;
                }
            }
        }

        [SerializeField] private bool sold;
        public bool Sold
        {
            get{return sold;}
            set
            {
                if(value != sold)
                {
                    sold = value;
                }
            }
        }

        public Icon(Sprite image, string name, string info)
        {
            this.image = image;
            this.name = name;
            this.info = info;
            this.sold = false;
            this.price = 0;
        }

        public Icon(Sprite image, string name, string info, int price)
        {
            this.image = image;
            this.name = name;
            this.info = info;
            this.sold = false;
            this.price = price;
        }
    }
}
