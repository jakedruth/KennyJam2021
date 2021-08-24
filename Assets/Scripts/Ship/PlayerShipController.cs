using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Actor))]
public class PlayerShipController : MonoBehaviour
{
    public static PlayerShipController Instance;
    public Actor actor { get; private set; }

    [Header("Required Transforms")]
    public Transform bulletSpawnPoint;
    public AudioSource fireAudioSource;

    [Header("Movement Variables")]
    public float angularVelocity;
    public float turnRadius;
    public Vector3 turnPoint { get; private set; }
    private int _direction;

    [Header("Laser variables")]
    public KeyCode shootKey;
    public string bulletPrefab;

    void Awake()
    {
        Instance = this;
        actor = GetComponent<Actor>();
        actor.isPlayer = true;

        // Set the initial direction
        _direction = -1;
        ChangeDirection();
    }

    // Update is called once per frame
    void Update()
    {
        // Update Position
        transform.RotateAround(turnPoint, Vector3.forward, angularVelocity * -_direction * Time.deltaTime);

        // Handle Input
        if (Input.GetKeyDown(shootKey))
        {
            Fire();
            ChangeDirection();
        }
    }

    void Fire()
    {
        // Create a new bullet
        Bullet bullet = Instantiate(Resources.Load<Bullet>($"Prefabs/Bullets/{bulletPrefab}"), bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        bullet._owner = actor;
        fireAudioSource.PlayOneShot(fireAudioSource.clip);
    }

    void ChangeDirection()
    {
        // Change direction
        _direction *= -1;

        // Update turn point
        turnPoint = transform.position + transform.right * _direction * turnRadius;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // If the other object is a asteroid
        // take damage and destroy the asteroid
        if (other.gameObject.tag.Contains("Asteroid"))
        {
            Actor otherActor = other.gameObject.GetComponent<Actor>();
            actor.TakeDamage(Mathf.CeilToInt(otherActor.currentHP / 2f), otherActor);
            otherActor.Die();
        }
    }
}