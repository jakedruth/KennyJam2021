using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float minCameraSpeed;
    public float maxCameraSpeed;
    public float minDistance;
    public float maxDistance;

    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 target = PlayerShipController.Instance.turnPoint;
        Vector3 displacement = target - transform.position;
        float sqrDist = displacement.sqrMagnitude;
        float t = Mathf.InverseLerp(minDistance * minDistance, maxDistance * maxDistance, sqrDist);
        float speed = Mathf.Lerp(minCameraSpeed, maxCameraSpeed, Mathf.Clamp01(t));
        transform.position = Vector3.Lerp(transform.position, target, speed * Time.deltaTime);
    }


    public Vector3 Damp(Vector3 a, Vector3 b, float lambda, float dt)
    {
        return Vector3.Lerp(a, b, 1 - Mathf.Exp(-lambda * dt));
    }
}
