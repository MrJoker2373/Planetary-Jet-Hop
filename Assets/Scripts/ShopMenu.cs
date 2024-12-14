namespace SpaceGame
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;

    public class ShopMenu : Menu
    {
        [SerializeField] private StatsMenu _stats;
        [SerializeField] private TextMeshProUGUI _titleLabel;
        [SerializeField] private TextMeshProUGUI _descriptionLabel;
        [SerializeField] private TextMeshProUGUI _priceLabel;
        [SerializeField] private Image _preview;
        [SerializeField] private Button _equipButton;
        [SerializeField] private Button _buyButton;
        [SerializeField] private Good[] _goods;
        private int _currentGood;

        private void Start()
        {
            if (PlayerPrefs.HasKey("Money") == false)
            {
                PlayerPrefs.SetInt("Money", 0);
                PlayerPrefs.Save();
            }
            if (PlayerPrefs.HasKey("PurchasedGood1") == false)
            {
                PlayerPrefs.SetInt("PurchasedGood1", 0);
                PlayerPrefs.Save();
            }
            if (PlayerPrefs.HasKey("EquipedShip") == false)
            {
                PlayerPrefs.SetInt("EquipedShip", 0);
                PlayerPrefs.Save();
            }
        }

        public override void Open()
        {
            base.Open();
            Refresh();
            _stats.Refresh();
        }

        public override IEnumerator OpenAsync()
        {
            yield return base.OpenAsync();
            Refresh();
            _stats.Refresh();
        }

        public void MoveBackward()
        {
            if (_currentGood == 0)
                _currentGood = _goods.Length - 1;
            else
                _currentGood--;
            Refresh();
        }

        public void MoveForward()
        {
            if (_currentGood == _goods.Length - 1)
                _currentGood = 0;
            else
                _currentGood++;
            Refresh();
        }

        public void Equip()
        {
            var good = _goods[_currentGood];
            PlayerPrefs.SetInt("EquipedShip", good.ID);
            PlayerPrefs.Save();
            _stats.Refresh();
            Refresh();
        }

        public void Buy()
        {
            if (PlayerPrefs.HasKey("CompletedAchieve3") == false)
                PlayerPrefs.SetInt("CompletedAchieve3", 2);

            var good = _goods[_currentGood];
            PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") - good.Price);
            var purchasedGoods = new List<int>();
            for (int i = 0; ; i++)
            {
                if (PlayerPrefs.HasKey("PurchasedGood" + (i + 1)) == false)
                    break;
                purchasedGoods.Add(PlayerPrefs.GetInt("PurchasedGood" + (i + 1)));
            }
            purchasedGoods.Add(good.ID);
            for (int i = 0; i < purchasedGoods.Count; i++)
                PlayerPrefs.SetInt("PurchasedGood" + (i + 1), purchasedGoods[i]);
            PlayerPrefs.Save();
            _stats.Refresh();
            Refresh();
        }

        private void Refresh()
        {
            var good = _goods[_currentGood];

            _titleLabel.text = good.Title;
            _descriptionLabel.text = good.Description;
            _preview.sprite = good.Preview;

            var money = PlayerPrefs.GetInt("Money");
            var purchasedGoods = new List<int>();
            for (int i = 0; ; i++)
            {
                if (PlayerPrefs.HasKey("PurchasedGood" + (i + 1)) == false)
                    break;
                purchasedGoods.Add(PlayerPrefs.GetInt("PurchasedGood" + (i + 1)));
            }
            var equipedShip = PlayerPrefs.GetInt("EquipedShip");

            if (good.IsShip == true)
            {
                if (purchasedGoods.Contains(good.ID) == true)
                {
                    _buyButton.interactable = false;
                    _priceLabel.text = string.Empty;
                    if (equipedShip == good.ID)
                        _equipButton.interactable = false;
                    else
                        _equipButton.interactable = true;
                }
                else
                {
                    _priceLabel.text = good.Price.ToString();
                    _equipButton.interactable = false;
                    if (money >= good.Price)
                        _buyButton.interactable = true;
                    else
                        _buyButton.interactable = false;
                }
            }
        }

        [System.Serializable]
        public struct Good
        {
            public string Title;
            public string Description;
            public Sprite Preview;
            public int ID;
            public int Price;
            public bool IsShip;
        }
    }
}