using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Defualt_Wave_Data", menuName = "Scriptable Objects/Defualt Wave Data", order = 0)]
public class WaveDataScriptableObject : ScriptableObject
{
    public List<SpawnDataScriptableObject> spawnDataList;

    public void SpawnWave(Vector3 center, SpawnManager manager)
    {
        foreach (SpawnDataScriptableObject spawnData in spawnDataList)
        {
            spawnData.SpawnGameObject(manager, this, center);
        }
    }
}
