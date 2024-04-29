using Assets;
using Assets.Resources;
using Assets.Ships;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem.XR;


public delegate void FloatEvent(float x);
public delegate void TargetSender(Ship target, Ship sender);

[Serializable]
public class Ship : MonoBehaviour, IDamageable
{
    [Header("Rendering")]
    [DoNotSerialize] public Transform shipModel;
    public Rigidbody2D rb2D;
    public float horizontalAngle = 45f;
    public float verticalAngle = 45f;
    public Vector2 offAxis;

    [DoNotSerialize] public FloatEvent OnDamage;
    [DoNotSerialize] public Action OnSank;

    [Header("Status")]
    public float health = 100, maxHealth = 100;
    public float sinkage = 0;  // 0 to 1, how far below water the ship is
    public float speed = 0, maxSpeed = 10;  // speed in knots. decreases as the ship takes on damage
    public float turningSpeed = 0, maxTurningSpeed = 1;
    public Crew crew;
    public TradeResources cargo;
    public TradeDeal tradeDeal = null;
    public float damageMultiplier = 1;  // amount of damage TAKEN
    public string homePort;
    public string destinationPort;
    public bool destroyOnDeath = true;

    [Header("Defenses")]
    public GameObject cannonPrefab;
    public int cannonCount = 4;
    [DoNotSerialize] public List<Cannon> portsideCannons = new List<Cannon>();
    [DoNotSerialize] public List<Cannon> starboardsideCannons = new List<Cannon>();
    [DoNotSerialize] public Ship attacker;

