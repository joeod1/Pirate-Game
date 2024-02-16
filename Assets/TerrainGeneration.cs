using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TerrainGeneration : MonoBehaviour
{
    [Header("Generation")]
    public float waterLevel = 0.15f;
    public float featureSize = 7f;
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
        float val = noise.snoise(new float2(cellPos.x / featureSize, cellPos.y / featureSize));
        return val >= waterLevel;
    }

    public Vector3Int WorldToCell(Vector2 position)
    {
        return collidable.WorldToCell(position);
    }

    // Renders a bounds-sized block of cells centered on middle.
    public void RenderBlock(Vector2 middle, Vector2Int bounds)
    {
        Vector3Int cellPos = collidable.WorldToCell(middle);
        for (float col = cellPos.x - bounds.x / 2; col < cellPos.x + bounds.x / 2; col += 1)
        {
            for (float row = cellPos.y - bounds.y / 2; row < cellPos.y + bounds.y / 2; row += 1)
            {
                float val = noise.snoise(new float2(col / featureSize, row / featureSize));
                if (val > waterLevel)
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
