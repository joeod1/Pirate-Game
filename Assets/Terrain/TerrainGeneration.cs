using Assets;
using Assets.Logic;
using Assets.Terrain;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public delegate void PathCallback(Path p);

public class TerrainGeneration : MonoBehaviour
{
    [Header("Config")]
    // public Config config;

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

    [Header("Debug")]
    public GameObject debugMarker;

    [Header("Rendering")]
    public List<TerrainZone> zones = new List<TerrainZone>();

    Dictionary<Vector3Int, CellData> occupiedCells = new Dictionary<Vector3Int, CellData>();
    public Dictionary<Vector3Int, bool> portCells = new Dictionary<Vector3Int, bool>();

    // Start is called before the first frame update
    void Start()
    {

    }

    public void AddZone(TerrainZone zone)
    {
        zones.Add(zone);
    }

    // Returns true if the current cell is water. Cells do not need to be rendered first.
    public bool IsWater(Vector2 position)
    {
        Vector3Int cellPos = collidable.WorldToCell(position);
        return GetTerrainValue(cellPos.y, cellPos.x) <= GameManager.Config.waterLevel;
    }
    public bool IsWater(Vector3Int cellPos)
    {
        return GetTerrainValue(cellPos.y, cellPos.x) <= GameManager.Config.waterLevel;
    }

    public bool IsWater(int row, int col)
    {
        return GetTerrainValue(row, col) <= GameManager.Config.waterLevel;
    }

