using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public float damage;
    public float range;
    private float _lifeTime;
    private float _time;

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
            Debug.Log($"Hit {hit.collider.name}");
            if (hit.collider.name == "Player")
            {

            }
        }

        transform.position += step;
    }
}
