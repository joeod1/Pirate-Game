using Assets;
using Assets.Logic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject newGameScreen;
    public SaveStateManager saveStateManager = new();

    public ActiveMapPlacer topdownMode;
    public ShipSideGenerator sideShipMode;

    public LoadingBar loadingBar;

    public Config iConfig;
    public static Config Config { get { return Instance.iConfig; } }

    public void Start()
    {
        newGameScreen.SetActive(true);
    }
    private void Awake()
    {
        Instance = this;
    }

    public static void SideShipMode(Ship boarding)
    {
        Instance.topdownMode.gameObject.SetActive(true);
        Instance.sideShipMode.ship = boarding;
        Instance.sideShipMode.gameObject.SetActive(true);
    }

    public static void TopDownMode(ShipController deboarding)
    {
        Instance.sideShipMode.gameObject.SetActive(false);
        deboarding.health = 0;
        Instance.topdownMode.gameObject.SetActive(true);
    }

    /*public static string SerializeGame()
    {
        return "";
    }*/
}
