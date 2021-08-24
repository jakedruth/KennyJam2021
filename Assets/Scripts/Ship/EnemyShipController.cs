using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShipController : MonoBehaviour
{
    [Header("Required Transforms")]
    public Transform bulletSpawnPoint;
    public AudioSource fireAudioSource;

    [Header("Movement Variables")]
    public float speed;
    public float turnSpeed;

    [Header("Ship Settings")]
    public int maxHP;
    public int currentHP { get; private set; }
    public float persueRange;
    public OnShipHitEvent onShipHitEvent { get; set; } = new OnShipHitEvent();
    public OnShipDestroyedEvent onShipDestroyedEvent { get; set; } = new OnShipDestroyedEvent();

    [Header("Laser variables")]
    public string bulletPrefab;
    public float fireRange;
    public float fireInterval;
    public int burstFireAmount;
    private int _burstFireCount;
    public float burstFireInterval;
    public bool isFiring { get; private set; }
    private float _fireTimer;


    // Start is called before the first frame update
    void Start()
    {
        //_velocity = Vector3.up * speed;
        currentHP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate the player data
        Vector2 playerPos = PlayerShipController.Instance.transform.position;
        Vector2 displacement = playerPos - (Vector2)transform.position;
        Vector2 direction = displacement.normalized;

        // Rotate the ship if in range of the player
        if (displacement.sqrMagnitude < persueRange * persueRange)
            HandleRotation(direction);

        // Handle Movement
        HandleMovement();

        // Handle Fire Logic
        HandleFire(displacement, direction);
    }

    void HandleRotation(Vector2 input)
    {
        float targetAngle = -Vector2.SignedAngle(input, Vector2.up);
        float heading = transform.eulerAngles.z;
        float newHeading = Mathf.MoveTowardsAngle(heading, targetAngle, turnSpeed * Time.deltaTime);
        transform.localRotation = Quaternion.AngleAxis(newHeading, Vector3.forward);
    }

    void HandleMovement()
    {
        transform.position += transform.up * speed * Time.deltaTime;
    }

    void HandleFire(Vector2 displacementToTarget, Vector2 directionToTarget)
    {
        _fireTimer += Time.deltaTime;
        if (!isFiring && _fireTimer >= fireInterval)
        {
            // Check to see if the target is in range
            float sqrDistance = displacementToTarget.sqrMagnitude;
            if (sqrDistance < fireRange * fireRange)
            {
                // Check to see if the target is infront of the ship
                float dotProdcut = Vector2.Dot(directionToTarget, bulletSpawnPoint.up);
                if (dotProdcut >= 0.5f)
                {
                    // Start Firing
                    isFiring = true;
                    _burstFireCount = burstFireAmount;
                }
            }
        }


        if (isFiring && _fireTimer >= burstFireInterval)
        {
            if (_burstFireCount > 0)
            {
                // Fire a bullet
                Fire();
                _burstFireCount--;
                _fireTimer = 0;
            }
            else
            {
                isFiring = false;
            }
        }
    }

    void Fire()
    {
        // Create a new bullet
        Instantiate(Resources.Load<Bullet>($"Prefabs/Bullets/{bulletPrefab}"), bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        fireAudioSource.PlayOneShot(fireAudioSource.clip);
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
            FindObjectOfType<HUD>().AddScore(maxHP);
            Destroy(gameObject);
        }
    }

    public void Destroy()
    {
        onShipDestroyedEvent?.Invoke(this);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        string tag = other.gameObject.tag;
        // If the other object is a asteroid, take damage
        switch (tag)
        {
            case "Asteroid":
                TakeDamage(1);
                other.gameObject.SendMessage("TakeDamage", int.MaxValue);
                break;
            case "Player":
                TakeDamage(maxHP);
                other.gameObject.SendMessage("TakeDamage", 3);
                break;
            default:
                break;
        }
    }
}
