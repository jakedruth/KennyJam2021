using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour
{
    public HealthMeter healthMeter;
    public TMPro.TMP_Text scoreText;
    private int score;

    private PlayerShipController _player;

    public void Start()
    {
        _player = FindObjectOfType<PlayerShipController>();
        SetCurrentHealth(_player.maxHP);
        _player.onShipHitEvent.AddListener(OnShipHit);
        SetScoreText(score);
    }

    private void OnShipHit(object ship, int damgage)
    {
        if (ship != null && ship is PlayerShipController == _player)
        {
            SetCurrentHealth(_player.currentHP);
        }
    }

    public void SetCurrentHealth(int health)
    {
        healthMeter.SetCurrentHealth(health);
    }

    public void AddScore(int value)
    {
        score += value;
        SetScoreText(score);
    }

    public void SetScoreText(int score)
    {
        scoreText.text = $"Score: {score}";
    }
}
