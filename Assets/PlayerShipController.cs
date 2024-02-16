using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerShipController : ShipController
{
    [Header("Physics & Camera")]
    public Camera camera;
    public Transform cameraTransform;
    public float cameraSizeNormal = 6f;
    public float cameraSizeMap = 100f;
    private float lerpCamera = 0f;

    [Header("World")]
    public TerrainGeneration terrainGenerator;
    private Dictionary<Vector2Int, bool> chunksRendered = new Dictionary<Vector2Int, bool>();
    private Vector3Int knownPosition;
    public Vector2Int renderBounds = new Vector2Int(24, 20);
    public Vector2Int mapModeBounds = new Vector2Int(200, 200);
    private Vector2Int currentBounds;


    // Start is called before the first frame update
    void Start()
    {
        base.Init();
        terrainGenerator.RenderBlock(new Vector2(0, 0), new Vector2Int(24, 24));
    }

    // Update is called once per frame
    void Update()
    {
        Vector3Int currentPosition = terrainGenerator.WorldToCell(transform.position);
        if ((currentPosition - knownPosition).magnitude > 5)
        {
            terrainGenerator.ClearPlusRender(transform.position, renderBounds);
            knownPosition = currentPosition;
        }
        /*Vector2Int chunkCoords = 
            new Vector2Int(
                (int)(transform.position.x / 25), 
                (int)(transform.position.y / 25));
        if (!chunksRendered.ContainsKey(chunkCoords))
        {
            RenderChunk((chunkCoords - new Vector2(1.5f, 1.5f)) * new Vector2(25, 25), new Vector2Int(30, 30));
            chunksRendered[chunkCoords] = true;
        }*/

        if (Input.GetKeyDown(KeyCode.M))
        {
            terrainGenerator.ClearPlusRender(transform.position, mapModeBounds);
        } else if (Input.GetKey(KeyCode.M))
        {
            if (lerpCamera < 1)
            {
                lerpCamera += 0.1f;
                camera.orthographicSize = math.lerp(cameraSizeNormal, cameraSizeMap, lerpCamera);
            }
        } else if (lerpCamera > 0)
        {
            lerpCamera -= 0.1f;
            camera.orthographicSize = math.lerp(cameraSizeNormal, cameraSizeMap, lerpCamera);
        }


        Vector2 direction = new Vector2(0, 0);
        if (Input.GetKey(KeyCode.W)) direction.y = 1;
        if (Input.GetKey(KeyCode.A)) direction.x += 1;
        if (Input.GetKey(KeyCode.D)) direction.x -= 1;
        base.ApplyForce(direction);
        base.Run();

        cameraTransform.position = transform.position - new Vector3(0, 0, 5);
    }
}
