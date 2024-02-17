using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyShipController : ShipController
{
    public GameObject target;
    public TerrainGeneration terrainGenerator;

    // Start is called before the first frame update
    void Start()
    {
        base.Init();
    }
    /*
    void ReconstructPath(Vector2 from, )
    {

    }*/

    void CalculatePath(Vector2 start, Vector2 end)
    {
        Vector3Int startCell = terrainGenerator.WorldToCell(start);
        Vector3Int endCell = terrainGenerator.WorldToCell(end);

        List<Vector3Int> openSet = new List<Vector3Int>();
        HashSet<Vector3Int> closedSet = new HashSet<Vector3Int>();
        openSet.Add(startCell);

        while (openSet.Count > 0)
        {
            Vector3Int current = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                // if (openSet[i].fC)
            }
        }
        //terrainGenerator.IsWater();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = new Vector2(0, 0);
        Vector2 targetPos3 = target.GetComponent<Transform>().position;
        Vector2 targetPos = new Vector2(targetPos3.x, targetPos3.y);
        Vector2 thisPos2 = new Vector2(transform.position.x, transform.position.y);
        Vector2 targetDir = (targetPos - thisPos2);
        float targetRot = math.atan2(targetPos.x - transform.position.x, targetPos.y - transform.position.y) * 180 / math.PI;
        //print(targetRot);
        float offRot = (targetRot - transform.rotation.eulerAngles.z);
        /*print(targetRot);
        print(transform.rotation.eulerAngles);
        print(offRot);*/
        if (offRot < -30 && Math.Abs(rb2D.angularVelocity) < 30)
        {
            direction.x += 1;
        } else if ((offRot > 30) && Math.Abs(rb2D.angularVelocity) < 30)
        {
            direction.x -= 1;
        }

        direction.y = (360 - math.abs(offRot)) / 360f;
        //direction.x = offRot / 10;
        // if (transform.position.y <
        //direction.x = UnityEngine.Random.Range(-1f, 1f);
        base.ApplyForce(direction);

        transform.rotation = Quaternion.Euler(0, 0, -targetRot);

        base.Run();
    }
}
