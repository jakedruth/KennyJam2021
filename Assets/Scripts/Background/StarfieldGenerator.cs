using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarFieldGenerator : MonoBehaviour
{
    //public Transform camera;

    public StarFieldParameter[] starFieldParameters;
    private StarFieldData[] _starFieldData;

    // Start is called before the first frame update
    void Start()
    {
        _starFieldData = new StarFieldData[starFieldParameters.Length];
        _starFieldData[0] = GenerateStarField(starFieldParameters[0]);
    }

    // Update is called once per frame
    void Update()
    {
        foreach (StarFieldData data in _starFieldData)
        {
            // Draw each cell
            for (int i = 0; i < data.parameter.gridDimension.x; i++)
            {
                for (int j = 0; j < data.parameter.gridDimension.y; j++)
                {
                    Vector3 position = data.bottomLeft + (data.parameter.cellSize * new Vector3(i, j, 0));
                    Debug.DrawLine(position, position + Vector3.left, Color.red, Time.deltaTime, false);
                }
            }
        }
    }

    private StarFieldData GenerateStarField(StarFieldParameter parameter)
    {
        StarFieldData data = new StarFieldData(parameter);

        // Create gameobject to hold field
        GameObject starFieldObject = new GameObject($"StarField_{parameter.depth}");
        starFieldObject.transform.SetParent(transform);
        starFieldObject.transform.localPosition = Vector3.zero;

        // Create stars 
        for (int y = 0; y < parameter.gridDimension.y; y++)
        {
            for (int x = 0; x < parameter.gridDimension.x; x++)
            {
                // Create star
                GameObject star = new GameObject($"Star_{x}_{y}");

                // Set the star's transform values
                star.transform.SetParent(starFieldObject.transform);
                Vector3 pos = data.bottomLeft + new Vector3(x * parameter.cellSize, y * parameter.cellSize, 0);
                Vector3 offset = new Vector3(Random.Range(-parameter.cellSize, parameter.cellSize), Random.Range(-parameter.cellSize, parameter.cellSize), 0);
                star.transform.localPosition = pos + offset;
                float scale = Random.Range(parameter.starScale.x, parameter.starScale.y);
                star.transform.localScale = Vector3.one * scale;
                star.transform.localRotation = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.forward);

                // Set the star's renderer values
                SpriteRenderer sr = star.AddComponent<SpriteRenderer>();
                sr.sprite = parameter.starSprite;
                sr.color = parameter.starColor.Evaluate(Random.Range(0, 1));

                // Add the star to the data
                data.stars.Add(star);
                data.starOffsets.Add(offset);
            }
        }

        return data;
    }
}

[System.Serializable]
public struct StarFieldParameter
{
    public float depth;
    public Sprite starSprite;
    public Gradient starColor;
    public Vector2 starScale;
    public Vector2Int gridDimension;
    public float cellSize;
}

public struct StarFieldData
{
    public readonly StarFieldParameter parameter;
    public readonly Vector3 bottomLeft;
    public List<GameObject> stars;
    public List<Vector3> starOffsets;

    public StarFieldData(StarFieldParameter paramater)
    {
        this.parameter = paramater;
        this.bottomLeft = bottomLeft = new Vector3(
            -parameter.gridDimension.x * 0.5f * parameter.cellSize,
            -parameter.gridDimension.y * 0.5f * parameter.cellSize, 0);
        stars = new List<GameObject>();
        starOffsets = new List<Vector3>();
    }
}