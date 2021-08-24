using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpawnManager : MonoBehaviour
{
    public WaveDataScriptableObject[] spawnWaveData;
    public Dictionary<WaveDataScriptableObject, int> _spawnWeights = new Dictionary<WaveDataScriptableObject, int>();
    public string initialWeights;

    public float spawnInterval;
    private float _spawnTimer;

    private void Awake()
    {
        foreach (WaveDataScriptableObject waveData in spawnWaveData)
        {
            _spawnWeights.Add(waveData, 0);
        }

        AddWeight(initialWeights);
    }

    public void AddWeight(params WaveWeightPair[] pairs)
    {
        foreach (WaveWeightPair pair in pairs)
        {
            AddWeight(pair.waveData, pair.weight);
        }
    }

    public void AddWeight(WaveDataScriptableObject waveData, int weight)
    {
        if (_spawnWeights.ContainsKey(waveData))
        {
            _spawnWeights[waveData] += weight;
        }
        else
        {
            Debug.Log($"Added Wave Data Key to weights [{waveData.name}]");
            _spawnWeights.Add(waveData, weight);
        }

        // Make sure the weight is above 0
        if (_spawnWeights[waveData] < 0)
        {
            _spawnWeights[waveData] = 0;
        }
    }

    private bool AddWeight(string weights)
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
                    KeyValuePair<WaveDataScriptableObject, int> kvp = _spawnWeights.ToList()[id];
                    _spawnWeights[kvp.Key] += weight;
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

    private WaveDataScriptableObject GetRandomWave()
    {
        // Calculate the sum of all weights
        int sumOfWeights = 0;
        foreach (KeyValuePair<WaveDataScriptableObject, int> kvp in _spawnWeights)
        {
            if (kvp.Value > 0)
                sumOfWeights += kvp.Value;
        }

        if (sumOfWeights <= 0)
        {
            Debug.LogError($"No spawn weights defined, Sum of wieghts equals {sumOfWeights}");
            return spawnWaveData[0];
        }

        // get a random number between 0 and the sum of all weights
        int randomNum = Random.Range(0, sumOfWeights);
        // find the index of the first weight that is greater than the random number
        int count = 0;
        foreach (KeyValuePair<WaveDataScriptableObject, int> kvp in _spawnWeights)
        {
            count += kvp.Value;
            if (count > randomNum)
            {
                return kvp.Key;
            }
        }

        // fail safe
        Debug.LogWarning($"Unable to find a Spawn Wave Index with random number [{randomNum}]");
        return spawnWaveData[0];
    }

    public void SpawnWave(WaveDataScriptableObject waveData)
    {
        // calculate the spawn position relative to the player.turnpoint
        Vector3 pos = PlayerShipController.Instance.turnPoint;

        // spawn the wave
        waveData.SpawnWave(pos, this);
    }

    void Update()
    {
        // spawn an wave of actors every spawn interval
        if (_spawnTimer >= spawnInterval)
        {
            // get a random wave index
            WaveDataScriptableObject wave = GetRandomWave();

            // spawn an wave
            SpawnWave(wave);

            // reset the timer
            _spawnTimer -= spawnInterval;
            spawnInterval *= 0.9999f;
        }

        // increment the timer
        _spawnTimer += Time.deltaTime;
    }
}
