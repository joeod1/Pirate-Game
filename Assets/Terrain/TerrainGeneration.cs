using Assets;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class TerrainGeneration : MonoBehaviour
{
    [Header("Config")]
    public Config config;

    // Handled by Config now... Should it show here, too?
    /* [Header("Generation")]
    public float waterLevel = 0.15f;
    public float featureSize = 7f;
    public float featureSizeVariation = 1.0f;
    public float featureSizeVariationFrequency = 7f;
    public float biomeSize = 32f;
    public Vector2Int bounds; */

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

    Dictionary<Vector3Int, CellData> occupiedCells = new Dictionary<Vector3Int, CellData>();
    public Dictionary<Vector3Int, bool> portCells = new Dictionary<Vector3Int, bool>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Returns true if the current cell is water. Cells do not need to be rendered first.
    public bool IsWater(Vector2 position)
    {
        Vector3Int cellPos = collidable.WorldToCell(position);
        return GetTerrainValue(cellPos.y, cellPos.x) <= config.waterLevel;
    }
    public bool IsWater(Vector3Int cellPos)
    {
        return GetTerrainValue(cellPos.y, cellPos.x) <= config.waterLevel;
    }

    public bool IsWater(int row, int col)
    {
        return GetTerrainValue(row, col) <= config.waterLevel;
    }

    public float GetTerrainValue(int row, int col)
    {
        if (row < -config.bounds.y || row > config.bounds.y || col < -config.bounds.x || col > config.bounds.y) return -1;

        row += (int)config.seed;
        col -= (int)config.seed;

        float tmp = (
                    noise.snoise(
                        new float2(
                            (col) / config.featureSizeVariationFrequency,
                            (row) / config.featureSizeVariationFrequency)
                        )
                    ) * (config.featureSizeVariation
                        * (
                            500f / (
                                Vector2.zero
                                - new Vector2(col, row)
                            ).magnitude
                        )
                    );
        float val = noise.snoise(new float2(col / (config.featureSize + tmp), row / (config.featureSize + tmp)));
        return val;
    }

    public Vector3Int WorldToCell(Vector2 position)
    {
        return collidable.WorldToCell(position);
    }

    public Vector3 CellToWorld(Vector3Int cell)
    {
        return collidable.CellToWorld(cell);
    }

    public bool OccupyCell(Vector3Int cell, float direction)
    {
        if (occupiedCells.ContainsKey(cell)) { 
            if ((direction - occupiedCells[cell].direction + 360) % 360 < 10)
            {
                occupiedCells[cell].numOccupying++;
                return true;
            } else
            {
                return false;
            }
        } else
        {
            CellData value = new CellData();
            value.direction = direction;
            value.numOccupying = 1;
            occupiedCells.Add(cell, value);
            return true;
        }
    }

    public bool UnoccupyCell(Vector3Int cell)
    {
        if (occupiedCells.ContainsKey(cell))
        {
            occupiedCells.Remove(cell);
            return true;
        }
        return false;
    }


    Vector3Int[] adjacentCellOffsets = {
        new Vector3Int(-1, 0), new Vector3Int(1, 0)
    };
    Vector3Int[] oddCellOffsets = {
        new Vector3Int(0, -1), new Vector3Int(1, -1),
        new Vector3Int(0, 1), new Vector3Int(1, 1),
    };
    Vector3Int[] evenCellOffsets = {
        new Vector3Int(0, -1), new Vector3Int(-1, -1),
        new Vector3Int(0, 1), new Vector3Int(-1, 1),
    };
    public Path AStar(Vector2 start, Vector2 end, int count = 2500)
    {
        return AStar(WorldToCell(start), WorldToCell(end), count);
    }
    public Path AStar(Vector3Int start, Vector3Int end, int count = 2500)
    {
        PathNode startNode = new PathNode(start);
        // startNode.cell = start;
        PathNode endNode = new PathNode(end);
        // endNode.cell = end;

        if (!IsWater(startNode.cell) || !IsWater(endNode.cell)) return null;

        List<PathNode> open = new List<PathNode>();
        Dictionary<Vector3Int, PathNode> closed = new Dictionary<Vector3Int, PathNode>();
        open.Add(startNode);

        print("Beginning A*");

        while (open.Count > 0 && closed.Count < count)
        {
            PathNode node = open[0];
            for (int i = 1; i < open.Count; i++)
            {
                if (open[i].f < node.f || open[i].f == node.f && open[i].h < node.h)
                {
                    node = open[i];
                }
            }
            open.Remove(node);
            closed[node.cell] = node;

            if (node.cell == endNode.cell)
            {
                // Logic to retrace path
                Path path = new Path();
                path.currentNode = node;
                /*

                foreach (KeyValuePair<Vector3Int, PathNode> node1 in closed)
                {
                    uncollidable.SetTile(node1.Value.cell, port);
                }

                foreach (PathNode node1 in open)
                {
                    if (closed.ContainsKey(node1.cell))
                    {
                        uncollidable.SetTile(node1.cell, plains);
                    } else
                    {

                        uncollidable.SetTile(node1.cell, mountains);
                    }
                }*/
                /*
                PathNode bottom = node;
                uncollidable.SetTile(bottom.cell, forest);
                bottom = bottom.prior;
                //print(bottom.cell);
                while (bottom != null && bottom.prior != null)
                {
                    uncollidable.SetTile(bottom.cell, mountains);
                    bottom = bottom.prior;
                }
                //print(bottom.cell);
                uncollidable.SetTile(bottom.cell, plains);*/

                return path;
            }

            for (int i = 0; i < 6; i++)
            {
                PathNode adjacentCell;
                if (i < 2)
                {
                    adjacentCell = new PathNode(node.cell + adjacentCellOffsets[i]);
                }
                else
                {
                    if (node.cell.y % 2 == 0)
                        adjacentCell = new PathNode(node.cell + evenCellOffsets[i - 2]);
                    else
                        adjacentCell = new PathNode(node.cell + oddCellOffsets[i - 2]);
                }

                float terrainValue = GetTerrainValue(adjacentCell.cell.y, adjacentCell.cell.x);
                if (terrainValue >= config.waterLevel || closed.ContainsKey(adjacentCell.cell))
                {
                    continue;
                }

                if (closed.ContainsKey(adjacentCell.cell))
                {
                    print(closed[adjacentCell.cell]);
                }


                bool openContains = false;
                for (int v = 0; v < open.Count; v++)
                {
                    if (open[v].cell == node.cell)
                    {
                        openContains = true;
                    }
                }

                adjacentCell.depth = math.abs((terrainValue + 1f));// * (1f / waterLevel));
                if (adjacentCell.depth > 0.5f)
                {
                    adjacentCell.depth = 0.5f;
                }
                //print(terrainValue + 1f);
                float temp = (1f - adjacentCell.depth);
                
                float newG = node.g + Vector3.Distance(node.cell, adjacentCell.cell) * temp;
                if (newG < adjacentCell.g || !openContains)
                {
                    adjacentCell.g = newG;
                    adjacentCell.h = Vector3.Distance(node.cell, endNode.cell) * (1f - temp + 0.5f);// - terrainValue * 10f;
                    adjacentCell.prior = node;

                    if (!openContains)
                        open.Add(adjacentCell);
                }
            }
        }

        return new Path();
    }

    // Renders a bounds-sized block of cells centered on middle.
    public void RenderBlock(Vector2 middle, Vector2Int bounds)
    {
        Vector3Int cellPos = collidable.WorldToCell(middle);
        for (int col = cellPos.x - bounds.x / 2; col < cellPos.x + bounds.x / 2; col += 1)
        {
            for (int row = cellPos.y - bounds.y / 2; row < cellPos.y + bounds.y / 2; row += 1)
            {
                if (portCells.ContainsKey(new Vector3Int(col, row, 0)))
                {
                    collidable.SetTile(
                        new Vector3Int(
                            (int)(col),
                            (int)(row), 0),
                        port);
                    continue;
                }

                float val = GetTerrainValue(row, col);
                if (val >= config.waterLevel)
                {
                    float tileType = val * noise.snoise(new float2(col / config.biomeSize, row / config.biomeSize)) * 1.2f;
                    UnityEngine.Tilemaps.Tile tile;
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
                        tile = plains;
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
