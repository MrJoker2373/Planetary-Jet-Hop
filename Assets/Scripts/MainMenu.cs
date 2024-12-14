namespace SpaceGame
{
    using TMPro;
    using UnityEngine;

    public class MainMenu : Menu
    {
        [SerializeField] private StatsMenu _stats;
        [SerializeField] private SettingsMenu _settings;
        [SerializeField] private ShopMenu _shop;
        [SerializeField] private AchievesMenu _achieves;
        [SerializeField] private RewardMenu _reward;
        [SerializeField] private LeaderboardMenu _leaderboard;
        [SerializeField] private DonateMenu _donate;
        [SerializeField] private TextMeshProUGUI _resultLabel;
        private Menu _currentMenu;

        private void Start()
        {
            if (PlayerPrefs.HasKey("BestScore") == false)
            {
                PlayerPrefs.SetFloat("BestScore", 0f);
                PlayerPrefs.Save();
            }
            _currentMenu = this;
            OpenMain();
        }

        public void OpenMain()
        {
            OpenMenu(this);
            _stats.Close();
            _resultLabel.text = $"Best score: {(int)PlayerPrefs.GetFloat("BestScore")}";
        }

        public void OpenSettings()
        {
            OpenMenu(_settings);
            _stats.Open();
        }

        public void OpenShop()
        {
            OpenMenu(_shop);
            _stats.Open();
        }

        public void OpenAchieves()
        {
            OpenMenu(_achieves);
            _stats.Open();
        }

        public void OpenReward()
        {
            OpenMenu(_reward);
            _stats.Open();
        }

        public void OpenLeaderboard()
        {
            OpenMenu(_leaderboard);
            _stats.Open();
        }

        public void OpenDonate()
        {
            OpenMenu(_donate);
            _stats.Open();
        }

        public void Play()
        {
            if (SceneLoader.IsLoading == false)
                CoroutineLauncher.Launch(SceneLoader.Load("Gameplay"));
        }

        private void OpenMenu(Menu menu)
        {
            if (_currentMenu != null)
                _currentMenu.Close();
            _currentMenu = menu;
            _currentMenu.Open();
        }
    }
}