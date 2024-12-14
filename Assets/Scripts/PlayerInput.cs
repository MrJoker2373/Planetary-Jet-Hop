namespace SpaceGame
{
    using System;
    using System.Collections;
    using UnityEngine;
    using UnityEngine.InputSystem;

    public class PlayerInput : MonoBehaviour
    {
        [SerializeField] private InputAction _touchPosition;
        [SerializeField] private InputAction _touchPress;
        private Vector2 _lastPosition;
        private Vector2 _startPosition;
        public event Func<Vector3, IEnumerator> OnPush;

        private void Start()
        {
            Enable();
        }

        public void Enable()
        {
            _touchPosition.Enable();
            _touchPress.Enable();
            _touchPosition.performed += OnTouchPosition;
            _touchPress.performed += OnTouchPress;
        }

        public void Disable()
        {
            _touchPosition.Disable();
            _touchPress.Disable();
            _touchPosition.performed -= OnTouchPosition;
            _touchPress.performed -= OnTouchPress;
        }

        private void OnTouchPosition(InputAction.CallbackContext context)
        {
            var size = new Vector2(Screen.width, Screen.height);
            _lastPosition = context.ReadValue<Vector2>() / size;
        }

        private void OnTouchPress(InputAction.CallbackContext context)
        {
            var input = context.ReadValue<float>();
            if (input == 1)
                _startPosition = _lastPosition;
            else if (input == 0)
            {
                var direction = _lastPosition - _startPosition;
                direction.x /= 1.025f;
                direction.y *= 3.5f;
                var result = Vector3.ClampMagnitude(direction, 1.4f);
                if (OnPush != null)
                    CoroutineLauncher.Launch(OnPush.Invoke(result));
            }
        }
    }
}