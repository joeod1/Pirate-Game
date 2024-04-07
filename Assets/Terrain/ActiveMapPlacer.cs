using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Mathematics;
using TMPro;
using System.Threading;

namespace Assets
{
    [Serializable]
    public class ActiveMapPlacer : MonoBehaviour
    {
        // long seed;
        // int numPorts;
        public TextMeshProUGUI textUI;
        // Vector2Int bounds = new Vector2Int(100, 100);

        // public Config config;
        public TerrainGeneration terrainGenerator;
        public ShipSideGenerator shipSideGenerator;
        public GameObject port;
        public GameObject shipPrefab;
        public GameObject playerShip;
        public Transform shipsContainer;
        public Transform portContainer;
        public NameMap portNames;

        [SerializeField]
        public List<EnemyShipController> controllers;

        private int portsMade = 0;
        private int portsTotal = 0;

        public ActiveMapPlacer(long seed = 0, int numPorts = 5) {
            // bounds = new Vector2Int(100, 100);
            // this.seed = seed;
            // this.numPorts = numPorts;
        }

        public void Start()
        {
            // bounds = new Vector2Int(100, 100);
            // terrainGenerator.bounds = bounds;
            // terrainGenerator.config = config;
            shipSideGenerator.transform.position = new Vector3(GameManager.Config.bounds.x * 6, GameManager.Config.bounds.y * 6, 0);
            // seed = 10;
            // numPorts = 20;
            StartCoroutine(coPlacePorts());
            Port portWithDock = null;
            foreach (Port port in Port.ports)
            {
                if (port.dockCell != null)
                {
                    portWithDock = port;
                    break;
                }
            }
            playerShip.transform.position = terrainGenerator.CellToWorld(portWithDock.dockCell);

            StartCoroutine(coPrintAfterTime(10));
        }

        public NameMap LoadPortNames(string filename)
        {
            String contents = System.IO.File.ReadAllText(filename);
            return JsonUtility.FromJson<NameMap>(contents);
        }

        public IEnumerator coPlacePort(int i)
        {
            int count = 0;
            // find coastal.. this should be moved into terraingeneration
            Vector3Int cell = new Vector3Int(
                (int)(noise.snoise(new float2(i * 1000 + GameManager.Config.seed, i * 1000)) * (GameManager.Config.bounds.x - 15)),
                (int)(noise.snoise(new float2(i * 1000, i * 1000 + GameManager.Config.seed)) * (GameManager.Config.bounds.y - 15)),
                0);
            int direction = (int)(math.abs(noise.snoise(new float2(i, GameManager.Config.seed * 1000))) * cellOffsets.Count);
            // print(direction);

            // print(cell);
            // print(noise.snoise(new float2(i * 1000 + seed, i * 1000)) * (float)bounds.x);

            Vector3Int original = cell;
            Vector3Int offset = cellOffsets[direction];
            while ((terrainGenerator.IsWater(cell) || !terrainGenerator.IsWater(cell + offset)) && count < GameManager.Config.bounds.y)
            {
                count++;
                cell += offset;

                cell.x %= GameManager.Config.bounds.x;
                cell.y %= GameManager.Config.bounds.y;
                //cell.x = (cell.x + config.bounds.x * 2) % config.bounds.x * 2 - config.bounds.x;
                //cell.y = (cell.y + config.bounds.y * 2) % config.bounds.y * 2 - config.bounds.y;
                if (cell == original) break;
                yield return 0;
            }
            // if (count >= GameManager.Config.bounds.y) yield return 0;

            GameObject newPort = Instantiate(port, portContainer);
            newPort.transform.position = terrainGenerator.CellToWorld(cell);
            Port portData = newPort.GetComponent<Port>();
            portData.cell = cell;
            portData.dockCell = cell + offset;
            portData.terrainGenerator = terrainGenerator;
            portData.shipsContainer = shipsContainer;
            // portData.portNames = portNames;
            portData.GeneratePortInfo();//cell.x * 15 + cell.y * 10, (long)config.seed);
            newPort.name = portData.name;
            portData.uiText = textUI; //.GetComponent<PortCollisions>().uiText = textUI;
            portData.shipPrefab = shipPrefab;
            // print(cell);
            if (!terrainGenerator.portCells.ContainsKey(cell))
            {
                terrainGenerator.portCells.Add(cell, true);
            }

            Port.ports.Add(portData);
            Interlocked.Increment(ref portsMade);
            GameManager.Instance.loadingBar.UpdateBar("Generating ports: " + portsMade + "/" + portsTotal, portsMade, portsTotal);
        }

