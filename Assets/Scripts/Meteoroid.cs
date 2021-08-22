using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteoroid : MonoBehaviour
{
    public RangedFloat speedRange;
    private Vector2 _velocity;

    public RangedFloat rotationSpeedRange;
    private float _rotationSpeed;

    void Awake()
    {
        //* Temp debug code to get movement working
        Vector3 playerPos = FindObjectOfType<PlayerShipController>().transform.position;
        Vector3 displacement = playerPos - transform.position;

        SetMovementDireciton(displacement);
        /* */

        _rotationSpeed = rotationSpeedRange.GetRandomValue;
    }


    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, _rotationSpeed * Time.deltaTime);
        transform.Translate(_velocity * Time.deltaTime, Space.World);
    }

    void SetMovementDireciton(Vector3 direction)
    {
        _velocity = direction.normalized * speedRange.GetRandomValue;
    }
}
