using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Defualt Data", menuName = "Scriptable Objects/Spawn/Defualt Data", order = 0)]
public class SpawnDataScriptableObject : ScriptableObject
{
    protected const float DEFAULT_SPAWN_DISTANCE = 20f;

    [Header("Base Spawn Data")]
    public GameObject prefab;

    public virtual GameObject SpawnGameObject(SpawnManager manager, WaveDataScriptableObject waveData, Vector3 center, params object[] args)
    {
        // Calculate the spawn position and direction to center
        Vector3 spawnPosition = GetSpawnPosition(center, args);

        // Calculate the spawn orientation
        Vector3 displacement = center - spawnPosition;
        Vector3 direction = displacement.normalized;
        float angle = -Vector3.SignedAngle(direction, Vector3.up, Vector3.forward);
        Quaternion SpawnOrientation = Quaternion.AngleAxis(angle, Vector3.forward);

        // Spawn the object
        GameObject spawnedObject = (GameObject)Instantiate(prefab, spawnPosition, SpawnOrientation);
        return spawnedObject;
    }

    public Vector3 GetSpawnPosition(Vector3 center, params object[] args)
    {
        return center + Quaternion.AngleAxis(Random.Range(0, 360), Vector3.forward) * Vector3.right * DEFAULT_SPAWN_DISTANCE;
    }
}
