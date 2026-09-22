using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Settings")]
    [SerializeField] private float _spawnCooldown = 1.5f;
    [Header("References")] 
    [SerializeField] private PlayerController _player;
    [SerializeField] private GameObject _enemyPrefab;
    
    private int _score;
    private float _time;
    private bool _paused;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        _time = _spawnCooldown;
        HUDManager.Instance.UpdateHealthText(_player.Health);
        HUDManager.Instance.UpdateScoreText(_score);
    }

    void Update()
    {
        if (_player == null)
        {
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _paused = !_paused;
            Time.timeScale = _paused ? 0f : 1f;
        }

        _time -= Time.deltaTime;
        if (_time <= 0f)
        {
            _time = _spawnCooldown;
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        Vector3 position = new Vector3(Random.Range(-7f, 7f), Random.Range(-4f, 4f), 0f);
        if (Vector3.Distance(position, _player.transform.position) < 1.5f)
        {
            position += new Vector3(3f, 3f, 0f);
        }
        Instantiate(_enemyPrefab, position, Quaternion.identity);
    }

    public void AddScore(int score)
    {
        _score += score;
        HUDManager.Instance.UpdateScoreText(_score);
    }

    public void DamagePlayer(int damage)
    {
        _player.Health -= damage;

        if (_player.Health <= 0)
        {
            _player.Health = 0;
            HUDManager.Instance.UpdateHealthText(_player.Health);
            Destroy(_player.gameObject);
            return;
        }
        
        HUDManager.Instance.UpdateHealthText(_player.Health);
    }
}
