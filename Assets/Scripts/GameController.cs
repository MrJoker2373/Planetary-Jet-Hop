namespace SpaceGame
{
    using Dan.Main;
    using System.Collections;
    using UnityEngine;

    public class GameController : MonoBehaviour
    {
        [SerializeField] private GameplayMenu _menu;
        [SerializeField] private PlayerShip _ship;
        [SerializeField] private AudioSource _looseSound;
        [SerializeField] private ShipModel _shipModel;
        [SerializeField] private Ship[] _ships;
        [SerializeField] private Transform _pointMin;
        [SerializeField] private Transform _pointMax;
        [SerializeField] private Planet _startPlanet;
        [SerializeField] private Planet _planetPrefab;
        [SerializeField] private FuelTank _fuelTankPrefab;
        [SerializeField] private FirstAid _firstAidPrefab;
        [SerializeField] private Asteroid _asteroid;
        private Vector3[] _spawnPoints;
        private float _time;
        private int _lastPoint;
        private Coroutine _coroutine;
        private float _planetChance;
        private float _fuelChance;
        private bool _isEnable;
        private bool _isDisable;

        private void Start()
        {
            if (PlayerPrefs.HasKey("LoseCount") == false)
                PlayerPrefs.SetInt("LoseCount", 0);
            if (PlayerPrefs.HasKey("CompletedAchieve1") == false)
                PlayerPrefs.SetInt("CompletedAchieve1", 0);
            _lastPoint = 1;
            _planetChance = 1;
            InitializePlayer();
            _startPlanet.Initialize(6, 0, 1.75f);
            _menu.SetTime((int)_time);
            CreatePoints();
        }

        private void OnDisable()
        {
            CoroutineLauncher.Stop(_coroutine); 
        }

        private void Update()
        {
            if (_isEnable == true && _isDisable == false)
            {
                _time += Time.deltaTime;
                _menu.SetTime((int)_time);
            }
        }

        public void Enable()
        {
            _isEnable = true;
            _startPlanet.Enable();
            _coroutine = CoroutineLauncher.Launch(Spawn());
        }

        private void InitializePlayer()
        {
            int shipID = PlayerPrefs.GetInt("EquipedShip");
            var currentShip = _ships[shipID];
            _shipModel.Initialize(shipID);
            _ship.Initialize(currentShip.Health, currentShip.FuelDecreaseSpeed);
        }

        private void CreatePoints()
        {
            _spawnPoints = new Vector3[3];
            float lengthX = _pointMax.position.x - _pointMin.position.x;
            float offsetX = lengthX / (_spawnPoints.GetLength(0) + 1);
            float pointY = _pointMax.position.y;
            for (int i = 0; i < _spawnPoints.GetLength(0); i++)
                _spawnPoints[i] = new Vector3(_pointMin.position.x + offsetX * (i + 1), pointY, 0);
        }

        private IEnumerator Spawn()
        {
            var fuelChance = Random.Range(0.4f, 0.9f);
            if (_fuelChance > fuelChance)
            {
                _fuelChance = 0;
                SpawnFuelTank();
                yield return new WaitForSeconds(1.5f);
            }
            else
            {
                _fuelChance += 0.3f;
                var planetChance = Random.Range(0.1f, 0.9f);
                var item = Random.Range(0, 5);
                if (_planetChance > planetChance || item == 0 || item == 1)
                {
                    _planetChance = 0;
                    SpawnPlanet();
                    yield return new WaitForSeconds(4f);
                }
                else
                {
                    _planetChance += 0.5f;
                    if (item == 2 || item == 3)
                    {
                        SpawnAsteroid();
                        yield return new WaitForSeconds(1.5f);
                    }
                    else if (item == 4)
                    {
                        SpawnFirstAid();
                        yield return new WaitForSeconds(1.5f);
                    }
                }
            }
            _coroutine = CoroutineLauncher.Launch(Spawn());
        }

        public void Disable()
        {
            _looseSound.Play();
            PlayerPrefs.SetInt("LoseCount", PlayerPrefs.GetInt("LoseCount") + 1);
            if (PlayerPrefs.HasKey("CompletedAchieve4") == false)
                PlayerPrefs.SetInt("CompletedAchieve4", 3);

            if (PlayerPrefs.HasKey("CompletedAchieve5") == false && PlayerPrefs.GetInt("LoseCount") >= 10)
                PlayerPrefs.SetInt("CompletedAchieve5", 4);

            var bestScore = PlayerPrefs.GetFloat("BestScore");

            if (PlayerPrefs.HasKey("CompletedAchieve6") == false && _time > 60f)
                PlayerPrefs.SetInt("CompletedAchieve6", 5);
            if (PlayerPrefs.HasKey("CompletedAchieve7") == false && _time > 150f)
                PlayerPrefs.SetInt("CompletedAchieve7", 6);
            if (PlayerPrefs.HasKey("CompletedAchieve8") == false && _time > 300f)
                PlayerPrefs.SetInt("CompletedAchieve8", 7);
            _isDisable = true;
            int money = (int)(_time * 1.6f);
            if (bestScore < money)
            {
                PlayerPrefs.SetFloat("BestScore", money);
            }

            if (PlayerPrefs.HasKey("UserName") == true)
                Leaderboards.SpaceGame.UploadNewEntry(PlayerPrefs.GetString("UserName"), (int)PlayerPrefs.GetFloat("BestScore"), isSuccesful => { });

            PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") + money);
            PlayerPrefs.Save();
            _menu.Loose(money);
            CoroutineLauncher.Stop(_coroutine);
        }

        private void SpawnPlanet()
        {
            while (true)
            {
                int point = Random.Range(0, 3);
                if (_lastPoint != point)
                {
                    _lastPoint = point;
                    break;
                }
            }
            var planet = Instantiate(_planetPrefab, _spawnPoints[_lastPoint], Quaternion.identity);
            planet.transform.SetParent(transform);
            float size = Random.Range(6, 9);
            float rotation = 0;
            if (Random.Range(0, 2) == 0)
            {
                rotation = Random.Range(45f, 90f);
            }
            planet.Initialize(size, rotation, 3);
            planet.Enable();
        }

        private void SpawnFuelTank()
        {
            var tank = Instantiate(_fuelTankPrefab, _spawnPoints[_lastPoint], Quaternion.identity);
            tank.transform.SetParent(transform);
            tank.Initialize(3);
        }

        private void SpawnFirstAid()
        {
            var aid = Instantiate(_firstAidPrefab, _spawnPoints[_lastPoint], Quaternion.identity);
            aid.transform.SetParent(transform);
            aid.Initialize(3);
        }

        private void SpawnAsteroid()
        {
            var asteroid = Instantiate(_asteroid, _spawnPoints[_lastPoint], Quaternion.identity);
            asteroid.transform.SetParent(transform);
            var direction = (_ship.transform.position - asteroid.transform.position).normalized;
            asteroid.Initialize(direction, 40);
        }

        [System.Serializable]
        public struct Ship
        {
            public int Health;
            public float FuelDecreaseSpeed;
        }
    }
}