using Assets;
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
    public TMPro.TMP_Text healthText;
    public TMPro.TMP_Text goldText;
    public TMPro.TMP_Text hintText;

    [Header("World")]
    public TerrainGeneration terrainGenerator;
    private Dictionary<Vector2Int, bool> chunksRendered = new Dictionary<Vector2Int, bool>();
    private Vector3Int knownPosition;
    public Vector2Int renderBounds = new Vector2Int(24, 20);
    public Vector2Int mapModeBounds = new Vector2Int(200, 200);
    private Vector2Int currentBounds;
    private bool renderedChunkZoomOut;
    public ShipController boardingRadiusShip;
    public Port boardingRadiusPort;
    public ShipSideGenerator sideGenerator;
    public ActiveMapPlacer topdownGenerator;

    public Transform point1;
    public Transform point2;


    // Start is called before the first frame update
    void Start()
    {
        base.Init();
        cargo.quantities[Assets.ResourceType.CannonBalls] = 100;
        if (terrainGenerator.config != null)
        {
            renderBounds = new Vector2Int(100, 100);
            terrainGenerator.RenderBlock(new Vector2(0, 0), new Vector2Int(100, 100));
        }
        UpdateHUD();
        // terrainGenerator.AStar(point1.position, point2.position);
    }

    public void OnBoardingRadiusEntered(ShipController ship)
    {
        boardingRadiusShip = ship;
        // StartCoroutine(sideGenerator.coGenerateShip(boardingRadiusShip));

    }

    private void UpdateHUD()
    {
        healthText.text = (int)health + "/" + (int)maxHealth;
        goldText.text = cargo.quantities[ResourceType.Gold].ToString() + " GOLD";
    }

    public override void DamageShip(float damage, ShipController attacker)
    {
        switch(Config.difficulty)
        {
            case 0:
                damage /= 2;
                break;
            case 1:
                // don't need to do anything!
                break;
            case 2:
                damage *= 2; // the enemies are stronger than us
                break;
            default:
                break;
        }
        health -= damage;
        UpdateHUD();
    }

    // Update is called once per frame
    void Update()
    {
        // render the cells around the player if they move to a new area
        Vector3Int currentPosition = terrainGenerator.WorldToCell(transform.position);
        if ((currentPosition - knownPosition).magnitude > renderBounds.magnitude / 5f)
        {
            renderedChunkZoomOut = false;
            if (Input.GetKey(KeyCode.M))
            {
                terrainGenerator.ClearPlusRender(transform.position, mapModeBounds);
            } else
            {
                terrainGenerator.ClearPlusRender(transform.position, renderBounds);
            }
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
            camera.orthographicSize = cameraSizeMap;
        }
        else if (Input.GetKeyUp(KeyCode.M))
        {
            terrainGenerator.ClearPlusRender(transform.position, renderBounds);
            camera.orthographicSize = cameraSizeNormal;
        }/* else if (Input.GetKey(KeyCode.M))
        {
            if (lerpCamera < 1)
            {
                lerpCamera += 0.1f;
                camera.orthographicSize = math.lerp(cameraSizeNormal, cameraSizeMap, lerpCamera);
            }
        } else if (lerpCamera > 0)
        {
            lerpCamera -= 0.05f;
            camera.orthographicSize = math.lerp(cameraSizeNormal, cameraSizeMap, lerpCamera);
        } else if (lerpCamera <= 0 && lerpCamera > -1f)
        {
            lerpCamera = -1f;
            terrainGenerator.ClearPlusRender(transform.position, renderBounds);
        }*/

        // allow ship boarding

        // {
        if (boardingRadiusShip != null && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)))
        {
            // hintText.text = "PRESS SPACE TO BOARD " + boardingRadiusShip.name;
            sideGenerator.playerShipCargo = cargo;
            sideGenerator.playerShip = this;
            sideGenerator.GenerateShip(boardingRadiusShip);
            topdownGenerator.gameObject.SetActive(false);
            sideGenerator.gameObject.SetActive(true);
        }
        //}

        // scroll to change zoom
        if (Input.mouseScrollDelta.y != 0)
        {
            camera.orthographicSize += Input.mouseScrollDelta.y / 2f;
            if (camera.orthographicSize < 2)
            {
                camera.orthographicSize = 2;
            }
            else if (camera.orthographicSize > 50)
            {
                camera.orthographicSize = 50 ;
            }
            renderBounds = new Vector2Int((int)camera.orthographicSize * 4 + 10, (int)camera.orthographicSize * 3 + 10);
            if (!renderedChunkZoomOut)
            {
                renderedChunkZoomOut = true;
                terrainGenerator.ClearPlusRender(transform.position, new Vector2Int(100, 100));
            }
        }

        // manual input for controls
        Vector2 direction = new Vector2(0, 0);
        if (Input.GetKey(KeyCode.W)) direction.y = 1;
        if (Input.GetKey(KeyCode.A)) direction.x += 1;
        if (Input.GetKey(KeyCode.D)) direction.x -= 1;
        base.ApplyForce(direction, Time.deltaTime);
        base.Run();

        // cannon firing
        if (Input.GetKeyDown(KeyCode.Q))
        {
            base.FireCannons(portSideCannons);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            base.FireCannons(starboardSideCannons);
        }

        cameraTransform.position = transform.position - new Vector3(0, 0, 5);
    }
}
