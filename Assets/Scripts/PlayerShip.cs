namespace SpaceGame
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.InputSystem.XR;

    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerShip : MonoBehaviour
    {
        [SerializeField] private AudioSource _explosionAudio;
        [SerializeField] private AudioSource _dashAudio;
        [SerializeField] private GameController _controller;
        [SerializeField] private GameplayMenu _menu;
        [SerializeField] private ParticleSystem _explosion;
        [SerializeField] private GameObject _model;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private float _pushForce;
        [SerializeField] private float _fuelDecreaseSpeed;
        [SerializeField] private int _maxHealth;
        [SerializeField] private float _fuel;
        private Transform _pivot;
        private PlayerInput _input;
        private Collider _collider;
        private Rigidbody _rigidbody;
        private int _health;
        private bool _isRotate;
        private bool _isPlant;
        private bool _isPivot;
        private bool _isLoose;
        private bool _isEnabled;

        private void Awake()
        {
            _input = GetComponent<PlayerInput>();
            _collider = GetComponent<Collider>();
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            _health = _maxHealth;
            _menu.SetHealth(_health / (float)_maxHealth);
            _menu.SetFuel(_fuel);
        }

        private void OnEnable()
        {
            _input.OnPush += Push;
        }

        private void OnDisable()
        {
            _input.OnPush -= Push;
        }

        private void Update()
        {
            if (_isEnabled == false)
                return;
            if (_pivot != null && _isPivot == true)
            {
                transform.position = _pivot.position;
                transform.up = _pivot.up;
            }
            if (_isLoose == false)
            {
                if (_fuel <= 0)
                {
                    _isLoose = true;
                    _input.OnPush -= Push;
                    _controller.Disable();
                }
                else
                {
                    _fuel = Mathf.Clamp(_fuel - _fuelDecreaseSpeed * Time.deltaTime, 0, 1);
                    _menu.SetFuel(_fuel);
                }
            }
        }

        private void FixedUpdate()
        {
            if (_isRotate == true && _isLoose == false)
                _rigidbody.rotation = Quaternion.LookRotation(_rigidbody.velocity) * Quaternion.Euler(90f, 0, 0);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (_isPlant == false)
                _isRotate = false;
        }

        public void Initialize(int health, float fuelDecreaseSpeed)
        {
            _maxHealth = _health = health;
            _fuelDecreaseSpeed = fuelDecreaseSpeed;
        }

        public void Damage()
        {
            if (_health > 0)
            {
                _health--;
                _menu.SetHealth(_health / (float)_maxHealth);
                if (_health == 0)
                    CoroutineLauncher.Launch(Destroy());
            }
        }

        public bool IsHealthFull() => _health == _maxHealth;

        public void AddHealth()
        {
            if (_health < 3)
            {
                _health++;
                _menu.SetHealth(_health / (float)_maxHealth);
            }
        }

        public void RefillFuel()
        {
            _fuel = 1;
            _menu.SetFuel(_fuel);
        }

        public void Plant(Transform pivot)
        {
            if (_isPlant == false)
            {
                _isPlant = true;
                _isPivot = true;
                _rigidbody.velocity = Vector3.zero;
                _rigidbody.angularVelocity = Vector3.zero;
                _rigidbody.isKinematic = true;
                _pivot = pivot;
            }
        }

        public IEnumerator Push(Vector3 direction)
        {
            if (_isPlant == true && _isRotate == false)
            {
                var dot = Vector3.Dot(transform.up, direction);
                if (dot > -0.1f)
                {
                    if (_isEnabled == false)
                    {
                        _isEnabled = true;
                        _controller.Enable();
                        yield break;
                    }
                    _dashAudio.Play();
                    _isRotate = true;
                    _isPivot = false;
                    _rigidbody.isKinematic = false;
                    _rigidbody.velocity += direction * _pushForce;
                    yield return new WaitForSeconds(0.1f);
                    _isPlant = false;
                }
            }
        }

        public IEnumerator Destroy()
        {
            if (_isLoose == true)
                yield break;
            _explosionAudio.Play();
            _isLoose = true;
            _explosion.Play();
            _controller.Disable();
            _input.OnPush -= Push;
            _collider.enabled = false;
            _rigidbody.isKinematic = true;
            _model.SetActive(false);
            while (_explosion != null && _explosion.IsAlive())
            {
                yield return null;
            }
            if (_explosion != null)
                Destroy(gameObject);
        }
    }
}