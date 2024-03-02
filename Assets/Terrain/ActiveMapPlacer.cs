using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Mathematics;
using TMPro;

namespace Assets
{
    public class ActiveMapPlacer : MonoBehaviour
    {
        // long seed;
        // int numPorts;
        public TextMeshProUGUI textUI;
        // Vector2Int bounds = new Vector2Int(100, 100);

        public Config config;
        public TerrainGeneration terrainGenerator;
        public GameObject port;
        public GameObject shipPrefab;
        public Transform shipsContainer;
        public Transform portContainer;
        public PortNames portNames;

        public ActiveMapPlacer(long seed = 0, int numPorts = 5) {
            // bounds = new Vector2Int(100, 100);
            // this.seed = seed;
            // this.numPorts = numPorts;
        }

        public void Start()
        {
            // bounds = new Vector2Int(100, 100);
            // terrainGenerator.bounds = bounds;
            terrainGenerator.config = config;
            // seed = 10;
            // numPorts = 20;
            PlacePorts();
        }

        public PortNames LoadPortNames(string filename)
        {
            String contents = System.IO.File.ReadAllText(filename);
            return JsonUtility.FromJson<PortNames>(contents);

        }

        public List<Vector3Int> cellOffsets = new List<Vector3Int>{
            new Vector3Int(-1, 0), new Vector3Int(1, 0),
            new Vector3Int(0, -1),
            new Vector3Int(0, 1)
        };
        public void PlacePorts()
        {
            portNames = LoadPortNames("portnames.json");
            // print(bounds);
            for (int i = 0; i < config.numPorts; i++)
            {

                int count = 0;
                // find coastal.. this should be moved into terraingeneration
                Vector3Int cell = new Vector3Int(
                    (int)(noise.snoise(new float2(i * 1000 + config.seed, i * 1000)) * (config.bounds.x - 15)),
                    (int)(noise.snoise(new float2(i * 1000, i * 1000 + config.seed)) * (config.bounds.y - 15)),
                    0);
                int direction = (int)(math.abs(noise.snoise(new float2(i, config.seed * 1000))) * cellOffsets.Count);
                // print(direction);

                // print(cell);
                // print(noise.snoise(new float2(i * 1000 + seed, i * 1000)) * (float)bounds.x);

                Vector3Int original = cell;
                Vector3Int offset = cellOffsets[direction];
                while (terrainGenerator.IsWater(cell) || !terrainGenerator.IsWater(cell + offset) && count < config.bounds.y)
                {
                    count++;
                    cell += offset;
                    //cell.x = (cell.x + config.bounds.x * 2) % config.bounds.x * 2 - config.bounds.x;
                    //cell.y = (cell.y + config.bounds.y * 2) % config.bounds.y * 2 - config.bounds.y;
                    if (cell == original) break;
                }

                GameObject newPort = Instantiate(port, portContainer);
                newPort.transform.position = terrainGenerator.CellToWorld(cell);
                Port portData = newPort.GetComponent<Port>();
                portData.cell = cell;
                portData.dockCell = cell + offset;
                portData.terrainGenerator = terrainGenerator;
                portData.shipsContainer = shipsContainer;
                portData.portNames = portNames;
                portData.GenerateName(cell.x * 15 + cell.y * 10, (long)config.seed);
                portData.uiText = textUI; //.GetComponent<PortCollisions>().uiText = textUI;
                portData.shipPrefab = shipPrefab;
                // print(cell);
                if (!terrainGenerator.portCells.ContainsKey(cell))
                {
                    terrainGenerator.portCells.Add(cell, true);
                }

                Port.ports.Add(portData);
            }

            foreach (Port port in Port.ports)
            {
                port.PathToPorts();
            }
        }

        public void Update()
        {

        }

    }
}
