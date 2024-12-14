namespace SpaceGame
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Dan.Main;
    using TMPro;

    public class LeaderboardMenu : Menu
    {
        [SerializeField] private LeaderboardPanel _panelPrefab;
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private Transform _parent;
        private List<LeaderboardPanel> _panels;

        private void Start()
        {
            _panels = new();
            if (PlayerPrefs.HasKey("UserName") == false)
                PlayerPrefs.SetString("UserName", "Guest");
        }

        public override void Open()
        {
            base.Open();
            Refresh();
        }

        public void SetUserName()
        {
            if (string.IsNullOrEmpty(_inputField.text) == false)
            {
                PlayerPrefs.SetString("UserName", _inputField.text);
                Leaderboards.SpaceGame.UploadNewEntry(PlayerPrefs.GetString("UserName"), (int)PlayerPrefs.GetFloat("BestScore"), isSuccesful => { });
                Refresh();
            }
        }

        public override IEnumerator OpenAsync()
        {
            yield return base.OpenAsync();
            Refresh();
        }

        private void Refresh()
        {
            _inputField.text = PlayerPrefs.GetString("UserName");
            for (int i = 0; i < _panels.Count; i++)
            {
                Destroy(_panels[i].gameObject);
                _panels.RemoveAt(i);
            }
            Leaderboards.SpaceGame.GetEntries(entries =>
            {
                for (int i = 0; i < entries.Length; i++)
                {
                    var panel = Instantiate(_panelPrefab, _parent);
                    panel.Initialize(entries[i].Username, entries[i].Score);
                    _panels.Add(panel);
                }
            }, error =>
            {

            });
        }
    }
}