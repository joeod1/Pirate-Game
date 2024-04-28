using Assets;
using Assets.Logic;
using Assets.PlatformerFolder;
using Assets.Ships;
using Cainos.PixelArtPlatformer_VillageProps;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using Unity.Burst.CompilerServices;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

// ideally this should be divided up further: PlatformerManager holds ShipPlatformGenerator:PlatformGenerator and PortPlatformGenerator:PlatformGenerator
// maybe during tuning stage? would do it sooner, but vcs

public class ShipSideGenerator : MonoBehaviour
{
    public GameObject enemy;
    public Transform shipPartContainer;
    public Ship ship;
    public Ship playerShip;
    public Tilemap background;
    public Tile woodTile;
    public GameObject platform;
    public GameObject ladder;
    public GameObject wall;
    public GameObject beam;
    public GameObject lamp;
    public GameObject playerCharacter;
    public TradeResources playerShipCargo;
    public Vector2Int bounds = new Vector2Int(10, 3);
    public GameObject[] containers;
    public int shipSeed = -1;

    public PlatformerPalette palette;


    public LadderMap ladders;

    public GameObject enemyPrefab;


    // Start is called before the first frame update
    void Start()
    {
        if (playerShip != null) 
            playerShipCargo = playerShip.cargo;
        // GenerateShip(ship);

    }

    public void PlacePlatforms()
    {
        playerCharacter.GetComponent<Character>().playerCargo = playerShipCargo;

        // print(shipPartContainer.childCount);
        for (int i = 0; i < shipPartContainer.childCount; i++)
        {
            Destroy(shipPartContainer.GetChild(i).gameObject);
        }
        // print(shipPartContainer.childCount);

        // move the player to the appropriate location
        playerCharacter.transform.localPosition = new Vector3(bounds.x * 2, bounds.y * 3, 0);

        background.ClearAllTiles();
        for (int x = 0; x < bounds.x * 4; x += 3)
        {
            for (int y = -1; y < (bounds.y - 1) * 3; y += 3)
            {
                background.SetTile(background.WorldToCell(new Vector3(x + 1 + transform.position.x, y + transform.position.y)), woodTile);
            }
        }

        // place cargo around the ship
        TradeResources leftToPlace = new TradeResources();
        leftToPlace.quantities = ship.cargo.quantities.ToDictionary();//entry => entry.Key, entry => entry.Value);
        print("serialized right here: " + JsonUtility.ToJson(leftToPlace));
        int totalLeft = (int)leftToPlace.GetWeight();
        while (totalLeft > 0)
        {
            foreach (KeyValuePair<ResourceType, float> quantity in leftToPlace.quantities)
            {
                // we're done with this resource
                if (quantity.Value == 0)
                {
                    print(quantity.Key);
                    continue;
                }

                // create a new container
                GameObject chest = Instantiate(containers[(int)UnityEngine.Random.Range(0, containers.Length)], shipPartContainer);

                // place the container behind or in front of the player character
                Renderer chestRenderer = chest.GetComponent<Renderer>();
                if (UnityEngine.Random.Range(0, 10) > 5) {
                    print("behind");
                    chestRenderer.sortingOrder = -5;
                } else
                {
                    chestRenderer.sortingOrder = 7;
                }

                // empty the container
                ResourceContainer container = chest.GetComponent<ResourceContainer>();
                container.Empty();

                // place the container on a platform
                chest.transform.localPosition = new Vector3(((int)UnityEngine.Random.Range(0, bounds.x * 2)) * 2, ((int)UnityEngine.Random.Range(-1, bounds.y - 1)) * 3) + container.offset;
                if (container == null || container.contents == null)
                {
                    print("Null container on the ship!");
                }

                // place the resources into the container
                int quantityToGrab = (int)TradeResources.QuantityFromWeight(quantity.Key, container.capacity);
                if (quantityToGrab > quantity.Value) quantityToGrab = (int)quantity.Value;
                totalLeft -= (int)TradeResources.WeightFromQuantity(quantity.Key, quantityToGrab);
                container.contents.quantities[quantity.Key] = quantityToGrab;
                leftToPlace.quantities[quantity.Key] -= quantityToGrab;
                break;
            }
        }

        ladders = new LadderMap();

        for (int y = -1; y < bounds.y; y++)
        {
            // place a ladder for each floor
            int ladderPosition = (int)(math.abs(noise.snoise(new float2(y * 100, shipSeed)) * (bounds.x + 1)));
            for (int x = 0; x < bounds.x + 1; x++)
            {
                if (x < bounds.x)
                {
                    // place the floor if this isn't all the way to the right
                    GameObject newPlatform = Instantiate(platform, shipPartContainer);
                    newPlatform.transform.localPosition = new Vector2(x * 4, y * 3);
                    newPlatform.layer = 8;

                    // place ladders
                    if (x == ladderPosition && y >= 0)
                    {
                        newPlatform.GetComponent<Collider2D>().isTrigger = true;
                        GameObject newLadder = Instantiate(ladder, shipPartContainer);
                        newLadder.transform.localPosition = new Vector2(x * 4 + 2, y * 3 - 3);
                        ladders.Set(newLadder.transform.position);
                    }
                }
                if (y < 0) continue;

                // alternate between beams and lights
                if (x % 2 == 0)
                {
                    // non-collidable support beam
                    GameObject newBeam = Instantiate(beam, shipPartContainer);
                    newBeam.transform.localPosition = new Vector3(x * 4, y * 3 - 1.75f, 0.5f);

                    // transform beam for variety
                    float val = noise.snoise(
                                new float2(
                                    (x + y) * 100,
                                    shipSeed
                                )
                            );
                    if (val < 0) 
                        newBeam.transform.rotation = Quaternion.Euler(0, 0, -90);

                    if ((int)((val * 100) % 2) == 0)
                    {
                        newBeam.GetComponent<SpriteRenderer>().flipX = true;
                    }
                    if (x >= bounds.x) continue;
                } else
                {
                    // create a lamp on every other
                    GameObject newLamp = Instantiate(lamp, shipPartContainer);
                    newLamp.transform.localPosition = new Vector3(x * 4 - 0.855f, y * 3 - 3);
                }
            }
        }        
    }

