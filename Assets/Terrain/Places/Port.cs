using Assets.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

namespace Assets
{
    public class Port : MonoBehaviour
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
        public List<EnemyShipController> ships;
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

        public void PathToPorts()
        {
            print(ports.Count);
            paths = new Dictionary<Port, Path>();
            print(paths);
            foreach (Port port in ports)
            {
                if (port == this) continue;
                if (dockCell == null) print("No dockCell?");
                if (port.dockCell == null) print("No dockCell on other port?");
                if (port != null)
                {
                    paths.Add(port, terrainGenerator.AStar(port.dockCell, dockCell, count: 25000));
                    if (paths[port] == null || paths[port].currentNode == null) paths.Remove(port);
                }

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
                EnemyShipController controller = ship.GetComponent<EnemyShipController>();
                controller.terrainGenerator = terrainGenerator;

                controller.homePort = this;
                controller.fromPort = destination;
                ship.transform.position = terrainGenerator.CellToWorld(paths[destination].currentNode.cell);
                controller.target = paths[destination].currentNode;
            }
        }

        static Dictionary<int3, bool> namesGenerated = new Dictionary<int3, bool>();


        public void GeneratePortInfo()
        {
            name = NameGenerator.GenerateName(new float2(transform.position.x, transform.position.y), nameMap);
            //GenerateName();
        }
        /*static string[] prefixes = { 
            "Port", "Fort", "Camp"
        };
        static string[] suffixes = {
            "Cove", "Respite", "Bay", "Castle", "City", "Coast", "Hideout", "Capital", "Seaport", "Harbor", "Haven", "Marina"
        };
        static string[] names = {
            "Sandy", "Sponge", "Starfish", "Stink", "Chip", "Deadman", "Junior", "Drowning", "Balloon", 
            "Computer", "Microprocessor", "Pyramid Scheme", "Echo", "Tacky", "Bludgeon", "Request Timeout",
            "Bond", "Michael", "Skeleton", "Winter", "Liberty", "E", "Chocolate", "Nite", "Knight", "Blueberry",
            "Banana", "Hamburger", "Automobile", "Microwave Oven", "Chicken", "Yugoslavia", "Slovenia", "Serbia",
            "North Macedonia", "Greece", "Juice", "Blooper", "Sludge", "Muck", "Name", "Legend", "User Interface",
            "Minced", "Grater", "Cheese", "Bling", "Argument", "Photograph", "Trades-a-lot", "Video Gamer",
            "Jim", "Watery", "Hotdog", "Filler", "Casino", "Slots Machine", "Roulette Wheel", "Imagination",
            "Happiness", "Jump", "Blind", "Ex-Wife", "Landlock", "Dishonesty", "Smidgen", "Copyright Law",
            "Strawberry", "Fisherman", "Useless", "Irate", "Danger", "Pepper", "Slime", "Styrofoam", "Jeepers Creepers",
            "I can't come up with a name", "Wrestling Champ", "Critical Acclaim", "Judicial", "Low Battery",
            "Unplayable", "Ten Frames", "Procedural", "Jubwub", "Down by the", "Hairier", "er, Harry", "Parrot",
            "Requirements", "Scrum", "Agile", "Craft", "Blub", "Soda", "Caffeine", "Ceramic", "Whammy", "Rock",
            "Boulder", "Market", "Cohort", "Short", "Turkey Dinner", "Mac'n'Cheese", "Discovery", "Poison",
            "Hotwire", "Intermission", "Marco", "Polo", "Lost", "Apprehensive", "Silly Billy", "Retail Store",
            "Station Wagon", "Zumzazoo", "Thingamabob", "Whozeewhatsit", "Whirligig", "Nick-Nack", "Witch",
            "Whatchamacallit", "Plasma Television", "Air Circulation", "Leftovers", ""
        };*/
        public void GenerateName()
        {
            float noiseValue = SeededRandom.RangeFloat(new float2(transform.position.x, transform.position.y), 0, 1);//math.abs(noise.snoise(new float2(i * 100, seed * 1000)));
            // print(noiseValue);
            nameAssembly = new int3();

            // Select the default name
            nameAssembly[2] = (int)(noiseValue * 2); // whether prefix (1) or suffix (0)
            if (nameAssembly[2] == 1)
            {
                nameAssembly[0] = (int)(noiseValue * portNames.prefixes.Count); // prefix;
            } else
            {
                nameAssembly[0] = (int)(noiseValue * portNames.suffixes.Count); // suffix
            }
            nameAssembly[1] = (int)(noiseValue * portNames.names.Count); // name

            // Iterate main part until name is unique OR we loop back to where we started
            int og = nameAssembly[1];
            while (namesGenerated.ContainsKey(nameAssembly) && (nameAssembly[1] + 1 != og)) nameAssembly[1] = (nameAssembly[1] + 1) % portNames.names.Count;

            // Iterate -fix until the name is unique OR we loop back to where we started
            og = nameAssembly[0];
            int limit = (nameAssembly[2] == 1) ? portNames.prefixes.Count : portNames.suffixes.Count;
            while (namesGenerated.ContainsKey(nameAssembly) && (nameAssembly[0] + 1 != og)) nameAssembly[0] = (nameAssembly[0] + 1) % limit;

            // Sad
            if (namesGenerated.ContainsKey(nameAssembly))
            {
                // print("Failed to make a unique name for " + i);
            }
            namesGenerated[nameAssembly] = true;

            // Convert name to string
            if (nameAssembly[2] == 1)
            {
                name = portNames.prefixes[nameAssembly[0]] + " " + portNames.names[nameAssembly[1]];
            } else
            {
                name = portNames.names[nameAssembly[1]] + " " + portNames.suffixes[nameAssembly[0]];
            }
            gameObject.name = name;

        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.name == "PlayerShip")
            {
                uiText.text = "at " + name;
                DeployTradeShip(paths.Keys.ToArray()[(int)UnityEngine.Random.Range(0, paths.Keys.Count)]);
            }
            //else
            //{
            //}
        }

        int countdownToAnother = 50;
        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.gameObject.name == "PlayerShip") { }
            else
            {
                EnemyShipController controller = collision.gameObject.GetComponent<EnemyShipController>();
                if (controller == null) return;
                if (controller.target == null || controller.target.cell == dockCell || (controller.target.prior != null && controller.target.prior.cell == dockCell))
                {
                    countdownToAnother = (countdownToAnother + 1) % 150;
                    if (controller.target.cell == dockCell && countdownToAnother == 0) DeployTradeShip(paths.Keys.ToArray()[(int)UnityEngine.Random.Range(0, paths.Keys.Count)]);
                    do
                        collision.GetComponent<EnemyShipController>().target = paths.Values.ToArray()[
                            (int)UnityEngine.Random.Range(0, paths.Values.Count)
                        ].currentNode;
                    while (controller.target == null || controller.target.prior == null);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.name == "PlayerShip")
                uiText.text = "";
        }
    }
}
