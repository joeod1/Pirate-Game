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

    public bool thinkingForSelf = false;

    // Start is called before the first frame update
    void Start()
    {
        base.Init();

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
    /*
    void ReconstructPath(Vector2 from, )
    {

    }*/

    void MoveToTarget(Vector2 targetPos, float depth)
    {
        Vector2 direction = Vector2.zero;
        from = new Vector2(transform.position.x, transform.position.y);
        targetDir = (targetPos - from);
        float targetRotation = math.degrees(math.atan2(targetDir.x, targetDir.y));
        float offRotation = (-targetRotation - transform.rotation.eulerAngles.z + 360) % 360;
        if (offRotation <= 180 && offRotation > 10)
        {
            direction.x += 1;
        } else if (offRotation > 180 && offRotation < 350)
        {
            direction.x -= 1;
        }
        direction.y = (math.abs(180 - offRotation) / 180f) * (target.depth * 0.3f + 0.1f);

        base.ApplyForce(direction, Time.deltaTime);
    }

    void MoveToCell(Vector3Int targetCell, float depth)
    {
        MoveToTarget(terrainGenerator.CellToWorld(targetCell), depth);
    } 

    void MoveToNode(PathNode node)
    {
        if (node != null)
            MoveToCell(node.cell, node.depth);
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
            if (target == null) return;
            PathNode next = target.prior;
            if (next != null)
            {
                destinationForViewing = terrainGenerator.CellToWorld(target.cell);
                target = next;
            } else
            {
                if (thinkingForSelf)
                    SelectTarget();
            }
        }
    }

    public override void DamageShip(float dmg)
    {
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
        
        /*
        if (target == null)
        {
            Destroy(gameObject);
        }
        MoveToNode(target);
        ManagePath();*/

        if (target == null)
        {
            print("Target is null");
            return;
        }
        

        
        Vector2 direction = new Vector2(0, 0);
        Vector2 targetPos3 = terrainGenerator.CellToWorld(target.cell);
        Vector2 targetPos = new Vector2(targetPos3.x, targetPos3.y);
        Vector2 thisPos2 = new Vector2(transform.position.x, transform.position.y);
        Vector2 targetDir = (targetPos - thisPos2);
        float targetRot = math.atan2(targetPos.x - transform.position.x, targetPos.y - transform.position.y) * 180 / math.PI;
        //print(targetRot);
        float offRot = (-targetRot - transform.rotation.eulerAngles.z) % 360;
        offRot = (offRot + 360) % 360;
        /*print(targetRot);
        print(transform.rotation.eulerAngles);
        print(offRot);*/
        // print(offRot);
        
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

        if (targetDir.magnitude < 2.26)
        {
            PathNode nextTarget = target.prior;
            if (nextTarget != null)
            {
                target = nextTarget;
            } else
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
            }
        }



        base.Run();
    }
}
