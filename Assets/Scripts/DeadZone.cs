namespace SpaceGame
{
    using UnityEngine;

    public class DeadZone : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<PlayerShip>(out var ship))
            {
                CoroutineLauncher.Launch(ship.Destroy());
            }
            else if (other.GetComponent<Planet>())
            {
                Destroy(other.gameObject, 5);
            }
        }
    }
}