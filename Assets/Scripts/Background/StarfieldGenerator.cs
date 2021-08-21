using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarfieldGenerator : MonoBehaviour
{
    public StarFieldParamaters paramaters;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

[System.Serializable]
public struct StarFieldParamaters
{
    public GameObject starPrefab;
    public float cellSize;
    public Vector2Int gridSize;
    public float depth;
}
