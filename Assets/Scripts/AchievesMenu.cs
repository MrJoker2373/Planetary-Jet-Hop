namespace SpaceGame
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class AchievesMenu : Menu
    {
        [SerializeField] private StatsMenu _stats;
        [SerializeField] private Achieve[] _achieves;

        private void Start()
        {
            for (int i = 0; i < _achieves.Length; i++)
            {
                int index = i;
                _achieves[i].Button.onClick.AddListener(() => Take(_achieves[index].ID));
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
            yield return base.CloseAsync();
            Refresh();
            _stats.Refresh();
        }

        public void Take(int ID)
        {
            var completedAchieves = new List<int>();
            for (int i = 0; i < 10; i++)
            {
                if (PlayerPrefs.HasKey("CompletedAchieve" + (i + 1)) == true)
                    completedAchieves.Add(PlayerPrefs.GetInt("CompletedAchieve" + (i + 1)));
            }

            var takenAchieves = new List<int>();
            for (int i = 0; i < 10; i++)
            {
                if (PlayerPrefs.HasKey("TakenAchieve" + (i + 1)) == true)
                    takenAchieves.Add(PlayerPrefs.GetInt("TakenAchieve" + (i + 1)));
            }

            if (completedAchieves.Contains(ID) == true && takenAchieves.Contains(ID) == false)
            {
                PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") + _achieves[ID].Price);
                takenAchieves.Add(ID);
                for (int i = 0; i < takenAchieves.Count; i++)
                    PlayerPrefs.SetInt("TakenAchieve" + (i + 1), takenAchieves[i]);
                PlayerPrefs.Save();
            }

            Refresh();
            _stats.Refresh();
        }

        private void Refresh()
        {
            var completedAchieves = new List<int>();
            for (int i = 0; i < 10; i++)
            {
                if (PlayerPrefs.HasKey("CompletedAchieve" + (i + 1)) == true)
                    completedAchieves.Add(PlayerPrefs.GetInt("CompletedAchieve" + (i + 1)));
            }

            var takenAchieves = new List<int>();
            for (int i = 0; i < 10; i++)
            {
                if (PlayerPrefs.HasKey("TakenAchieve" + (i + 1)) == true)
                    takenAchieves.Add(PlayerPrefs.GetInt("TakenAchieve" + (i + 1)));
            }

            for (int i = 0; i < _achieves.Length; i++)
            {
                if (completedAchieves.Contains(_achieves[i].ID) == false || takenAchieves.Contains(_achieves[i].ID))
                    _achieves[i].Button.interactable = false;
                else
                    _achieves[i].Button.interactable = true;
            }
        }
    }
}