using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidManager : MonoBehaviour
{
    public AsteroidDifficulty[] asteroidDifficulties;
    public int[] spawnWeights;
    public string initialWeights;

    public float spawnInterval;
    private float _spawnTimer;

    private void Awake()
    {
        spawnWeights = new int[asteroidDifficulties.Length];
        AddWeight(initialWeights);
    }

    public void AddWeight(string weights)
    {
        string[] splitWeights = weights.Split(' ');
        foreach (string w in splitWeights)
        {
            try
            {
                string[] split = w.Split(':');
                int id = int.Parse(split[0]);
                int weight = int.Parse(split[1]);

                if (id >= 0 && id < asteroidDifficulties.Length)
                    spawnWeights[id] += weight;
                else
                    throw new System.Exception("Invalid difficulty ID");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"{e.Message}\nInvalid weight: [{weights}] => {w}");
            }
        }
    }

    private int GetRandomDifficultyIndex()
    {
        // Calculate the sum of all weights
        int sumOfWeights = 0;
        for (int i = 0; i < spawnWeights.Length; i++)
        {
            int weight = spawnWeights[i];
            if (weight > 0)
                sumOfWeights += weight;
        }

        if (sumOfWeights <= 0)
        {
            Debug.LogError($"No spawn weights defined, sum of wieghts equals {sumOfWeights}");
            return 0;
        }

        // get a random number between 0 and the sum of all weights
        int randomNum = Random.Range(0, sumOfWeights);
        // find the index of the first weight that is greater than the random number
        int count = 0;
        for (int i = 0; i < spawnWeights.Length; i++)
        {
            count += spawnWeights[i];
            if (count > randomNum)
            {
                return i;
            }
        }

        // fail safe
        Debug.LogWarning($"Unable to find a random difficulty with random number [{randomNum}]");
        return 0;
    }

    private void SpawnAsteroid(int index)
    {
        // get the asteroid difficulty
        AsteroidDifficulty difficulty = asteroidDifficulties[index];

        // calculate the spawn position relative to the player.turnPoint
        Vector3 pos = PlayerShipController.Instance.turnPoint;

        for (int i = 0; i < difficulty.spawnCount; i++)
        {
            const float RADIUS = 20;
            Vector3 spawnPos = pos + Quaternion.AngleAxis(Random.Range(0, 360), Vector3.forward) * Vector3.right * RADIUS;

            // calculate the spawn velocity
            Vector3 displacement = pos - spawnPos;
            Vector3 direction = displacement.normalized;
            Vector3 velocity = direction * difficulty.speedRange.GetRandomValue();

            // create the asteroid prefab
            Asteroid asteroid = Instantiate(difficulty.astroidPrefab, spawnPos, transform.rotation);
            asteroid.SetVelocity(velocity);
            asteroid.onAstroidDestroyed.AddListener((arg) =>
            {
                int nextID = Mathf.Min(index + 1, asteroidDifficulties.Length - 1);
                int weight = 1;
                AddWeight($"{nextID}:{weight}");
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
