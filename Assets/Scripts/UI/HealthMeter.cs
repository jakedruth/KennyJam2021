using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthMeter : MonoBehaviour
{
    public Color hpFilledColor;
    public Color hpEmptyColor;
    private UnityEngine.UI.Image[] hpSquares;

    public int maxHP;
    public int currentHP;

    // Start is called before the first frame update
    void Start()
    {
        hpSquares = new UnityEngine.UI.Image[maxHP];
        for (int i = 0; i < maxHP; i++)
        {
            hpSquares[i] = transform.GetChild(i).GetComponent<UnityEngine.UI.Image>();
        }

        SetCurrentHealth(maxHP);
    }

    public void SetCurrentHealth(int health)
    {
        currentHP = health;
        for (int i = 0; i < hpSquares.Length; i++)
        {
            hpSquares[i].color = i < currentHP ? hpFilledColor : hpEmptyColor;
        }
    }
}
