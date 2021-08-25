using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Actor))]
[RequireComponent(typeof(Ship))]
public class EnemyShipController : MonoBehaviour
{
    public Actor actor { get; private set; }
    public Ship ship { get; private set; }

    [Header("Movement Variables")]
    public float speed;
    public float turnSpeed;

    [Header("Ship Settings")]
    public float persueRange;
    private Vector2 _playerPos;
    private Vector2 _displacement;
    private Vector2 _direction;

    [Header("Laser variables")]
    public float fireRange;
    public float fireInterval;
    public int burstFireAmount;
    private int _burstFireCount;
    public float burstFireInterval;
    public bool isFiring { get; private set; }
    private float _fireTimer;


    private void Awake()
    {
        // Get the actor and ship components
        actor = GetComponent<Actor>();
        ship = GetComponent<Ship>();

        // Add listeners
        actor.onActorDeath.AddListener((actor, sourceOfDeath) =>
        {
            Actor other = sourceOfDeath as Actor;
            if (sourceOfDeath == null || (sourceOfDeath != null && actor != sourceOfDeath))
            {
                HUD.instance.AddScore(actor.maxHP);
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate the player data
        if (PlayerShipController.Instance != null)
        {
            _playerPos = PlayerShipController.Instance.transform.position;
        }

        // calculate the displacement to the player
        _displacement = _playerPos - (Vector2)transform.position;
        _direction = _displacement.normalized;

        // Rotate the ship if in range of the player
        if (_displacement.sqrMagnitude < persueRange * persueRange)
            HandleRotation(_direction);

        // Handle Movement
        HandleMovement();

        // Handle Fire Logic
        HandleFire();

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

    void HandleFire()
    {
        _fireTimer += Time.deltaTime;
        if (!isFiring && _fireTimer >= fireInterval)
        {
            // Check to see if the target is in range
            float sqrDistance = _displacement.sqrMagnitude;
            if (sqrDistance < fireRange * fireRange)
            {
                // Check to see if the target is infront of the ship
                float dotProdcut = Vector2.Dot(_direction, ship.projectilSpawnPoint.up);
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
                ship.Fire();
                _burstFireCount--;
                _fireTimer = 0;
            }
            else
            {
                isFiring = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        string tag = other.gameObject.tag;
        Actor otherActor = other.gameObject.GetComponent<Actor>();
        if (otherActor != null)
        {
            switch (tag)
            {
                case "Asteroid":
                    actor.TakeDamage(otherActor.currentHP, otherActor);
                    otherActor.Die();
                    break;
                case "Player":
                    actor.TakeDamage(actor.maxHP, otherActor);
                    otherActor.TakeDamage(3, actor);
                    break;
                default:
                    break;
            }
        }
    }
}
