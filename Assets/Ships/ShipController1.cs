using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController1 : MonoBehaviour
{
    [SerializeReference] public Ship ship;

    // Start is called before the first frame update
    public virtual void Start()
    {
        ship = GetComponent<Ship>();
    }
}
