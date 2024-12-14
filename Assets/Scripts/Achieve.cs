namespace SpaceGame
{
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class Achieve : MonoBehaviour
    {
        public int ID;
        public int Price;
        public Button Button;
        public TextMeshProUGUI RewardLabel;

        private void Start()
        {
            RewardLabel.text = Price.ToString();
        }
    }
}