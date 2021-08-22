using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Asteroid : MonoBehaviour
{
    private Vector3 _velocity;
    public RangedFloat rotationSpeedRange;
    private float _rotationSpeed;
    public float health;

    public AstroidDestroyedEvent onAstroidDestroyed = new AstroidDestroyedEvent();

    void Awake()
    {
        _rotationSpeed = rotationSpeedRange.GetRandomValue;
    }


    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward, _rotationSpeed * Time.deltaTime);
        transform.Translate(_velocity * Time.deltaTime, Space.World);
    }

    public void SetVelocity(Vector3 velocity)
    {
        _velocity = velocity;
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void OnDestroy()
    {
        onAstroidDestroyed?.Invoke(this);
    }
}

public class AstroidDestroyedEvent : UnityEvent<Asteroid> { }
