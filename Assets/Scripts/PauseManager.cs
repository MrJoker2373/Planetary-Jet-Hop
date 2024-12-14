namespace SpaceGame
{
    using UnityEngine;

    public class PauseManager : MonoBehaviour
    {
        private static PauseManager _instance;

        private void Awake()
        {
            if (_instance == null)
                _instance = this;
        }

        public static void Pause()
        {
            Time.timeScale = 0;
        }

        public static void Resume()
        {
            Time.timeScale = 1;
        }
    }
}