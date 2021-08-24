using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour
{
    public static HUD instance;
    public HealthMeter healthMeter;
    public TMPro.TMP_Text scoreText;
    private int score;

    private Actor _player;

    void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        _player = FindObjectOfType<PlayerShipController>().actor;
        SetCurrentHealth(_player.maxHP);
        _player.onActorHit.AddListener(OnShipHit);
        SetScoreText(score);
    }

    private void OnShipHit(Actor ship, object arg)
    {
        if (ship != null && ship == _player)
        {
            SetCurrentHealth(ship.currentHP);
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
