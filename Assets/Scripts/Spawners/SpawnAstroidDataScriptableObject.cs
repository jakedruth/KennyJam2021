using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Astroid Data", menuName = "Scriptable Objects/Spawn/Astroid Data", order = 0)]
public class SpawnAstroidDataScriptableObject : SpawnDataScriptableObject
{
    [Header("Astroid Data")]
    public RangedFloat speedRange;
    public RangedFloat angledOffsetRange;
    public bool addMirroredOffset;
    public string addWeightsOnDestroyed;

    public override GameObject SpawnGameObject(SpawnManager manager, WaveDataScriptableObject waveData, Vector3 center, params object[] args)
    {
        GameObject gameObject = base.SpawnGameObject(manager, waveData, center, args);
        Asteroid asteroid = gameObject.GetComponent<Asteroid>();
        if (asteroid != null)
        {
            // Apply rotational offset
            float delta = angledOffsetRange.GetRandomValue();
            if (delta != 0)
            {
                if (addMirroredOffset)
                {
                    float rng = Random.Range(0f, 1f);
                    if (rng < 0.5f)
                        delta *= -1;
                }

                asteroid.transform.Rotate(Vector3.forward, delta, Space.Self);
            }

            // calculate the velocity
            Vector3 velocity = asteroid.transform.up * speedRange.GetRandomValue();

            // set the velocity
            asteroid.SetVelocity(velocity);

            // Add Listener
            if (!string.IsNullOrEmpty(addWeightsOnDestroyed))
            {
                asteroid.onAstroidDestroyed.AddListener((args) =>
                {
                    if (!manager.AddWeight(addWeightsOnDestroyed))
                    {
                        Debug.Log($"This [{this}:{this.name}] sent bad weights", this);
                    }
                });
            }
        }

        return gameObject;
    }
}
