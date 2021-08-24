using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerShipController : MonoBehaviour
{
    public static PlayerShipController Instance;

    [Header("Required Transforms")]
    public Transform bulletSpawnPoint;
    public AudioSource fireAudioSource;

    [Header("Player Ship Properties")]
    public int maxHP;
    public int currentHP { get; private set; }

    public OnShipHitEvent onShipHitEvent { get; set; } = new OnShipHitEvent();
    public OnShipDestroyedEvent onShipDestroyedEvent { get; set; } = new OnShipDestroyedEvent();

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

        // Set up intial health
        currentHP = maxHP;

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
        Instantiate(Resources.Load<Bullet>($"Prefabs/Bullets/{bulletPrefab}"), bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        fireAudioSource.PlayOneShot(fireAudioSource.clip);
    }

    void ChangeDirection()
    {
        // Change direction
        _direction *= -1;

        // Update turn point
        turnPoint = transform.position + transform.right * _direction * turnRadius;
    }

    public void TakeDamage(int amount)
    {
        // Handle damage
        currentHP -= amount;
        onShipHitEvent?.Invoke(this, amount);

        // If health is 0, destroy the object
        if (currentHP <= 0)
        {
            // Destroy the ship
            Destroy(gameObject);
        }
    }

    public void Destroy()
    {
        onShipDestroyedEvent?.Invoke(this);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // If the other object is a asteroid, take damage
        if (other.gameObject.tag.Contains("Asteroid"))
        {
            TakeDamage(1);
            Destroy(other.gameObject);
        }
    }
}

public class OnShipDestroyedEvent : UnityEvent<object> { }
public class OnShipHitEvent : UnityEvent<object, int> { }