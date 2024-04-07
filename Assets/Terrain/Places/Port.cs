using Assets.Logic;
using Assets.Ships;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

namespace Assets
{

    [Serializable]
    public class Port : MonoBehaviour, ISerializationCallbackReceiver
    {
        [Header("Map")]
        public TerrainGeneration terrainGenerator;
        public Vector3Int cell;
        public Vector3Int dockCell;

        [Header("Relations")]
        public GameObject player;
        public string name;
        public NameMap portNames;
        public int3 nameAssembly;
        public TextMeshProUGUI uiText;
        public bool pirateFriendly = false;
        public Dictionary<string, float> pirateRelations;
        public int citizens = 0; // number of total citizens
        public int residents = 0; // number of citizens located within the port
        public float defenseNeed = 0; // 0 = no cannons, defense vessels; 1 = many cannons, lots of defense vessels per trade vessel
        public float shipNeed = 0; // 0 = no ship production, resources saved (can be traded); 1 = high ship production, resources diverted
        private TradeResources resources = new TradeResources(); // the resources that the port has
        private TradeResources productions = new TradeResources(); // the resources that are produced by the port
        private TradeResources needs = new TradeResources(); // needed resources

        [Header("Other Ports")]
        static public List<Port> ports = new List<Port>();
        public Dictionary<Port, Path> paths;

        [Header("Wealth")]
        public float wealth;
        public float food;
        public float drink;
        public float gold;
        public int production; // change to enum; produces two of [wood, food, drink, gold]

        [Header("Ships")]
        public GameObject shipPrefab;
        public List<EnemyShipController1> ships;
        public Transform shipsContainer;

        public static NameMap nameMap;

        // what if wood is produced by chopping down wood cells?
        // create a dictionary and iterate through the cells rendered in terraingenerator
        
        // possibly create a new class that identify production time remaining, etc
        // how do we cut down trees without the terrain generator working? should each Port be a gameobject? probably
        public Port(Vector3Int inputCell, bool friendly = false)
        {
            cell = inputCell;
            pirateFriendly = friendly;
            paths = new Dictionary<Port, Path>();
            ports.Add(this);
        }

        static Port()
        {
            nameMap = NameGenerator.LoadFromFile("portnames.json");
        }

        static int ct = 0;
        public IEnumerator coPathToPorts(int portnum)
        {
            print(ports.Count);
            paths = new Dictionary<Port, Path>();
            print(paths);
            List<Coroutine> co = new List<Coroutine>();
            foreach (Port port in ports.ToList())
            {
                if (port == this) continue;
                if (dockCell == null) print("No dockCell?");
                if (port.dockCell == null) print("No dockCell on other port?");
                if (port != null)
                {
                    Port.ct++;
                    co.Add(StartCoroutine(terrainGenerator.coAStar(port.dockCell, dockCell, (Path p) =>
                    {
                        if (p != null)
                        {
                            p.UpdateArray();
                            paths.Add(port, p);
                            GameManager.Instance.loadingBar.UpdateSubBar(Port.ct, (ports.Count - 1) * (ports.Count));
                        }
                    }, count: 5000, port: portnum)));
                    //paths.Add(port, terrainGenerator.AStar(port.dockCell, dockCell, count: 500));
                    //if (paths[port] == null || paths[port].currentNode == null) paths.Remove(port);
                    yield return null;
                    // yield return co[co.Count - 1];
                }
            }
            foreach (Coroutine cor in co)
            {
                yield return cor;
            }
            try
            {
                DeployTradeShip(paths.Keys.ToArray()[(int)UnityEngine.Random.Range(0, paths.Values.Count - 1)]);
            } catch (Exception e)
            {
                print(e);
            }
        }

        public void DeployTradeShip(Port destination)
        {
            print("Deploying trade ship");
            if (paths.ContainsKey(destination) && paths[destination].currentNode != null && paths[destination].currentNode.prior != null)
            {
                GameObject ship = Instantiate(shipPrefab, shipsContainer);
                EnemyShipController1 controller = ship.GetComponent<EnemyShipController1>();
                if (controller == null)
                {
                    print("why is controller null??");
                }
                // controller.terrainGenerator = terrainGenerator;

                if (controller.ship == null)
                {
                    print("controller's ship is null? strange...");
                }
                controller.ship.homePort = name;
                controller.ship.destinationPort = destination.name;
                ship.transform.position = SystemsManager.Instance.terrainGenerator.CellToWorld(dockCell); // paths[destination].currentNode.position;//terrainGenerator.CellToWorld(paths[destination].currentNode.cell);
                controller.StartPath(paths[destination]);
                //controller.path = paths[destination];//.startNode
                //controller.currentPathNode = 0;
            }
        }

        static Dictionary<int3, bool> namesGenerated = new Dictionary<int3, bool>();


        public void GeneratePortInfo()
        {
            name = NameGenerator.GenerateName(new float2(transform.position.x, transform.position.y), nameMap);
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            IBoards player = collision.GetComponent<IBoards>();
            if (player != null)
            {
                player.EnteredRadius(gameObject);
            }
            /*PlayerShipController1 controller = collision.GetComponent<PlayerShipController1>();
            if (controller != null)
            {
                controller.BoardPort = Board;
                SystemsManager.SetHint("at " + name + "\nPress space to enter");
            }*/
        }

        public void Board(Ship ship)
        {
            print("Ship would be boarded");
        }

        /*int countdownToAnother = 50;
        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.gameObject.name == "PlayerShip") { }
            else
            {
                EnemyShipController controller = collision.gameObject.GetComponent<EnemyShipController>();
                if (controller == null) return;
                if (controller.target == null || controller.target.cell == dockCell || (controller.target.prior != null && controller.target.prior.cell == dockCell))
                {
                    // countdownToAnother = (countdownToAnother + 1) % 150;
                    // if (controller.target.cell == dockCell && countdownToAnother == 0) DeployTradeShip(paths.Keys.ToArray()[(int)UnityEngine.Random.Range(0, paths.Keys.Count)]);
                    do
                    {
                        if (paths == null) return;
                        List<Path> list = paths.Values.ToList();
                        if (list.Count <= 0) return;
                        controller.target = list[
                            (int)UnityEngine.Random.Range(0, list.Count - 1)
                        ].currentNode;
                        controller.targetPos = terrainGenerator.CellToWorld(controller.target.cell);
                    }
                    while (controller.target == null || controller.target.prior == null);
                }
            }
        }*/

        private void OnTriggerExit2D(Collider2D collision)
        {
            IBoards player = collision.GetComponent<IBoards>();
            if (player != null)
            {
                player.LeftRadius(gameObject);
            }
            /*PlayerShipController1 controller = collision.GetComponent<PlayerShipController1>();
            if (controller != null)
            {
                controller.BoardPort = null;
                SystemsManager.UnsetHint("at " + name + "\nPress space to enter");
            }*/
        }

        public void OnBeforeSerialize()
        {
            //throw new NotImplementedException();
        }

        public void OnAfterDeserialize()
        {
            //throw new NotImplementedException();
        }
    }
}
