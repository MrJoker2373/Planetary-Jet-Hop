namespace SpaceGame
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class SceneLoader : MonoBehaviour
    {
        private static SceneLoader _instance;
        private string _currentScene;
        private bool _isLoading;

        public static bool IsLoading => _instance._isLoading;

        private void Awake()
        {
            if (_instance == null)
                _instance = this;
        }

        public static IEnumerator Load(string scene)
        {
            if (_instance._isLoading == true)
                yield break;
            _instance._isLoading = true;
            PauseManager.Pause();
            if (string.IsNullOrEmpty(_instance._currentScene) == false)
            {
                var unload = SceneManager.UnloadSceneAsync(_instance._currentScene);
                while (unload.isDone == false)
                    yield return null;
            }
            var load = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
            while (load.isDone == false)
                yield return null;
            _instance._currentScene = scene;
            PauseManager.Resume();
            _instance._isLoading = false;
        }
    }

}