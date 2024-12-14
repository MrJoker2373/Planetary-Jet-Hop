namespace SpaceGame
{
    using UnityEngine;

    public class ShipModel : MonoBehaviour
    {
        [SerializeField] private GameObject _default;
        [SerializeField] private GameObject _effective;
        [SerializeField] private GameObject _protective;

        public void Initialize(int ID)
        {
            if (ID == 0)
                _default.SetActive(true);
            else if (ID == 1)
                _effective.SetActive(true);
            else if (ID == 2)
                _protective.SetActive(true);
        }
    }
}