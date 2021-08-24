using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Asteroid : MonoBehaviour
{
    private Vector3 _velocity;
    public RangedFloat angularVelocityRange;
    private float _angularVelocity;
    public int maxHP;
    public int currentHP { get; private set; }


    public AstroidDestroyedEvent onAstroidDestroyed = new AstroidDestroyedEvent();

    void Awake()
    {
        _angularVelocity = angularVelocityRange.GetRandomValue();
        currentHP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward, _angularVelocity * Time.deltaTime);
        transform.Translate(_velocity * Time.deltaTime, Space.World);

        const float MAX_DISTANCE = 45f;
        float sqrDistToPlayer = Vector3.SqrMagnitude(transform.position - PlayerShipController.Instance.transform.position);
        if (sqrDistToPlayer > MAX_DISTANCE * MAX_DISTANCE)
        {
            Debug.Log("Here");
            Destroy(gameObject);
        }
    }

    public void SetVelocity(Vector3 velocity)
    {
        _velocity = velocity;
    }

    public void TakeDamage(int amount)
    {
        maxHP -= amount;
        if (maxHP <= 0)
        {
            FindObjectOfType<HUD>().AddScore(maxHP);
            Destroy(gameObject);
        }
    }

    public void OnDestroy()
    {
        onAstroidDestroyed?.Invoke(this);
    }
}

public class AstroidDestroyedEvent : UnityEvent<Asteroid> { }
