using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TerrainGeneration : MonoBehaviour
{
    [Header("Generation")]
    public float waterLevel = 0.15f;
    public float featureSize = 7f;
    public float featureSizeVariation = 1.0f;
    public float featureSizeVariationFrequency = 7f;
    public float biomeSize = 32f;

    [Header("Tilemaps")]
    public Tilemap collidable;
    public Tilemap uncollidable;

    [Header("Tileset")]
    public Tile ground;
    public Tile mountains;
    public Tile port;
    public Tile forest;
    public Tile plains;
    public Tile water;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Returns true if the current cell is water. Cells do not need to be rendered first.
    public bool IsWater(Vector2 position)
    {
        Vector3Int cellPos = collidable.WorldToCell(position);
        return GetTerrainValue(cellPos.x, cellPos.y) >= waterLevel;
    }
    public bool IsWater(Vector3Int cellPos)
    {
        return GetTerrainValue(cellPos.x, cellPos.y) >= waterLevel;
    }

    public bool IsWater(int row, int col)
    {
        return GetTerrainValue(row, col) >= waterLevel;
    }

    public float GetTerrainValue(int row, int col)
    {
        float tmp = (
                    noise.snoise(
                        new float2(
                            (col) / featureSizeVariationFrequency,
                            (row) / featureSizeVariationFrequency)
                        )
                    ) * (featureSizeVariation
                        * (
                            500f / (
                                Vector2.zero
                                - new Vector2(col, row)
                            ).magnitude
                        )
                    );
        float val = noise.snoise(new float2(col / (featureSize + tmp), row / (featureSize + tmp)));
        return val;
    }

    public Vector3Int WorldToCell(Vector2 position)
    {
        return collidable.WorldToCell(position);
    }


    // Renders a bounds-sized block of cells centered on middle.
    public void RenderBlock(Vector2 middle, Vector2Int bounds)
    {
        Vector3Int cellPos = collidable.WorldToCell(middle);
        for (int col = cellPos.x - bounds.x / 2; col < cellPos.x + bounds.x / 2; col += 1)
        {
            for (int row = cellPos.y - bounds.y / 2; row < cellPos.y + bounds.y / 2; row += 1)
            {
                float val = GetTerrainValue(row, col);
                if (val <= waterLevel)
                {
                    float tileType = val * noise.snoise(new float2(col / biomeSize, row / biomeSize)) * 1.2f;
                    Tile tile;
                    if (tileType < 0.1)
                    {
                        tile = ground;
                    }
                    else if (tileType < 0.5)
                    {
                        tile = forest;
                    }
                    else if (tileType < 0.7)
                    {
                        tile = plains;
                    }
                    else if (tileType < 0.8)
                    {
                        tile = mountains;
                    }
                    else
                    {
                        tile = port;
                    }
                    collidable.SetTile(
                        new Vector3Int(
                            (int)(col),
                            (int)(row), 0),
                        tile);
                }
                else
                {
                    uncollidable.SetTile(
                        new Vector3Int(
                            (int)(col),
                            (int)(row), 0),
                        water);
                }
            }
        }
    }

    public void ClearPlusRender(Vector2 middle, Vector2Int bounds)
    {
        collidable.ClearAllTiles();
        uncollidable.ClearAllTiles();
        RenderBlock(middle, bounds);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
