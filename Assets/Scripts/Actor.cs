using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Actor : MonoBehaviour
{
    // health variables
    public bool isPlayer;
    public int maxHP;
    public int currentHP { get; private set; }

    public float delayDestroyOnDeath;

    // public events
    public OnActorHitEvent onActorHit = new OnActorHitEvent();
    public OnActorDeathEvent onActorDeath = new OnActorDeathEvent();
    public OnActorDestroyedEvent onActorDestroyed = new OnActorDestroyedEvent();

    private void Awake()
    {
        currentHP = maxHP;
    }

    [ContextMenu("Take Damage")]
    public void Testing_TakeOneDamage()
    {
        TakeDamage(1);
    }

    public void TakeDamage(int amount, Actor source = null)
    {
        // Handle damage
        currentHP -= amount;
        onActorHit?.Invoke(this, source);

        // If health is 0, call OnDeath
        if (currentHP <= 0)
        {
            OnDeath();
        }
    }

    public void Die()
    {
        // Handle special-case death
        OnDeath(this);
    }

    public void Despawn()
    {
        Destroy(gameObject);
    }

    private void OnDeath(Actor source = null)
    {
        // Call OnDeathEvent
        onActorDeath?.Invoke(this, source);

        // Destroy the ship
        Destroy(gameObject, delayDestroyOnDeath);
    }

    public void Destroy()
    {
        onActorDestroyed?.Invoke(this);
    }
}

public class OnActorHitEvent : UnityEvent<Actor, Actor> { }
public class OnActorDeathEvent : UnityEvent<Actor, Actor> { }
public class OnActorDestroyedEvent : UnityEvent<Actor> { }