    public float GetTerrainValue(int row, int col)
    {
        if (row < -GameManager.Config.bounds.y || row > GameManager.Config.bounds.y || col < -GameManager.Config.bounds.x || col > GameManager.Config.bounds.y) return -1;

        row += (int)GameManager.Config.seed;
        col -= (int)GameManager.Config.seed;

        float tmp = (
                    noise.snoise(
                        new float2(
                            (col) / GameManager.Config.featureSizeVariationFrequency,
                            (row) / GameManager.Config.featureSizeVariationFrequency)
                        )
                    ) * (GameManager.Config.featureSizeVariation
                        * (
                            500f / (
                                Vector2.zero
                                - new Vector2(col, row)
                            ).magnitude
                        )
                    );
        float val = noise.snoise(new float2(col / (GameManager.Config.featureSize + tmp), row / (GameManager.Config.featureSize + tmp)));
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
        startNode.h = Vector3.Distance(startNode.cell, endNode.cell) * 0.5f;
        // endNode.cell = end;

        if (!IsWater(startNode.cell) || !IsWater(endNode.cell)) return null;

        List<PathNode> open1 = new List<PathNode>();  // just make it a priorityqueue if unity's c# runtime ever gets that built-in (probably not) (I don't want to write the code for that myself)
        PriorityQueue<PathNode> open = new PriorityQueue<PathNode>();
        Dictionary<Vector3Int, PathNode> closed = new Dictionary<Vector3Int, PathNode>();
        //open.Add(startNode);
        open.Enqueue(startNode, startNode.h);

        // print("Beginning A*");
        int ct = 0;
        while (open.Count > 0 && ct < count)
        {
            ct++;
            if (ct > count - 10)
                print(ct);
            /* PathNode node = open[0];
             for (int i = 1; i < open.Count; i++)
             {
                 if (open[i].f < node.f || open[i].f == node.f && open[i].h < node.h)
                 {
                     node = open[i];
                 }
             }
             open.Remove(node);*/
            // PathNode node = open.Dequeue().Item1;
            // print(open.Print());
            PathNode node = open.Dequeue();
            // closed[node.cell] = node;
            if (closed.ContainsKey(node.cell)) continue;
            closed.Add(node.cell, node);

            if (node.cell == endNode.cell)
            {
                print(open.Print());

                // print("open cells: " + open.Print());
                // Logic to retrace path
                Path path = new Path();
                path.currentNode = node;

                /*PathNode womp = path.currentNode;
                while (womp != null)
                {
                    // print("ends at " + path.currentNode.position + ": " + womp.position);
                    if (womp.prior.position == Vector2.zero)
                    {
                        womp.prior = null;
                    }
                    womp = womp.prior;
                }*/

                
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
                
                /*PathNode bottom = node;
                uncollidable.SetTile(bottom.cell, forest);
                bottom = bottom.prior;
                //print(bottom.cell);
                while (bottom != null && bottom.prior != null)
                {
                    uncollidable.SetTile(bottom.cell, mountains);
                    //GameObject marker = Instantiate(debugMarker);
                    //marker.transform.position = bottom.position;
                    bottom = bottom.prior;
                }
                //print(bottom.cell);*/

                // print("Path? Found!");
                // print("open: " + open.Count + ", closed: " + closed.Count);
                // uncollidable.SetTile(bottom.cell, plains);*/

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
                if (terrainValue >= GameManager.Config.waterLevel || closed.ContainsKey(adjacentCell.cell))
                {
                    continue;
                }


                bool openContains = false;
                for (int v = 0; v < open.Count; v++)
                {
                    if (open[v].cell == node.cell)
                    {
                        openContains = true;
                        break;
                    }
                }

                adjacentCell.depth = math.abs((terrainValue + 1f));// * (1f / waterLevel));
                if (adjacentCell.depth > 0.5f)
                {
                    adjacentCell.depth = 0.5f;
                }
                //print(terrainValue + 1f);
                float temp = (1f - adjacentCell.depth);
                float newG = node.g + Vector3.Distance(node.cell, adjacentCell.cell) * temp * 1.2f;
                if (newG < adjacentCell.g || !openContains)
                {
                    adjacentCell.position = CellToWorld(adjacentCell.cell);
                    adjacentCell.g = newG;
                    adjacentCell.h = Vector3.Distance(node.cell, endNode.cell) * 0.5f; //* (1f - temp + 0.5f);// - terrainValue * 10f;
                    adjacentCell.f = adjacentCell.g + adjacentCell.h;
                    adjacentCell.prior = node;

                    if (!openContains)
                        open.Enqueue(adjacentCell, adjacentCell.h);
                }
            }
        }

        return null; //new Path();
    }
    public IEnumerator coAStar(Vector3Int start, Vector3Int end, PathCallback callback, int count = 2500, int port=0)
    {
        PathNode startNode = new PathNode(start);
        PathNode endNode = new PathNode(end);

        /*if (port == 30)
        {
            GameManager.Instance.loadingBar.UpdateBar("created nodes", 1, 1);
            yield return new WaitForSeconds(0.01f);
        }*/

        //if (port >= 30) Debug.Log(UnityEngine.Random.Range(0, 100000) + "Started A*");

        startNode.h = Vector3.Distance(startNode.cell, endNode.cell) * 0.5f;

        if (!IsWater(startNode.cell) || !IsWater(endNode.cell))
        {
            callback(null);
            yield break;
        }

        PriorityQueue<PathNode> open = new PriorityQueue<PathNode>();
        Dictionary<Vector3Int, PathNode> closed = new Dictionary<Vector3Int, PathNode>();
        open.Enqueue(startNode, startNode.h);

        int ct = 0;
        while (open.Count > 0 && ct < count)
        {
            ct++;
            PathNode node = open.Dequeue();

            if (closed.ContainsKey(node.cell)) continue;
            closed.Add(node.cell, node);
           // if (port >= 30) Debug.Log(UnityEngine.Random.Range(0, 100000) + "Dequeued new node");


            if (node.cell == endNode.cell)
            {
                // print("Pathed");
                Path path = new Path();
                path.currentNode = node;
               // if (port >= 30) Debug.Log(UnityEngine.Random.Range(0, 100000) + "Reached goal");

                callback(path);
                break;
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
                if (terrainValue >= GameManager.Config.waterLevel || closed.ContainsKey(adjacentCell.cell))
                {
                    continue;
                }

              //  if (port >= 30) Debug.Log(UnityEngine.Random.Range(0, 100000) + "Looking at cell");

                PathNode openContains = open.Find(node, (PathNode one, PathNode two) =>
                {
                    return one.cell == two.cell;
                });
                adjacentCell.depth = math.abs((terrainValue + 1f));
                if (adjacentCell.depth > 0.5f)
                {
                    adjacentCell.depth = 0.5f;
                }
                //print(terrainValue + 1f);
                float temp = (1f - adjacentCell.depth);
                float newG = node.g + Vector3.Distance(node.cell, adjacentCell.cell) * temp * 1.2f;
             //   if (port >= 30) Debug.Log(UnityEngine.Random.Range(0, 100000) + "Comparing metrics");
                if (newG < adjacentCell.g || openContains == null)
                {
                    if (openContains != null) 
                        adjacentCell = openContains;

                    adjacentCell.position = CellToWorld(adjacentCell.cell);
                    adjacentCell.g = newG;
                    adjacentCell.h = Vector3.Distance(node.cell, endNode.cell) * 0.5f;
                    adjacentCell.f = adjacentCell.g + adjacentCell.h;
                    adjacentCell.prior = node;

                    if (openContains == null)
                        open.Enqueue(adjacentCell, adjacentCell.h);

                 //   if (port >= 30) Debug.Log(UnityEngine.Random.Range(0, 100000) + "Adding node");
                }
            }
        }
        if (ct >= count) callback(null);
        yield return null;
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
                if (val >= GameManager.Config.waterLevel)
                {
                    float tileType = val * noise.snoise(new float2(col / GameManager.Config.biomeSize, row / GameManager.Config.biomeSize)) * 1.2f;
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

    public void RenderZones()
    {
        collidable.ClearAllTiles();
        uncollidable.ClearAllTiles();
        foreach (var zone in zones)
        {
            RenderBlock(zone.position, zone.size);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
