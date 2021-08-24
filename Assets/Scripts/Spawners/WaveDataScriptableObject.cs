using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Defualt_Wave_Data", menuName = "Scriptable Objects/Defualt Wave Data", order = 0)]
public class WaveDataScriptableObject : ScriptableObject
{
    public List<SpawnDataScriptableObject> spawnDataList;
    public WaveWeightPair[] addWeightsOnDestroyed;

    public void SpawnWave(Vector3 center, SpawnManager manager)
    {
        foreach (SpawnDataScriptableObject spawnData in spawnDataList)
        {
            // Spawn an actor
            Actor actor = spawnData.SpawnActor(manager, this, center);

            // Add an on death event
            if (addWeightsOnDestroyed.Length != 0)
            {
                actor.onActorDeath.AddListener((actor, args) =>
                    { manager.AddWeight(addWeightsOnDestroyed); });
            }
        }
    }
}

[System.Serializable]
public class WaveWeightPair
{
    [SerializeField]
    public WaveDataScriptableObject waveData;
    [SerializeField]
    public int weight;
}
