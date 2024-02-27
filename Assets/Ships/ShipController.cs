using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ShipController : MonoBehaviour
{
    [Header("Physics & Camera")]
    protected Rigidbody2D rb2D;

    [Header("Speed")]
    public float forwardSpeed;
    public float turningSpeed;

    [Header("3D Illusion")]
    public Transform shipModel;
    public float horizontalAngle;
    public float verticalAngle;

    [Header("Status")]
    public TradeResources cargo;
    public float health;

    [Header("Combat")]
    public List<Cannon> portSideCannons = new List<Cannon>(); // left side
    public List<Cannon> starboardSideCannons = new List<Cannon>(); // right side

    // Start is called before the first frame update
    void Start()
    {;
    }

    protected void Init()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    void UpdateModelRotation()
    {
        float primaryAngle = -transform.rotation.eulerAngles.z - 90f;

        shipModel.rotation = Quaternion.Euler(primaryAngle, 90f, -90f);
        shipModel.Rotate(new Vector3(0f, 0f, (horizontalAngle * math.sin((primaryAngle + 90) / 57.23f))
            + math.sin(Time.realtimeSinceStartup / 2f) * 10f - 5f));
        shipModel.Rotate(new Vector3((verticalAngle * math.sin((primaryAngle + 180) / 57.23f))
            + math.sin(Time.realtimeSinceStartup / 1f) * 5f - 2.5f, 0f, 0f));
    }

    // Move the ship; directions.x is turning, and directions.y is forward.
    protected void ApplyForce(Vector2 directions, float dt)
    {
        dt *= 200;
        rb2D.AddRelativeForce(new Vector2(0, directions.y * forwardSpeed * dt));
        rb2D.AddTorque(directions.x * turningSpeed * dt);
    }

    protected void Run()
    {
        UpdateModelRotation();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
