using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShipController : MonoBehaviour
{
    public Transform bulletSpawnPoint;
    public float angularVelocity;
    public float turnRadius;
    public Vector3 turnPoint { get; private set; }
    private int _direction;
    public KeyCode shootKey;
    public string bulletPrefab;

    // Start is called before the first frame update
    void Start()
    {
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
    }

    void ChangeDirection()
    {
        // Change direction
        _direction *= -1;

        // Update turn point
        turnPoint = transform.position + transform.right * _direction * turnRadius;
    }
}
