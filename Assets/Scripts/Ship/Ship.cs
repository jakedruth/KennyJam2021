using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Actor))]
public class Ship : MonoBehaviour
{
    public Actor actor { get; private set; }

    [Header("Required Transforms")]
    public Transform projectilSpawnPoint;
    public ParticleSystem fireParticleSystem;
    public AudioSource fireAudioSource;
    public string projectilPrefab;

    void Awake()
    {
        actor = GetComponent<Actor>();

        actor.onActorHit.AddListener(OnShipHit);
        actor.onActorDestroyed.AddListener(OnShipDestroyed);
        actor.onActorDeath.AddListener(OnShipDeath);
    }

    public void Fire()
    {
        // Create a new bullet
        Bullet bullet = Instantiate(Resources.Load<Bullet>($"Prefabs/Bullets/{projectilPrefab}"), projectilSpawnPoint.position, projectilSpawnPoint.rotation);
        bullet._owner = actor;
        fireParticleSystem.Play();
        fireAudioSource.PlayOneShot(fireAudioSource.clip);
    }

    private void OnShipHit(Actor actor, Actor source)
    { }

    private void OnShipDeath(Actor arg0, Actor arg1)
    { }

    private void OnShipDestroyed(Actor arg0)
    { }
}
