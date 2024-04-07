using Assets;
using Assets.Logic;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class NewGame : MonoBehaviour
{
    [Header("Form Elements")]
    public TMPro.TMP_InputField seedInput;
    public TMPro.TMP_InputField portsNumInput;
    public UnityEngine.UI.Slider waterLevelInput;
    public TMPro.TMP_InputField boundsXInput;
    public TMPro.TMP_InputField boundsYInput;
    public TMPro.TMP_Dropdown difficultyInput;
    public GameObject newGameMenu;

    [Header("Game Elements")]
    public Config config;
    public Camera mainCamera;
    public TerrainGeneration generator;
    public ActiveMapPlacer mapPlacer;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(StartNewGame);
    }

    public void StartNewGame()
    {
        GameManager.Config.seed = SeededRandom.String2Seed(seedInput.text);
        GameManager.Config.numPorts = int.Parse(portsNumInput.text);
        GameManager.Config.waterLevel = waterLevelInput.value;
        GameManager.Config.bounds = new Vector2Int(
            int.Parse(boundsXInput.text),
            int.Parse(boundsYInput.text)
            );
        GameManager.Config.difficulty = difficultyInput.value;

        mainCamera.orthographicSize = 12;
        mapPlacer.enabled = true;
        generator.ClearPlusRender(new Vector2(0, 0), new Vector2Int(40, 30));
        newGameMenu.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
