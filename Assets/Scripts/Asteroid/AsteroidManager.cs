using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidManager : MonoBehaviour
{
    public PlayerShipController player;
    public AsteroidDifficulty[] asteroidDifficulties;
    public List<int> asteroidDifficultyWeights;

    public float spawnInterval;
    private float _spawnTimer;

    private void Awake()
    {
        asteroidDifficultyWeights = new List<int>();
        int initialWeight = 10;
        for (int i = 0; i < initialWeight; i++)
        {
            AddWeight(0);
        }
    }

    private int GetRandomDifficultyIndex()
    {
        // get a weighted index from the weights array
        int weightedIndex = Random.Range(0, asteroidDifficultyWeights.Count);
        int index = asteroidDifficultyWeights[weightedIndex];
        return index;
    }

    private void AddWeight(int weight)
    {
        // check if the weight is valid
        if (weight >= 0 && weight < asteroidDifficulties.Length)
        {
            // add the weight to the end of the list
            asteroidDifficultyWeights.Add(weight);
        }
        else
        {
            Debug.LogError("Invalid weight: " + weight);
        }
    }

    private void SpawnAsteroid(int index)
    {
        // get the asteroid difficulty
        AsteroidDifficulty difficulty = asteroidDifficulties[index];

        // calculate the spawn position relative to the player.turnPoint
        Vector3 pos = player.turnPoint;

        for (int i = 0; i < difficulty.spawnCount; i++)
        {
            Vector3 spawnPos = pos + Quaternion.AngleAxis(Random.Range(0, 360), Vector3.forward) * Vector3.right * 15;

            // calculate the spawn velocity
            Vector3 displacement = pos - spawnPos;
            Vector3 direction = displacement.normalized;
            Vector3 velocity = direction * difficulty.speedRange.GetRandomValue;

            // create the asteroid prefab
            Asteroid asteroid = Instantiate(difficulty.astroidPrefab, spawnPos, transform.rotation);
            asteroid.SetVelocity(velocity);
            asteroid.onAstroidDestroyed.AddListener((arg) =>
            {
                int nextWeight = Mathf.Min(index + 1, asteroidDifficulties.Length - 1);
                AddWeight(nextWeight);
            });
        }
    }

    void Update()
    {
        // spawn an asteroid every spawn interval
        if (_spawnTimer >= spawnInterval)
        {
            // get a random difficulty
            int index = GetRandomDifficultyIndex();

            // spawn an asteroid
            SpawnAsteroid(index);

            // reset the timer
            _spawnTimer -= spawnInterval;
            spawnInterval *= 0.9999f;
        }

        // increment the timer
        _spawnTimer += Time.deltaTime;
    }
}
