using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class EnemyShipController : ShipController
{
    public PathNode target;
    public TerrainGeneration terrainGenerator;
    // public GameObject circle;
    //public GameObject healthBar;
     // public GameObject healthMeter;
    public Assets.Port homePort;
    public Assets.Port fromPort;
    public Vector2 pathStart;
    public Vector2 pathEnd;

    public Vector2 destinationForViewing;

    private Vector2 from;
    private Vector2 targetDir;
    public Vector2 targetPos;
    private float targetDepth = 0.5f;

    [Header("Defenses")]
    public GameObject cannonPrefab;
    public int cannonCount = 20;
    public float fightReadiness = 0; // -1 is flight, 0 is defensive, 1 is offensive
    public ShipController attacking;

    [Header("Debug")]
    public GameObject debugMarker;

    public bool thinkingForSelf = false;

    // Start is called before the first frame update
    void Start()
    {
        base.Init();
        print("Literally about to call placecannons");
        PlaceCannons();

        name = NameGenerator.GenerateName(new float2(transform.position.x, transform.position.y), nameMap);

        if (thinkingForSelf)
        {
            Path result = new Path();
            int count = 0;

            while ((result == null || result.currentNode == null) && count < 50)
            {
                count++;
                result = terrainGenerator.AStar(
                    new Vector2(transform.position.x + UnityEngine.Random.Range(-100f, 100f),
                    transform.position.x + UnityEngine.Random.Range(-100f, 100f)),
                    new Vector2(transform.position.x + UnityEngine.Random.Range(-100f, 100f),
                    transform.position.x + UnityEngine.Random.Range(-100f, 100f))
                    );
            }
            target = result.currentNode;
            Vector3 pos1 = terrainGenerator.CellToWorld(target.cell);
            transform.position = new Vector3(pos1.x, pos1.y, transform.position.z);
        }
    }

    void PlaceCannons()
    {
        int mid = cannonCount / 2;
        for (int i = 0; i < cannonCount; i++)
        {
            GameObject cannon = Instantiate(cannonPrefab, transform);
            cannon.SetActive(true);
            float edgePos = (i % mid) / (float)mid;
            if (i < mid)
            {
                cannon.transform.localPosition = new Vector3(0.4f, edgePos - 0.33f, 0);
                cannon.transform.localRotation = Quaternion.Euler(0, 0, 90);
                starboardSideCannons.Add(cannon.GetComponent<Cannon>());
            } else
            {
                cannon.transform.localPosition = new Vector3(-0.4f, edgePos - 0.33f, 0);
                cannon.transform.localRotation = Quaternion.Euler(0, 0, -90);
                portSideCannons.Add(cannon.GetComponent<Cannon>());
            }
        }
    }
    /*
    void ReconstructPath(Vector2 from, )
    {

    }*/
    
    // Assess whether (continuing) combat engagement is worthwhile
    // Returns true if fight, false if flight
    bool FightOrFlight()
    {
        if (attacking == null) return false; // Why fight nothing?

        float armormentReadiness = 1;
        int cannonballsNeeded = (int)(attacking.health / 2); // cannonballs needed determined by number of cannonballs to deplete attacking's health
        if (cannonballsNeeded > 0)
            armormentReadiness = cargo.quantities[ResourceType.CannonBalls] / cannonballsNeeded; // if >1, we have more than enough cannonballs to take it out
        
        float healthAdvantage = 1;
        if (attacking.health > 0)
            healthAdvantage = health / attacking.health; // determine the health advantage
        
        // float fireRateAdvantage = attacking.CalculateFirePeriod() / CalculateFirePeriod(); // determine whether we can shoot faster. commented out because it's the same as healthAdvantage right now
        
        float solution = armormentReadiness * healthAdvantage; // * fireRateAdvantage; // simplest way to put all these factors  together. may need to add/adjust coefficients and constants
        if (solution > 0.5f) return true;
        return false;
    }

    void MoveToTarget(Vector2 targetPos3, float depth)
    {
        Vector2 direction = new Vector2(0, 0);
        // Vector2 targetPos3 = terrainGenerator.CellToWorld(target.cell);
        Vector2 targetPos = new Vector2(targetPos3.x, targetPos3.y);
        Vector2 thisPos2 = new Vector2(transform.position.x, transform.position.y);
        targetDir = (targetPos - thisPos2);
        float targetRot = math.atan2(targetPos.x - transform.position.x, targetPos.y - transform.position.y) * 180 / math.PI;
        float yMultiplier = 1;

        bool shouldFight = FightOrFlight();
        if (shouldFight && targetDir.magnitude < 4f && floating > 0.9f)
        {
            targetRot = (targetRot + 90) % 360;
            yMultiplier = 0.1f;
        } else if (!shouldFight && attacking != null)
        {
            attacking = null;
        } 

        float offRot = (-targetRot - transform.rotation.eulerAngles.z) % 360;
        offRot = (offRot + 360) % 360;

        if (offRot <= 180 && offRot > 10)// && Math.Abs(rb2D.angularVelocity) > 30)
        {
            direction.x += 1;
        }
        else if (offRot > 180 && offRot < 350)// && Math.Abs(rb2D.angularVelocity) <= 30)
        {
            direction.x -= 1;
        } else
        {
            if (attacking != null && targetDir.magnitude < 4f)
                FireCannons(portSideCannons);
        }

        direction.y = ((math.abs(180 - offRot) / 180f) * target.depth * 2 + 0.2f) * yMultiplier; //(target.depth * 0.3f + 0.1f); //(360 - math.abs(offRot)) / 360f;


        base.ApplyForce(direction, Time.deltaTime);
        /*Vector2 direction = Vector2.zero;
        from = new Vector2(transform.position.x, transform.position.y);
        targetDir = (targetPos - from);
        float targetRotation = math.atan2(targetPos.x - transform.position.x, targetPos.y - transform.position.y) * 180 / math.PI;
        if (attacking != null && targetDir.magnitude < 4f && target.prior == null && floating > 90)
        {
            targetRotation = (targetRotation - 90) % 360;
            FireCannons(starboardSideCannons);
        }
            //math.degrees(math.atan2(targetDir.x, targetDir.y));

        float offRotation = (-targetRotation - transform.rotation.eulerAngles.z + 360) % 360;


        if (offRotation <= 180 && offRotation > 10)// && Math.Abs(rb2D.angularVelocity) > 30)
        {
            direction.x += 1;
        }
        else if (offRotation > 180 && offRotation < 350)// && Math.Abs(rb2D.angularVelocity) <= 30)
        {
            direction.x -= 1;
        }
        direction.y = (math.abs(180 - offRotation) / 180f) * target.depth * 2 + 0.2f;
        if (attacking != null && targetDir.magnitude < 4f && target.prior == null) direction.y *= 2;

        base.ApplyForce(direction, Time.deltaTime);*/
    }

    void MoveToCell(Vector3Int targetCell, float depth)
    {
        MoveToTarget(terrainGenerator.CellToWorld(targetCell), depth);
    } 

    void MoveToNode(PathNode node)
    {
        if (node != null)
            MoveToCell(node.cell, node.depth); else
            MoveToTarget(transform.position, 0.05f);
    }

    void SelectTarget()
    {
        Path result = new Path();
        int count = 0;
        while ((result == null || result.currentNode == null) && count < 1)
        {
            count++;
            pathStart = new Vector2(transform.position.x, transform.position.y);
            pathEnd = new Vector2(transform.position.x + UnityEngine.Random.Range(-100f, 100f),
                transform.position.y + UnityEngine.Random.Range(-100f, 100f));
            result = terrainGenerator.AStar(
                pathEnd,
                pathStart
                );
        }
        if (result != null && result.currentNode != null)
        {
            target = result.currentNode;
            //Vector3 pos1 = terrainGenerator.CellToWorld(target.cell);
            //transform.position = new Vector3(pos1.x, pos1.y, transform.position.z);
        }
    }

    void ManagePath()
    {
        if (targetDir.magnitude< 2.56f)
        {
            if (target.prior == null && attacking != null)
            {
                targetPos = attacking.transform.position;
            }
            if (target == null) return;
            PathNode next = target.prior;
            if (next != null)
            {
                // targetDepth = target.depth;

                // GameObject newMarker = Instantiate(debugMarker);
                // newMarker.SetActive(true);
                // newMarker.transform.position = target.position;

                //if (next != null)
                target = next;
            } else
            {
                target = null;
                //if (thinkingForSelf)
                //    SelectTarget();
            }
        }
    }

    private void MoveToPoint(Vector2 targetPos3)
    {
        Vector2 direction = new Vector2(0, 0);
        // Vector2 targetPos3 = terrainGenerator.CellToWorld(target.cell);
        Vector2 targetPos = new Vector2(targetPos3.x, targetPos3.y);
        Vector2 thisPos2 = new Vector2(transform.position.x, transform.position.y);
        targetDir = (targetPos - thisPos2);
        float targetRot = math.atan2(targetPos.x - transform.position.x, targetPos.y - transform.position.y) * 180 / math.PI;

        float offRot = (-targetRot - transform.rotation.eulerAngles.z) % 360;
        offRot = (offRot + 360) % 360;

        if (offRot <= 180 && offRot > 10)// && Math.Abs(rb2D.angularVelocity) > 30)
        {
            direction.x += 1;
        }
        else if (offRot > 180 && offRot < 350)// && Math.Abs(rb2D.angularVelocity) <= 30)
        {
            direction.x -= 1;
        }

        direction.y = (math.abs(180 - offRot) / 180f) * target.depth * 2 + 0.2f; //(target.depth * 0.3f + 0.1f); //(360 - math.abs(offRot)) / 360f;

        base.ApplyForce(direction, Time.deltaTime);
    }

    public void SelectNext()
    {
        if (targetDir.magnitude < 2.26)
        {
            PathNode nextTarget = target.prior;
            if (nextTarget != null)
            {
                target = nextTarget;
            }
            else
            {
                target = null;
            }
        }

    }

    public override void DamageShip(float dmg, ShipController attacker)
    {
        if (attacker != null)
        {
            attacking = attacker;
            // Path toAttacker = terrainGenerator.AStar(attacker.transform.position, transform.position);
            // target = toAttacker.currentNode;
        }

        health -= dmg;
        if (health <= 0) health = 0;
        if (health < 100)
        {
            healthBar.SetActive(true);
            healthMeter.transform.localScale = new Vector3(health / 100, 1);
            healthMeter.transform.localPosition = new Vector3(health / 200 - 0.5f, 0);
        }
        if (health < 20)
        {
            rb2D.drag = 1000 / (20 - health);
            if (!circle.GetComponent<Renderer>().enabled)
            {
                circle.GetComponent<Renderer>().enabled = true;
                // circle.GetComponent<Renderer>().material.SetFloat("Radius", 0.2f);
                StartCoroutine(boardingCircle.TweenRadius());//ExpandBoarding());
                // circle.SetActive(true);
            }
            return;
        }
        if (health == 0)
        {
            healthBar.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            print("Target is null");
            return;
        }

        //MoveToPoint(terrainGenerator.CellToWorld(target.cell));
        //ManagePath();//SelectNext();

        if (attacking != null)
        {
            targetPos = attacking.transform.position;
            targetDepth = 2f;
            MoveToTarget(targetPos, 0f);
        }
        else if (target != null)
        {
            MoveToNode(target);

            //MoveToTarget(targetPos, targetDepth);
            ManagePath();
        }

                /*
                if (target == null)
                {
                    Destroy(gameObject);
                }
                MoveToNode(target);
                ManagePath();*/

                /*if (target == null)
                {
                    print("Target is null");
                    return;
                }*/


                // transform.position = terrainGenerator.
                /*Vector2 direction = new Vector2(0, 0);
                Vector2 targetPos3 = target.position;//terrainGenerator.CellToWorld(target.cell);
                Vector2 targetPos1 = new Vector2(targetPos3.x, targetPos3.y);
                Vector2 thisPos2 = new Vector2(transform.position.x, transform.position.y);
                Vector2 targetDir = (targetPos - thisPos2);
                float targetRot = math.atan2(targetPos1.x - transform.position.x, targetPos1.y - transform.position.y) * 180 / math.PI;
                print("The 'trics!");
                print(targetDir.magnitude);
                int random = UnityEngine.Random.Range(0, 1000000);
                print(name + random + " from: " + thisPos2);
                print(name + random + " to: " + target.position);
                //print(targetRot);
                float offRot = (-targetRot - transform.rotation.eulerAngles.z) % 360;
                //offRot = (offRot + 360) % 360;
                /*print(targetRot);
                print(transform.rotation.eulerAngles);
                print(offRot);*/
                // print(offRot);
                /*
                if (offRot <= 180 && offRot > 10)// && Math.Abs(rb2D.angularVelocity) > 30)
                {
                    direction.x += 1;
                } else if (offRot > 180 && offRot < 350)// && Math.Abs(rb2D.angularVelocity) <= 30)
                {
                    direction.x -= 1;
                }

                direction.y = (math.abs(180 - offRot) / 180f) * target.depth * 2 + 0.2f; //(target.depth * 0.3f + 0.1f); //(360 - math.abs(offRot)) / 360f;
                //direction.x = offRot / 10;
                // if (transform.position.y <
                //direction.x = UnityEngine.Random.Range(-1f, 1f);
                base.ApplyForce(direction, Time.deltaTime);

                //rb2D.MoveRotation(-targetRot);
                //transform.rotation = Quaternion.Euler(0, 0, -targetRot);

                if (targetDir.magnitude < 1.26f)
                {
                    PathNode nextTarget = target.prior;
                    if (nextTarget != null)
                    {
                        target = nextTarget;
                        targetPos = target.position;
                    }
                    else
                    {
                        target = null;
                        /*Path result = new Path();
                        int count = 0;
                        while ((result == null || result.currentNode == null) && count < 1)
                        {
                            count++;
                            pathStart = new Vector2(transform.position.x, transform.position.y);
                            pathEnd = new Vector2(transform.position.x + UnityEngine.Random.Range(-100f, 100f),
                                transform.position.y + UnityEngine.Random.Range(-100f, 100f));
                            result = terrainGenerator.AStar(
                                pathEnd,
                                pathStart
                                );
                        }
                        if (result != null && result.currentNode != null)
                        {
                            target = result.currentNode;
                            //Vector3 pos1 = terrainGenerator.CellToWorld(target.cell);
                            //transform.position = new Vector3(pos1.x, pos1.y, transform.position.z);
                        }*/
                //}
                //}



                base.Run();
    }
}
