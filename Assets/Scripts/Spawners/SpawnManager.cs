using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public WaveDataScriptableObject[] spawnWaveData;
    public int[] _spawnWeights;
    public string initialWeights;

    public float spawnInterval;
    private float _spawnTimer;

    private void Awake()
    {
        _spawnWeights = new int[spawnWaveData.Length];
        AddWeight(initialWeights);
    }

    public bool AddWeight(string weights)
    {
        string[] splitWeights = weights.Split(' ');
        foreach (string w in splitWeights)
        {
            try
            {
                string[] split = w.Split(':');
                int id = int.Parse(split[0]);
                int weight = int.Parse(split[1]);

                if (id >= 0 && id < spawnWaveData.Length)
                {
                    _spawnWeights[id] += weight;
                    return true;
                }
                else
                    throw new System.Exception("Invalid difficulty ID");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"{e.Message}\nInvalid weight: [{weights}] => {w}");
            }
        }

        return false;
    }

    private int GetRandomWaveIndex()
    {
        // Calculate the sum of all weights
        int sumOfWeights = 0;
        for (int i = 0; i < _spawnWeights.Length; i++)
        {
            int weight = _spawnWeights[i];
            if (weight > 0)
                sumOfWeights += weight;
        }

        if (sumOfWeights <= 0)
        {
            Debug.LogError($"No spawn weights defined, Sum of wieghts equals {sumOfWeights}");
            return 0;
        }

        // get a random number between 0 and the sum of all weights
        int randomNum = Random.Range(0, sumOfWeights);
        // find the index of the first weight that is greater than the random number
        int count = 0;
        for (int i = 0; i < _spawnWeights.Length; i++)
        {
            count += _spawnWeights[i];
            if (count > randomNum)
            {
                return i;
            }
        }

        // fail safe
        Debug.LogWarning($"Unable to find a Spawn Wave Index with random number [{randomNum}]");
        return 0;
    }

    public void SpawnWave(int index)
    {
        // Get the wave data
        WaveDataScriptableObject waveData = spawnWaveData[index];

        // calculate the spawn position relative to the player.turnpoint
        Vector3 pos = PlayerShipController.Instance.turnPoint;

        // spawn the wave
        waveData.SpawnWave(pos, this);
    }

    void Update()
    {
        // spawn an asteroid every spawn interval
        if (_spawnTimer >= spawnInterval)
        {
            // get a random wave index
            int index = GetRandomWaveIndex();

            // spawn an asteroid
            SpawnWave(index);

            // reset the timer
            _spawnTimer -= spawnInterval;
            spawnInterval *= 0.9999f;
        }

        // increment the timer
        _spawnTimer += Time.deltaTime;
    }
}
