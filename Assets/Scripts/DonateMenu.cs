namespace SpaceGame
{
    using UnityEngine;
    using UnityEngine.Purchasing;

    public class DonateMenu : Menu
    {
        [SerializeField] private StatsMenu _stats;
        [SerializeField] private Donate[] _donates;

        public void OnPurchaseCompleted(Product product)
        {
            foreach (var donate in _donates)
            {
                if (product.definition.id == donate.ID)
                {
                    PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") + donate.Price);
                    break;
                }
            }
            _stats.Refresh();
        }

        [System.Serializable]
        public struct Donate
        {
            public string ID;
            public int Price;
        }
    }
}