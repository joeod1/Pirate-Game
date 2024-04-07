using Assets.Ships;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class BoardingCircle : MonoBehaviour
{
    private bool locked = false;

    [Header("Circle Properties")]
    public GameObject parent;
    public GameObject circle;
    private Renderer renderer;
    public float radius = 0f;
    public float targetRadius = 0f;
    public float thickness = 0f;
    public float targetThickness = 0f;
    public float weight = 10f;
    public bool boardable = false;

    private int radiusPropety;
    private int thicknessProperty;
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
        radiusPropety = Shader.PropertyToID("_Radius");
        thicknessProperty = Shader.PropertyToID("_Thickness");
    }

    public float Scale(float x)
    {
        return x * transform.localScale.x;
    }

    public float InvScale(float x)
    {
        return x / transform.localScale.x;
    }

    public bool WithinTolerance(float variable, float goal, float tolerance = 0.0001f)
    {
        return Mathf.Abs(variable - goal) <= tolerance; //(variable - tolerance <= goal 
             //&& variable + tolerance >= goal);
    }

    public IEnumerator TweenRadius(float tgRadius = 0.05f, float tgThickness = 0.003f)
    {
        print("Tweening the circle's radius to " + tgRadius + ", " + tgThickness);
        targetRadius = tgRadius;
        targetThickness = tgThickness;

        radius = InvScale(renderer.material.GetFloat(radiusPropety));
        thickness = InvScale(renderer.material.GetFloat(thicknessProperty));
        while (!Mathf.Approximately(radius, targetRadius) || !Mathf.Approximately(thickness, targetThickness))
        //(!WithinTolerance(radius, targetRadius) || !WithinTolerance(thickness, targetThickness))  // 
        {
            yield return new WaitForSeconds(0.016f);
            radius = (radius * weight + targetRadius) / (weight + 1);
            thickness = (thickness * weight + targetThickness) / (weight + 1);
            print("adjusting radius: " + radius + ", thickness: " + thickness);
            renderer.material.SetFloat(radiusPropety, Scale(radius));
            renderer.material.SetFloat(thicknessProperty, Scale(thickness));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerShipController1 player = collision.GetComponent<PlayerShipController1>();
        if (player != null)
        {
            // player.OnBoardingRadiusEntered(GetComponentInParent<Ship>());
            //player.boardingRadiusShip = gameObject.GetComponentInParent<ShipController>();
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        IBoards player = collision.GetComponent<IBoards>();
        if (player != null && boardable)
        {
            player.EnteredRadius(parent);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        IBoards player = collision.GetComponent<IBoards>();
        if (player != null && boardable)
        {
            player.LeftRadius(parent);
        }
    }
}
