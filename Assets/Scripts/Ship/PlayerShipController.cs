using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShipController : MonoBehaviour
{
    public float speed;
    public float turnRadius;
    public Vector3 turnPoint { get; private set; }
    private int _direction;
    public KeyCode shootKey;

    // Start is called before the first frame update
    void Start()
    {
        _direction = 1;
        turnPoint = transform.position + transform.right * _direction * turnRadius;
    }

    // Update is called once per frame
    void Update()
    {
        // Update Position
        //transform.Rotate(0, 0, _direction * turnSpeed * Time.deltaTime);
        //transform.position += transform.up * speed * Time.deltaTime;

        transform.RotateAround(turnPoint, Vector3.forward, speed * -_direction * Time.deltaTime);

        // Handle Input
        if (Input.GetKeyDown(shootKey))
        {
            Fire();
            TurnDirection();
        }
    }

    void Fire()
    {
        Debug.Log("Fire");
    }

    void TurnDirection()
    {
        _direction *= -1;
        turnPoint = transform.position + transform.right * _direction * turnRadius;
    }
}
