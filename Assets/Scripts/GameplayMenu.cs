namespace SpaceGame
{
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;

    public class GameplayMenu : MonoBehaviour
    {
        [SerializeField] private Menu _interface;
        [SerializeField] private Menu _pause;
        [SerializeField] private Menu _loose;
        [SerializeField] private Image _healthImage;
        [SerializeField] private Image _fuelImage;
        [SerializeField] private TextMeshProUGUI _moneyLabel;
        [SerializeField] private TextMeshProUGUI _timeLabel;
        private Menu _currentMenu;

        private void Start()
        {
            _currentMenu = _interface;
            OpenMenu(_interface);
        }

        public void Pause()
        {
            OpenMenu(_pause);
            PauseManager.Pause();
        }

        public void Resume()
        {
            OpenMenu(_interface);
            PauseManager.Resume();
        }

        public void Loose(int money)
        {
            OpenMenu(_loose);
            _moneyLabel.text = $"You got {money} coins!";
        }

        public void Retry()
        {
            if (SceneLoader.IsLoading == false)
                CoroutineLauncher.Launch(SceneLoader.Load("Gameplay"));
        }

        public void Main()
        {
            if (SceneLoader.IsLoading == false)
                CoroutineLauncher.Launch(SceneLoader.Load("Main"));
        }

        public void SetTime(int time)
        {
            _timeLabel.text = $"Time: {time}";
        }

        public void SetHealth(float amount)
        {
            _healthImage.fillAmount = amount;
        }

        public void SetFuel(float amount)
        {
            _fuelImage.fillAmount = amount;
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