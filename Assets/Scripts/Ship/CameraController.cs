using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private PlayerShipController _player;
    public float minCameraSpeed;
    public float maxCameraSpeed;
    public float minDistance;
    public float maxDistance;

    void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerShipController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_player != null)
        {
            Vector3 displacement = _player.turnPoint - transform.position;
            float sqrDist = displacement.sqrMagnitude;
            float t = Mathf.InverseLerp(minDistance * minDistance, maxDistance * maxDistance, sqrDist);
            float speed = Mathf.Lerp(minCameraSpeed, maxCameraSpeed, Mathf.Clamp01(t));
            transform.position = Vector3.Lerp(transform.position, _player.turnPoint, speed * Time.deltaTime);
        }
    }

    public Vector3 Damp(Vector3 a, Vector3 b, float lambda, float dt)
    {
        return Vector3.Lerp(a, b, 1 - Mathf.Exp(-lambda * dt));
    }
}
