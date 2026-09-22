using System;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance { get; private set; }
    
    [Header("References")]
    [SerializeField] private Text _healthText;
    [SerializeField] private Text _scoreText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void UpdateHealthText(int health)
    {
        if (_healthText != null)
        {
            _healthText.text = $"HP: {health}";
        }
    }

    public void UpdateScoreText(int score)
    {
        if (_scoreText != null)
        {
            _scoreText.text = $"Score: {score}";
        }
    }
}
