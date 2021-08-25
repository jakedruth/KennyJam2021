using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Actor))]
public class Asteroid : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    public Actor actor { get; private set; }
    private Vector3 _velocity;
    public RangedFloat angularVelocityRange;
    private float _angularVelocity;

    void Awake()
    {
        // Get components
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        actor = GetComponent<Actor>();

        // seed the crack value
        _spriteRenderer.material.SetFloat("_CrackSeed", Random.Range(0f, 100f));
        actor.onActorHit.AddListener((a, source) =>
        {
            float crackValue = (float)a.currentHP / a.maxHP;
            _spriteRenderer.material.SetFloat("_CrackValue", crackValue);
        });

        // Add listener for when the actor is dead
        actor.onActorDeath.AddListener((a, sourceOfDeath) =>
        {
            if (sourceOfDeath == null || (sourceOfDeath != null && a != sourceOfDeath))
            {
                HUD.instance.AddScore(a.maxHP);
            }
        });

        // Set initial values
        _angularVelocity = angularVelocityRange.GetRandomValue();
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
                actor.Die();
            }
        }
    }

    public void SetVelocity(Vector3 velocity)
    {
        _velocity = velocity;
    }
}
