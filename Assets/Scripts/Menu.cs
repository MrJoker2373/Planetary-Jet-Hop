namespace SpaceGame
{
    using System.Collections;
    using UnityEngine;

    [RequireComponent(typeof(CanvasGroup))]
    public class Menu : MonoBehaviour
    {
        [SerializeField] private float _speed;
        private CanvasGroup _group;
        private bool _isOpen;

        public bool IsOpen => _isOpen;

        private void Awake()
        {
            _group = GetComponent<CanvasGroup>();
        }

        public virtual void Open()
        {
            _isOpen = true;
            _group.alpha = 1f;
            _group.blocksRaycasts = true;
            _group.interactable = true;
        }

        public virtual void Close()
        {
            _isOpen = false;
            _group.alpha = 0f;
            _group.blocksRaycasts = false;
            _group.interactable = false;
        }

        public virtual IEnumerator OpenAsync()
        {
            _group.alpha = 0f;
            while (_group.alpha < 1f)
            {
                _group.alpha = Mathf.MoveTowards(_group.alpha, 1f, _speed);
                yield return null;
            }
            _group.blocksRaycasts = true;
            _group.interactable = true;
            _isOpen = true;
        }

        public virtual IEnumerator CloseAsync()
        {
            _isOpen = false;
            _group.alpha = 1f;
            _group.blocksRaycasts = false;
            _group.interactable = false;
            while (_group.alpha > 0f)
            {
                _group.alpha = Mathf.MoveTowards(_group.alpha, 0f, _speed);
                yield return null;
            }
        }
    }
}