        public List<Vector3Int> cellOffsets = new List<Vector3Int>{
            new Vector3Int(-1, 0), new Vector3Int(1, 0),
            new Vector3Int(0, -1),
            new Vector3Int(0, 1)
        };
        public IEnumerator coPlacePorts()
        {
            GameManager.Instance.loadingBar.gameObject.SetActive(true);
            portsMade = 0;
            portsTotal = GameManager.Config.numPorts;
            portNames = LoadPortNames("portnames.json");

            List<Coroutine> co = new List<Coroutine>();
            // print(bounds);
            for (int i = 0; i < GameManager.Config.numPorts; i++)
            {
                co.Add(StartCoroutine(coPlacePort(i)));
                
                /*int count = 0;
                // find coastal.. this should be moved into terraingeneration
                Vector3Int cell = new Vector3Int(
                    (int)(noise.snoise(new float2(i * 1000 + GameManager.Config.seed, i * 1000)) * (GameManager.Config.bounds.x - 15)),
                    (int)(noise.snoise(new float2(i * 1000, i * 1000 + GameManager.Config.seed)) * (GameManager.Config.bounds.y - 15)),
                    0);
                int direction = (int)(math.abs(noise.snoise(new float2(i, GameManager.Config.seed * 1000))) * cellOffsets.Count);
                // print(direction);

                // print(cell);
                // print(noise.snoise(new float2(i * 1000 + seed, i * 1000)) * (float)bounds.x);

                Vector3Int original = cell;
                Vector3Int offset = cellOffsets[direction];
                while ((terrainGenerator.IsWater(cell) || !terrainGenerator.IsWater(cell + offset)) && count < GameManager.Config.bounds.y)
                {
                    count++;
                    cell += offset;
                    
                    cell.x %= GameManager.Config.bounds.x;
                    cell.y %= GameManager.Config.bounds.y;
                    //cell.x = (cell.x + config.bounds.x * 2) % config.bounds.x * 2 - config.bounds.x;
                    //cell.y = (cell.y + config.bounds.y * 2) % config.bounds.y * 2 - config.bounds.y;
                    if (cell == original) break;
                }
                if (count >= GameManager.Config.bounds.y) continue;

                GameObject newPort = Instantiate(port, portContainer);
                newPort.transform.position = terrainGenerator.CellToWorld(cell);
                Port portData = newPort.GetComponent<Port>();
                portData.cell = cell;
                portData.dockCell = cell + offset;
                portData.terrainGenerator = terrainGenerator;
                portData.shipsContainer = shipsContainer;
                // portData.portNames = portNames;
                portData.GeneratePortInfo();//cell.x * 15 + cell.y * 10, (long)config.seed);
                portData.uiText = textUI; //.GetComponent<PortCollisions>().uiText = textUI;
                portData.shipPrefab = shipPrefab;
                // print(cell);
                if (!terrainGenerator.portCells.ContainsKey(cell))
                {
                    terrainGenerator.portCells.Add(cell, true);
                }

                Port.ports.Add(portData);*/
            }

            for (int i = 0; i < portsMade; i++)
            {
                yield return co[i];
            }

            co.Clear();

            for (int i = 0; i < Port.ports.Count; i++)
            {
                Port port = Port.ports[i];
                // yield return port.coPathToPorts();
                GameManager.Instance.loadingBar.UpdateBar("Pathing ports: " + i + "/" + Port.ports.Count, i, Port.ports.Count);
                co.Add(StartCoroutine(port.coPathToPorts(i + 1)));
                yield return co[co.Count - 1];
            }
            
            for (int i = 0; i < portsMade; i++)
            {
                yield return co[i];
            }

            GameManager.Instance.loadingBar.gameObject.SetActive(false);
            yield return null;
        }


        public string PrintShips()
        {
            //controllers = shipsContainer.GetComponentsInChildren<EnemyShipController>().ToList();
            //string output = JsonUtility.ToJson(GameManager.Instance.saveStateManager, true);//JsonUtility.ToJson(this, true);
            //print(output);
            GameManager.Instance.saveStateManager.SaveGame();
            return "";//output;
        }

        public IEnumerator coPrintAfterTime(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            PrintShips();
        }

        public void Update()
        {

        }

    }
}
