namespace SpaceGame
{
    using System.Collections;
    using UnityEngine;

    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private Menu _loading;
        [SerializeField] private bool _loadOnAwake;

        private IEnumerator Start()
        {
            if(_loadOnAwake == false)
                yield break;
            _loading.Open();
            yield return CoroutineLauncher.Launch(SceneLoader.Load("Main"));
            _loading.Close();
        }
    }
}