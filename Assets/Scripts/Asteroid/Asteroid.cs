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
    public float crackSpeed;

    void Awake()
    {
        // Get components
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        actor = GetComponent<Actor>();

        // seed the crack value
        _spriteRenderer.material.SetFloat("_CrackSeed", Random.Range(0f, 100f));
        actor.onActorHit.AddListener(OnAsteroidHit);

        // Add listener for when the actor is dead
        actor.onActorDeath.AddListener(OnAsteroidDeath);

        // Add listener for when the actor is destroyed
        actor.onActorDestroyed.AddListener(OnAsteroidDestroyed);

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
                actor.Despawn();
            }
        }
    }

    public void SetVelocity(Vector3 velocity)
    {
        _velocity = velocity;
    }

    private void OnAsteroidHit(Actor a, Actor source)
    {
        float crackValue = (float)a.currentHP / a.maxHP;
        StopAllCoroutines();
        StartCoroutine(LerpCrackValue(crackValue, crackSpeed));
    }

    private IEnumerator LerpCrackValue(float targetValue, float duration)
    {
        float timer = 0;
        float startValue = _spriteRenderer.material.GetFloat("_CrackValue");
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float lerpValue = Mathf.Lerp(startValue, targetValue, timer / duration);
            _spriteRenderer.material.SetFloat("_CrackValue", lerpValue);
            yield return null;
        }

        _spriteRenderer.material.SetFloat("_CrackValue", targetValue);
    }

    private void OnAsteroidDeath(Actor a, Actor source)
    {
        if (source != this)
        {
            // Spawn Explosion
            string explosionName = "AsteroidExplosion";
            GameObject explosionPrefab = Resources.Load<GameObject>($"Prefabs/Asteroids/{explosionName}");
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }

        // Deterime if points should be awarded
        if (source == null || (source != null && a != source))
        {
            HUD.instance.AddScore(a.maxHP);
        }
    }

    private void OnAsteroidDestroyed(Actor a)
    {
    }
}