    [Header("UI")]
    public bool showBoardingCircle = true;
    [DoNotSerialize]
    public BoardingCircle boardingCircle;
    public bool showHealth = true;
    [DoNotSerialize] public GameObject healthBar;
    [DoNotSerialize] public GameObject healthMeter;


    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        boardingCircle.parent = gameObject;
        PlaceCannons();
    }

    /*public void Damage(float dmg, Ship attacker)
    {
        this.attacker = attacker;
        health -= dmg * damageMultiplier;
        if (showHealth) UpdateHealthBar();
        if (OnDamage != null) OnDamage(dmg);
    }*/

    public void Sink(float amount)
    {
        shipModel.transform.position += new Vector3(0, 0, amount);
        sinkage += amount;

        if (sinkage >= 3)
        {
            if (OnSank != null && sinkage < 3 + amount)
                OnSank();

            if (destroyOnDeath)
                Destroy(gameObject);
        }
    }

    public void DestroyCannons()
    {
        int ct = starboardsideCannons.Count;
        for (int i = 0; i < ct; i++)
        {
            // Remove all starboard side cannons from the scene
            Destroy(starboardsideCannons[i].gameObject);
        }
        // Destroy all references to starboard side Cannon objects
        starboardsideCannons.Clear();

        ct = portsideCannons.Count;
        for (int i = 0; i < ct; i++)
        {
            // Remove all port side cannons from the scene
            Destroy(portsideCannons[i].gameObject);
        }
        // Destroy all references to port side Cannon objects
        portsideCannons.Clear();
    }

    public bool ConsumeCargo(ResourceType type, int quantity)
    {
        // Check to see whether this amount of cargo may be consumed
        if (quantity > cargo.quantities[type]) return false;

        // Consume the cargo
        cargo.quantities[type] -= quantity;
        return true;
    }

    public void PlaceCannons()
    {
        // Start with a clean slate
        DestroyCannons();
        print("Placing cannons" + cannonCount);

        int mid = cannonCount / 2;  // Midpoint; set each half on opposite sides of the ship
        for (int i = 0; i < cannonCount; i++)
        {
            // Create a new cannon
            GameObject cannon = Instantiate(cannonPrefab, transform);
            Cannon cannonController = cannon.GetComponent<Cannon>();
            cannonController.ConsumeResource = ConsumeCargo;
            cannonController.parent = gameObject;
            // cannonController.LandedCallback = ShotLanded;
            cannon.SetActive(true);
            float edgePos = (i % mid) / (float)mid;
            if (i < mid)
            {
                cannon.transform.localPosition = new Vector3(0.4f, edgePos - 0.33f, 0);
                cannon.transform.localRotation = Quaternion.Euler(0, 0, 90);
                starboardsideCannons.Add(cannonController);
            }
            else
            {
                cannon.transform.localPosition = new Vector3(-0.4f, edgePos - 0.33f, 0);
                cannon.transform.localRotation = Quaternion.Euler(0, 0, -90);
                portsideCannons.Add(cannonController);
            }
        }
    }

    /*public void ShotLanded(float damage, Collider2D other)
    {
        print("Shot landed!");
        Ship otherShip = other.GetComponent<Ship>();
        if (otherShip != null)
        {
            if (otherShip != this)
            {
                otherShip.Damage(damage, this);

                float cannonRot = math.atan2(rb2D.velocity.x, rb2D.velocity.y);
                otherShip.offAxis += new Vector2(math.sin(cannonRot) * -10, math.cos(cannonRot) * 10);
            }
        } else
        {
            // TODO: Might be land? Pass it up to Port to check
        }
    }*/

    public void FireCannons(List<Cannon> cannons)
    {
        foreach (Cannon cannon in cannons)
        {
            cannon.Fire();
        }
    }

    public void ChangeVelocity(Vector2 amt)
    {
        speed += amt.y;
        if (speed > maxSpeed) speed = maxSpeed;
        if (speed < 0) speed = 0;

        turningSpeed = amt.x;
        //if (turningSpeed < -maxTurningSpeed) turningSpeed = -maxTurningSpeed;
        //if (turningSpeed > maxTurningSpeed) turningSpeed = maxTurningSpeed;
    }

    private void Move()
    {
        float velocity = Vector3.Dot(transform.up, rb2D.velocity);
        if (velocity < speed / 10)
        {
            rb2D.AddRelativeForce(new Vector2(0, 500 * Time.deltaTime));
        }
        
        rb2D.AddTorque(turningSpeed);
    }

    // the old movement mechanic
    public void ApplyForce(Vector2 directions, float dt)
    {
        if (directions.x == float.NaN) directions.x = 0;
        if (directions.y == float.NaN) directions.y = 0;
        if (dt == float.NaN) dt = 0;

        dt *= 200;
        Vector2 relativeForce = new Vector2(0, directions.y * dt / 2f);
        if (relativeForce.x == float.NaN) relativeForce.x = 0;
        if (relativeForce.y == float.NaN) relativeForce.y = 0;

        rb2D.AddRelativeForce(relativeForce);
        rb2D.AddTorque(directions.x * dt / 4f);
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

    void UpdateHealthBar()
    {
        if (health < maxHealth)
        {
            healthBar.SetActive(true);
            healthMeter.transform.localScale = new Vector3(health / maxHealth, 1);
            healthMeter.transform.localPosition = new Vector3(health / (maxHealth * 2) - 0.5f, 0);
        }
        else
        {
            healthBar.SetActive(false);
        }
    }

    private void Update()
    {
        if (health <= 0)
        {
            rb2D.velocity = Vector3.zero;
            healthBar.SetActive(false);
            showHealth = false;
            Sink(Time.deltaTime);
        }
        Move();
        UpdateModelRotation();
    }

    public void Damage(float dmg, GameObject attacker)
    {
        this.attacker = attacker.GetComponent<Ship>();
        health -= dmg * damageMultiplier;
        if (health <= 0)
        {
            StartCoroutine(boardingCircle.TweenRadius(tgRadius: 0.025f, tgThickness: 0));
            rb2D.velocity = Vector3.zero;
            GetComponent<CapsuleCollider2D>().enabled = false;
        }
        if (showHealth) UpdateHealthBar();
        if (showBoardingCircle && health > 0 && health < 5)
        {
            boardingCircle.boardable = true;
            StartCoroutine(boardingCircle.TweenRadius());
        }
        if (OnDamage != null) OnDamage(dmg);
    }

    public void Heal(float dmg, GameObject healer)
    {
        health += dmg;
        if (health > maxHealth) health = maxHealth;
        if (showHealth) UpdateHealthBar();
        if (OnDamage != null) OnDamage(-dmg);
    }

    public float GetHealth()
    {
        return health;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }
}
