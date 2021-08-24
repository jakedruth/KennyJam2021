using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public int damage;
    public float range;
    private float _lifeTime;
    private float _time;

    internal Actor _owner;

    private void Awake()
    {
        _lifeTime = range / speed;
    }

    // Update is called once per frame
    void Update()
    {
        _time += Time.deltaTime;
        if (_time > _lifeTime)
        {
            // Destroy self
            Destroy(this.gameObject);
            return;
        }

        float stepLength = speed * Time.deltaTime;
        Vector3 step = transform.up * stepLength;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, step.normalized, stepLength);
        if (hit.collider != null)
        {
            Debug.Log($"[{name}]: Hit {hit.collider.name}");
            Actor other = hit.collider.GetComponent<Actor>();
            switch (hit.collider.tag)
            {
                case "Asteroid":
                    other.TakeDamage(damage, _owner);
                    Destroy(gameObject);
                    break;
                case "Player":
                case "Enemy":
                    other.TakeDamage(damage, _owner);
                    Destroy(gameObject);
                    break;
                default:
                    break;
            }
        }

        transform.position += step;
    }
}