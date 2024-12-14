namespace SpaceGame
{
    using TMPro;
    using UnityEngine;

    public class LeaderboardPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private TextMeshProUGUI _score;

        public void Initialize(string name, float score)
        {
            _name.text = name;
            _score.text = score.ToString();
        }
    }
}