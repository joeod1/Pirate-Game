using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class BoardingCircle : MonoBehaviour
{
    private bool locked = false;

    [Header("Circle Properties")]
    public GameObject circle;
    private Renderer renderer;
    public float radius = 0f;
    public float targetRadius = 0f;
    public float thickness = 0f;
    public float targetThickness = 0f;
    public float weight = 10f;

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

    public bool WithinTolerance(float variable, float goal, float tolerance = 0.1f)
    {
        return (variable - tolerance <= goal 
             && variable + tolerance >= goal);
    }

    public IEnumerator TweenRadius(float tgRadius = 0.05f, float tgThickness = 0.003f)
    {
        print("Tweening the circle's radius");
        targetRadius = tgRadius;
        targetThickness = tgThickness;

        radius = InvScale(renderer.material.GetFloat(radiusPropety));
        thickness = InvScale(renderer.material.GetFloat(thicknessProperty));
        while (!Mathf.Approximately(radius, targetRadius) || !Mathf.Approximately(thickness, targetThickness))
        {
            yield return new WaitForSeconds(0.016f);
            radius = (radius * weight + targetRadius) / (weight + 1);
            thickness = (thickness * weight + targetThickness) / (weight + 1);
            renderer.material.SetFloat(radiusPropety, Scale(radius));
            renderer.material.SetFloat(thicknessProperty, Scale(thickness));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerShipController player = collision.GetComponent<PlayerShipController>();
        if (player != null)
        {
            player.OnBoardingRadiusEntered(GetComponentInParent<ShipController>());
            //player.boardingRadiusShip = gameObject.GetComponentInParent<ShipController>();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerShipController player = collision.GetComponent<PlayerShipController>();
        if (player != null && player.boardingRadiusShip == GetComponentInParent<ShipController>())
        {
            player.boardingRadiusShip = null;
            player.hintText.text = "";
            //player.boardingRadiusShip = gameObject.GetComponentInParent<ShipController>();
        }
    }
}
