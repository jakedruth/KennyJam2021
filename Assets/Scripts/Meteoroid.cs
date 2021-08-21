using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteoroid : MonoBehaviour
{

    public float minSpeed;
    public float maxSpeed;
    private Vector2 _velocity;

    public float minRotationSpeed;
    public float maxRotationSpeed;
    private float _rotationSpeed;

    void Awake()
    {
        _velocity = Random.insideUnitCircle * Random.Range(minSpeed, maxSpeed);
        _rotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);
    }


    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, _rotationSpeed * Time.deltaTime);
        transform.Translate(_velocity * Time.deltaTime, Space.World);
    }
}
