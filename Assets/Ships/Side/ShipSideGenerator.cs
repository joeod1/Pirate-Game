using Assets;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Unity.Burst.CompilerServices;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ShipSideGenerator : MonoBehaviour
{
    public Transform shipPartContainer;
    public ShipController ship;
    public Tilemap background;
    public Tile woodTile;
    public GameObject platform;
    public GameObject ladder;
    public GameObject wall;
    public GameObject beam;
    public GameObject lamp;
    public Vector2Int bounds = new Vector2Int(10, 3);
    public GameObject[] containers;
    public int shipSeed = -1;


    // Start is called before the first frame update
    void Start()
    {
        // GenerateShip(ship);
    }

    public void PlacePlatforms()
    {
        // print(shipPartContainer.childCount);
        for (int i = 0; i < shipPartContainer.childCount; i++)
        {
            Destroy(shipPartContainer.GetChild(i).gameObject);
        }
        // print(shipPartContainer.childCount);

        for (int x = 0; x < bounds.x * 4; x += 3)
        {
            for (int y = -1; y < (bounds.y - 1) * 3; y += 3)
            {
                background.SetTile(background.WorldToCell(new Vector3(x + 1 + transform.position.x, y + transform.position.y)), woodTile);
            }
        }

        TradeResources leftToPlace = new TradeResources();
        leftToPlace.quantities = ship.cargo.quantities.ToDictionary(entry => entry.Key, entry => entry.Value);
        int totalLeft = leftToPlace.GetWeight();
        while (totalLeft > 0)
        {
            foreach (KeyValuePair<ResourceType, int> quantity in leftToPlace.quantities)
            {
                if (quantity.Value == 0) continue;

                GameObject chest = Instantiate(containers[(int)UnityEngine.Random.Range(0, containers.Length)], shipPartContainer);
                Renderer chestRenderer = chest.GetComponent<Renderer>();
                if (UnityEngine.Random.Range(0, 10) > 5) {
                    print("behind");
                    chestRenderer.sortingOrder = -5;
                } else
                {
                    chestRenderer.sortingOrder = 7;
                }
                ResourceContainer container = chest.GetComponent<ResourceContainer>();
                chest.transform.localPosition = new Vector3(((int)UnityEngine.Random.Range(0, bounds.x * 2)) * 2, ((int)UnityEngine.Random.Range(-1, bounds.y - 1)) * 3) + container.offset;
                if (container == null || container.contents == null)
                {
                    print("Null container on the ship!");
                }
                int quantityToGrab = TradeResources.QuantityFromWeight(quantity.Key, container.capacity);
                if (quantityToGrab > quantity.Value) quantityToGrab = quantity.Value;
                totalLeft -= quantityToGrab;
                container.contents.quantities[quantity.Key] = quantityToGrab;
                break;
            }
        }

        for (int y = -1; y < bounds.y; y++)
        {
            int ladderPosition = (int)(math.abs(noise.snoise(new float2(y * 100, shipSeed)) * (bounds.x + 1)));
            for (int x = 0; x < bounds.x + 1; x++)
            {
                if (x < bounds.x)
                {
                    GameObject newPlatform = Instantiate(platform, shipPartContainer);
                    newPlatform.transform.localPosition = new Vector2(x * 4, y * 3);
                    newPlatform.layer = 8;

                    if (x == ladderPosition && y >= 0)
                    {
                        newPlatform.GetComponent<Collider2D>().isTrigger = true;
                        GameObject newLadder = Instantiate(ladder, shipPartContainer);
                        newLadder.transform.localPosition = new Vector2(x * 4 + 2, y * 3 - 3);
                    }
                }
                if (y < 0) continue;

                if (x % 2 == 0)
                {
                    GameObject newBeam = Instantiate(beam, shipPartContainer);
                    newBeam.transform.localPosition = new Vector3(x * 4, y * 3 - 1.75f, 0.5f);
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
                    GameObject newLamp = Instantiate(lamp, shipPartContainer);
                    newLamp.transform.localPosition = new Vector3(x * 4 - 0.855f, y * 3 - 3);
                }
            }
        }        
    }

    public void GenerateShip(ShipController ship)
    {
        this.ship = ship;
        for (int i = 0; i < ship.name.Length; i++)
        {
            shipSeed += (int)ship.name.ElementAt<char>(i);
        }
        shipSeed *= 2;
        PlacePlatforms();
    }

    public IEnumerator coGenerateShip(ShipController ship)
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
