using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Actor))]
public class Asteroid : MonoBehaviour
{
    public Actor actor { get; private set; }
    private Vector3 _velocity;
    public RangedFloat angularVelocityRange;
    private float _angularVelocity;
    public int maxHP;
    public int currentHP { get; private set; }

    void Awake()
    {
        actor = GetComponent<Actor>();
        actor.onActorDeath.AddListener((actor, sourceOfDeath) =>
        {
            if (sourceOfDeath == null || (sourceOfDeath != null && actor != sourceOfDeath))
            {
                HUD.instance.AddScore(actor.maxHP);
            }
        });

        _angularVelocity = angularVelocityRange.GetRandomValue();
        currentHP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward, _angularVelocity * Time.deltaTime);
        transform.Translate(_velocity * Time.deltaTime, Space.World);

        const float MAX_DISTANCE = 45f;

        if (PlayerShipController.Instance != null)
        {
            float sqrDistToPlayer = Vector3.SqrMagnitude(transform.position - PlayerShipController.Instance.transform.position);
            if (sqrDistToPlayer > MAX_DISTANCE * MAX_DISTANCE)
            {
                Debug.Log("Here");
                actor.Die();
            }
        }
    }

    public void SetVelocity(Vector3 velocity)
    {
        _velocity = velocity;
    }
}
