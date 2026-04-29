using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;
using System;
using Unity.VisualScripting;

namespace SpiritShardNamespace
{
    struct OptionSlot
    {
        public VisualElement element;
        public VisualElement image;
        public Label name;
        public Label desc;
        public Label priceDisplay;
        public Icon data;

        public OptionSlot(VisualElement _el, VisualElement _im, Label _name, Label _desc, Icon _data, Label _price)
        {
            element = _el;
            image = _im;
            name = _name;
            desc = _desc;
            data = _data;
            priceDisplay = _price;
        }
    }

    public class ShopManager : MonoBehaviour
    {
        VisualElement optionsContainer;
        OptionSlot[] currentOptions = new OptionSlot[3];
        List<Icon> options;
        int currentIndex = 1;
        int currentCoins = 10000;
        VisualElement leftArrow;
        VisualElement rightArrow;
        Label coinText;
        VisualElement addCoins;


        VisualElement initMenu;
        Label exit;

        private void OnEnable()
        {
            VisualElement root = GetComponent<UIDocument>().rootVisualElement;
            VisualElement menu = root.Q<VisualElement>("Shop");
            optionsContainer = menu.Q<VisualElement>("Body");

            leftArrow = menu.Q<VisualElement>("LeftArrow");
            rightArrow = menu.Q<VisualElement>("RightArrow");
            coinText = menu.Q<Label>("CoinText");
            addCoins = menu.Q<VisualElement>("SpiritCellButton");

            initMenu = root.Q<VisualElement>("InitMenu");
            exit = menu.Q<Label>("exit");

            leftArrow.RegisterCallback<MouseDownEvent>(MoveLeft);
            rightArrow.RegisterCallback<MouseDownEvent>(MoveRight);

            options = new List<Icon>();
            options = ShopDatabase.getData();

            for(int i = 0; i < currentOptions.Length; i++)
            {
                int index = i;

                currentOptions[index].element = optionsContainer.Q<VisualElement>("Info" + index);
                currentOptions[index].image = currentOptions[index].element.Q<VisualElement>("SkillImage");
                currentOptions[index].name = currentOptions[index].element.Q<Label>("SkillName");
                currentOptions[index].desc = currentOptions[index].element.Q<Label>("SkillInfo");
                currentOptions[index].data = options[index];
                currentOptions[index].priceDisplay = currentOptions[index].element.Q<Label>("CoinText");

                currentOptions[index].element.RegisterCallback<ClickEvent>(evt =>
                {
                    BuyItem(currentOptions[index].data);
                });
            }

            exit.RegisterCallback<ClickEvent>(evt =>
            {
                menu.style.display = DisplayStyle.None;
                initMenu.style.display = DisplayStyle.Flex;
            });

            addCoins.RegisterCallback<ClickEvent>(evt =>{
                OnAddCoins();
            });

            UpdateDisplay();
        }

        void GuardarDatos()
        {
            
        }

        void MoveLeft(MouseDownEvent evt)
        {
            currentIndex--;
            if(currentIndex < 0) currentIndex = options.Count - 1;

            UpdateDisplay();
        }

        void MoveRight(MouseDownEvent evt)
        {
            currentIndex++;
            if(currentIndex >= options.Count) currentIndex = 0;

            UpdateDisplay();
        }
        
        void UpdateDisplay()
        {
            int leftIndex  = (currentIndex - 1 + options.Count) % options.Count;
            int rightIndex = (currentIndex + 1) % options.Count;

            currentOptions[0].data = options[leftIndex];
            currentOptions[1].data = options[currentIndex];
            currentOptions[2].data = options[rightIndex];

            for(int i = 0; i < currentOptions.Length; i++)
            {
                currentOptions[i].image.style.backgroundImage = new StyleBackground(currentOptions[i].data.Image);
                currentOptions[i].name.text = currentOptions[i].data.Name; 
                currentOptions[i].desc.text = currentOptions[i].data.Info; 
                currentOptions[i].priceDisplay.text = currentOptions[i].data.Sold ? "OWNED" : currentOptions[i].data.Price.ToString();

                if(currentOptions[i].data.Sold){
                    currentOptions[i].priceDisplay.style.color = Color.green;
                }else if(currentCoins < currentOptions[i].data.Price){
                    currentOptions[i].priceDisplay.style.color = Color.red;
                }else{
                    currentOptions[i].priceDisplay.style.color = Color.white;
                }
            }

            coinText.text = currentCoins.ToString();
        }

        void BuyItem(Icon icon)
        {
            if(icon.Sold || icon.Price > currentCoins) return;
            
            Debug.Log("Buying: " + icon.Name);
            icon.Sold = true;
            currentCoins -= icon.Price;

            UpdateDisplay();
        }

        void OnAddCoins()
        {
            int coins = UnityEngine.Random.Range(5, 40) * 100;
            currentCoins += coins;
            UpdateDisplay();
        }
    }
}