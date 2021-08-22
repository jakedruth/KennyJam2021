using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerShipController : MonoBehaviour
{
    [Header("Required Transforms")]
    public Transform bulletSpawnPoint;
    public AudioSource fireAudioSource;

    [Header("Player Ship Properties")]
    public float maxHP;
    private float _currentHP;

    public OnShipHitEvent onShipHitEvent { get; set; }
    public OnShipDestroyedEvent onShipDestroyedEvent { get; set; }

    [Header("Movement Variables")]
    public float angularVelocity;
    public float turnRadius;
    public Vector3 turnPoint { get; private set; }
    private int _direction;

    [Header("Shooting variables")]
    public KeyCode shootKey;
    public string bulletPrefab;

    // Start is called before the first frame update
    void Start()
    {
        // Set up intial health
        _currentHP = maxHP;

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

    public void TakeDamage(float amount)
    {
        // Handle damage
        _currentHP -= amount;
        onShipHitEvent?.Invoke(this, amount);

        // If health is 0, destroy the object
        if (_currentHP <= 0)
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

public class OnShipDestroyedEvent : UnityEvent<PlayerShipController> { }
public class OnShipHitEvent : UnityEvent<PlayerShipController, float> { }