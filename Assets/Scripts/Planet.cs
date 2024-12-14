namespace SpaceGame
{
    using UnityEngine;

    [RequireComponent(typeof(SphereCollider))]
    public class Planet : MonoBehaviour
    {
        [SerializeField] GameObject[] _models;
        [SerializeField] private Transform _pivot;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private float _fallSpeed;
        private SphereCollider _collider;
        private bool _isEnabled;

        private void Awake()
        {
            _collider = GetComponent<SphereCollider>();
        }

        private void Update()
        {
            if (_isEnabled == false)
                return;
            transform.position += Vector3.down * _fallSpeed * Time.deltaTime;
            transform.Rotate(Vector3.forward * _rotationSpeed * Time.deltaTime);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent<PlayerShip>(out var ship))
            {
                var dot = Vector3.Dot(collision.GetContact(0).normal, Vector3.down);
                if (dot > 0.3f)
                {
                    _pivot.up = -collision.GetContact(0).normal;
                    _pivot.position = transform.position + -collision.GetContact(0).normal * transform.localScale.x / 2;
                    ship.Plant(_pivot);
                }
            }
        }

        public void Initialize(float size, float rotationSpeed, float fallSpeed)
        {
            int index = Random.Range(0, _models.Length);
            _models[index].SetActive(true);
            transform.localScale = Vector3.one * size;
            _rotationSpeed = rotationSpeed;
            _fallSpeed = fallSpeed;
        }

        public void Enable()
        {
            _isEnabled = true;
        }
    }
}