    public void PlaceEnemies() {

        int crewMates = 3;

        CharacterCreator creator = GetComponentInParent<CharacterCreator>();
        
        for (int i = 0; i < crewMates; i++)
        {
            GameObject newEnemy = Instantiate(enemy, shipPartContainer);
            newEnemy.transform.localPosition = new Vector3(
                UnityEngine.Random.Range(0, bounds.x * 4),
                UnityEngine.Random.Range(0, bounds.y * 3)
                );
            newEnemy.GetComponent<NonPlayerController>().ladders = ladders;
            newEnemy.GetComponent<NonPlayerController>().target = playerCharacter;
            Character enemyCharacter = newEnemy.GetComponent<Character>();

            int rollForFish = UnityEngine.Random.Range(1, 7);
            if (rollForFish == 6)
            {
                creator.CreateFishCharacter(enemyCharacter);
            }
            else
            {
                creator.CreateHumanCharacter(enemyCharacter);
            }

            newEnemy.SetActive(true);
        }
    
    }

    public IEnumerator coFloodShip()
    {
        while (palette.water.transform.localScale.y <= 9)
        {
            yield return null;
            palette.water.transform.localScale = new Vector3(40, palette.water.transform.localScale.y + 0.1f * Time.deltaTime, 1);
            palette.water.transform.localPosition = new Vector3(20, 0.5f * palette.water.transform.localScale.y - 3f, 1);
        }
    }

    public void GenerateShip(Ship ship)
    {
        palette.water.transform.localScale = new Vector3(40, 0.5f, 1);
        palette.water.transform.localPosition = new Vector3(20, -2.75f, 1);
        StartCoroutine(coFloodShip());

        palette.sunLighting.gameObject.SetActive(false);
        this.ship = ship;
        playerCharacter.GetComponent<PlayerController>().boardedShip = ship;
        // create a seed from the ship's name
        for (int i = 0; i < ship.name.Length; i++)
        {
            shipSeed += (int)ship.name.ElementAt<char>(i);
        }
        shipSeed *= 2;
        PlacePlatforms();
        PlaceEnemies();
    }

    // generate the ship with a coroutine
    public IEnumerator coGenerateShip(Ship ship)
    {
        gameObject.SetActive(true);
        for (int i = 0; i < transform.childCount; i++)
        {
            //if (transform.GetChild(i).name != "Plane")
            transform.GetChild(i).gameObject.SetActive(true);
        }
        GenerateShip(ship);
        yield return null;
    }

    // Update is called once per frame
    /*void Update()
    {
        
    }*/
}
