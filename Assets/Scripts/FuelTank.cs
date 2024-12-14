namespace SpaceGame
{
    using UnityEngine;

    public class FuelTank : MonoBehaviour
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
                _mesh.enabled = false;
                _collider.enabled = false;
                ship.RefillFuel();
                _audio.Play();
                Destroy(gameObject, 5f);
            }
        }
    }
}