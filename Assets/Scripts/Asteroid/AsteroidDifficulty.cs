using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AstroidDifficulty", menuName = "KennyJam2021/AstroidDifficulty", order = 0)]
public class AsteroidDifficulty : ScriptableObject
{
    public Asteroid astroidPrefab;
    public int spawnCount = 1;
    public RangedFloat speedRange = new RangedFloat(0.1f, 1f);
    public float spread = 35f;
}
