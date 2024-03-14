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
    public Vector2 offAxis = new Vector2(0, 0);

    [Header("Status")]
    public string name = "hms fun";
    public TradeResources cargo = new TradeResources();
    public float health = 100;
    public GameObject circle;
    public BoardingCircle boardingCircle;
    public bool sank = false;
    public GameObject healthBar;
    public GameObject healthMeter;

    [Header("Combat")]
    public List<Cannon> portSideCannons = new List<Cannon>(); // left side
    public List<Cannon> starboardSideCannons = new List<Cannon>(); // right side

    public static NameMap nameMap;

    static ShipController()
    {
        nameMap = NameGenerator.LoadFromFile("shipnames.json");
    }

    // Start is called before the first frame update
    void Start()
    {;
    }

    protected void Init()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    public void FireCannons(List<Cannon> cannons)
    {
        print("Got into the main FireCannons call");
        foreach (Cannon cannon in cannons)
        {
            print("Trying to fire a cannon");
            cannon.Fire();
        }
    }

    void UpdateModelRotation()
    {
        float primaryAngle = -transform.rotation.eulerAngles.z - 90f;

        shipModel.rotation = Quaternion.Euler(primaryAngle, 90f, -90f);
        shipModel.Rotate(new Vector3(0f, 0f, (horizontalAngle * math.sin((primaryAngle + 90 + offAxis.x) / 57.23f))
            + math.sin(Time.realtimeSinceStartup / 2f) * 10f - 5f));
        shipModel.Rotate(new Vector3((verticalAngle * math.sin((primaryAngle + 180 + offAxis.y) / 57.23f))
            + math.sin(Time.realtimeSinceStartup / 1f) * 5f - 2.5f, 0f, 0f));
        offAxis /= 1.05f;
    }

    protected void Sink()
    {
        shipModel.transform.position += new Vector3(0, 0, 0.01f);
    }

    public virtual void DamageShip(float dmg) { }

    public IEnumerator ExpandBoarding()
    {
        Renderer renderer = circle.GetComponent<Renderer>();
        int rad = Shader.PropertyToID("_Radius");
        int thick = Shader.PropertyToID("_Thickness");
        float tmp_rad = 0.45f; //renderer.material.GetFloat("Radius");
        float tmp_thick = 0.03f;
        float current = renderer.material.GetFloat(rad);
        float current_thick = renderer.material.GetFloat(thick);
        renderer.material.SetFloat(rad, current);
        while (current + 0.01f < tmp_rad)
        {
            yield return new WaitForSeconds(0.016f);
            current = (current * 10 + tmp_rad) / 11f;
            current_thick = (current_thick * 10 + tmp_thick) / 11f;
            renderer.material.SetFloat(rad, current);
            renderer.material.SetFloat(thick, current_thick);
        }
    }

    public IEnumerator ShrinkBoarding()
    {
        Renderer renderer = circle.GetComponent<Renderer>();
        int rad = Shader.PropertyToID("_Radius");
        int thick = Shader.PropertyToID("_Thickness");
        float tmp_rad = 0;
        float tmp_thick = 0;
        float current = renderer.material.GetFloat(rad);
        float current_thick = renderer.material.GetFloat(thick);
        renderer.material.SetFloat(rad, current);
        while (current - 0.01f > tmp_rad)
        {
            yield return new WaitForSeconds(0.016f);
            current = (current * 100 + tmp_rad) / 101f;
            current_thick = (current_thick * 100 + tmp_thick) / 101f;
            renderer.material.SetFloat(rad, current);
            renderer.material.SetFloat(thick, current_thick);
        }
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
        if (health == 0)
        {
            Sink();
            if (circle != null && sank == false)
            {
                rb2D.velocity = Vector3.zero;
                rb2D.isKinematic = true;
                GetComponent<CapsuleCollider2D>().enabled = false;
                sank = true;
                healthBar.SetActive(false);
                StartCoroutine(boardingCircle.TweenRadius(0, 0));
                //StartCoroutine(ShrinkBoarding());
            }
        }
        UpdateModelRotation();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
