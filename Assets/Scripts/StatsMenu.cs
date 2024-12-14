namespace SpaceGame
{
    using UnityEngine;
    using TMPro;

    public class StatsMenu : Menu
    {
        [SerializeField] private TextMeshProUGUI _moneyLabel;    

        public void Refresh()
        {
            _moneyLabel.text = PlayerPrefs.GetInt("Money").ToString();
        }
    }
}