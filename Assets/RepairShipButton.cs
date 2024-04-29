using Assets;
using Assets.Logic;
using Assets.Ships;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RepairShipButton : MonoBehaviour
{
    private Button button;
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            if (PlayerShipController1.Instance.ship.cargo.quantities[ResourceType.Wood] < 20) return;
            PlayerShipController1.Instance.ship.cargo.quantities[ResourceType.Wood] -= 20;
            PlayerShipController1.Instance.ship.Heal(5f, null);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
