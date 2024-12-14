namespace SpaceGame
{
    using System.Collections;
    using UnityEngine;

    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(MeshRenderer))]
    public class Asteroid : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _explosion;
        [SerializeField] private AudioSource _audio;
        private Collider _collider;
        private Rigidbody _rigidbody;
        private MeshRenderer _renderer;
        private Vector3 _direction;
        private float _speed;
        private bool _isDead;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
            _rigidbody = GetComponent<Rigidbody>();
            _renderer = GetComponent<MeshRenderer>();
        }

        private void FixedUpdate()
        {
            if (_isDead == false)
                _rigidbody.velocity += _direction * _speed * Time.deltaTime;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (_isDead == false)
            {
                if (collision.gameObject.TryGetComponent<PlayerShip>(out var ship))
                    ship.Damage();
                CoroutineLauncher.Launch(Destroy());
            }
        }

        public void Initialize(Vector3 direction, float speed)
        {
            _direction = direction;
            _speed = speed;
        }

        private IEnumerator Destroy()
        {
            _isDead = true;
            _explosion.Play();
            _audio.Play();
            _collider.enabled = false;
            _renderer.enabled = false;
            _rigidbody.isKinematic = true;
            while (_explosion != null && _explosion.IsAlive())
                yield return null;
            if (_explosion != null)
                Destroy(gameObject);
        }
    }
}