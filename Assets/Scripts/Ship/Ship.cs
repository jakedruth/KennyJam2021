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

    private void OnShipHit(Actor a, Actor source)
    { }

    private void OnShipDeath(Actor a, Actor source)
    {
        string explosionName = "ShipExplosion";
        GameObject explosionPrefab = Resources.Load<GameObject>($"Prefabs/Ships/{explosionName}");
        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
    }

    private void OnShipDestroyed(Actor a)
    { }
}
