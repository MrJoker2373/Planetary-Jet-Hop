namespace SpaceGame
{
    using System;
    using System.Collections;
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;

    public class RewardMenu : Menu
    {
        [SerializeField] private StatsMenu _stats;
        [SerializeField] private TextMeshProUGUI _moneyLabel;
        [SerializeField] private Button _takeButton;
        [SerializeField] private float _coinsPerSecond;

        private void Start()
        {
            if (PlayerPrefs.HasKey("ExitTime") == false)
            {
                PlayerPrefs.SetString("ExitTime", DateTime.Now.ToString());
                PlayerPrefs.Save();
            }
        }

        public override void Open()
        {
            base.Open();
            Refresh();
        }

        public override IEnumerator OpenAsync()
        {
            yield return base.OpenAsync();
            Refresh();
        }

        public void Take()
        {
            if (PlayerPrefs.HasKey("CompletedAchieve2") == false)
                PlayerPrefs.SetInt("CompletedAchieve2", 1);

            DateTime exitTime = DateTime.Parse(PlayerPrefs.GetString("ExitTime"));
            var money = Mathf.FloorToInt((float)(DateTime.Now - exitTime).TotalSeconds * _coinsPerSecond);

            PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") + money);
            PlayerPrefs.SetString("ExitTime", DateTime.Now.ToString());
            PlayerPrefs.Save();

            Refresh();
        }

        private void Refresh()
        {
            if (PlayerPrefs.GetString("ExitTime") != string.Empty)
            {
                DateTime exitTime = DateTime.Parse(PlayerPrefs.GetString("ExitTime"));
                var money = Mathf.FloorToInt((float)(DateTime.Now - exitTime).TotalSeconds * _coinsPerSecond);

                if (money == 0)
                {
                    _takeButton.interactable = false;
                    _moneyLabel.text = "0";
                }
                else
                {
                    _takeButton.interactable = true;
                    _moneyLabel.text = money.ToString();
                }

                _stats.Refresh();
            }
        }
    }
}