namespace SpaceGame
{
    using UnityEngine;

    public class FirstAid : MonoBehaviour
    {
        [SerializeField] private AudioSource _audio;
        [SerializeField] private MeshRenderer _mesh;
        [SerializeField] private Collider _collider;
        private float _fallSpeed;

        public void Initialize(float fallSpeed)
        {
            _fallSpeed = fallSpeed;
        }

        private void Update()
        {
            transform.Translate(Vector3.down * _fallSpeed * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<PlayerShip>(out var ship))
            {
                if (ship.IsHealthFull() == true)
                    return;
                _mesh.enabled = false;
                _collider.enabled = false;
                ship.AddHealth();
                _audio.Play();
                Destroy(gameObject, 5f);
            }
        }
    }
}