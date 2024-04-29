using Assets.Logic;
using Assets.Resources;
using Assets.Ships;
using Assets.Terrain.Places;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets
{

    [Serializable]
    public class Port : MonoBehaviour, ISerializationCallbackReceiver, ITradeable
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
        public Dictionary<string, float> pirateRelations = new Dictionary<string, float>();
        public int citizens = 1000; // number of total citizens
        public int residents = 0; // number of citizens located within the port
        public float defenseNeed = 0; // 0 = no cannons, defense vessels; 1 = many cannons, lots of defense vessels per trade vessel
        public float shipNeed = 0; // 0 = no ship production, resources saved (can be traded); 1 = high ship production, resources diverted
        public int shipsOwned = 0;
        public TradeResources resources = new TradeResources(); // the resources that the port has
        public TradeResources productions = new TradeResources(); // the resources that are produced by the port
        public TradeResources consumption = new TradeResources(); // the resources that are produced by the port
        public TradeResources needs = new TradeResources(); // needed resources
        public TradeResources fetchingNeeds = new TradeResources(); // needed resources
        public List<TradeDeal> dealsAwaitingShip = new List<TradeDeal>();

        [Header("Other Ports")]
        static public List<Port> ports = new List<Port>();
        public Dictionary<Port, Path> paths = new Dictionary<Port, Path>();

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
        public List<EnemyShipController1> awaitingDispatch = new List<EnemyShipController1>();

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
                FetchNeeds(true);
                //DeployTradeShip(paths.Keys.ToArray()[(int)UnityEngine.Random.Range(0, paths.Values.Count - 1)]);
            } catch (Exception e)
            {
                print(e);
            }
        }

        public Ship DeployTradeShip(Port destination)
        {
            print("Deploying trade ship");
            if (destination != null && paths != null && paths.ContainsKey(destination) && paths[destination] != null && paths[destination].currentNode != null && paths[destination].currentNode.prior != null)
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

                controller.ship.cannonCount = (int)(defenseNeed * 10f) + 2;

                controller.ship.OnSank += () =>
                {
                    defenseNeed += 0.2f;
                    FetchNeeds(true);
                };
                //controller.path = paths[destination];//.startNode
                //controller.currentPathNode = 0;
                return controller.ship;
            }
            return null;
        }

        static Dictionary<int3, bool> namesGenerated = new Dictionary<int3, bool>();


        public void GeneratePortInfo()
        {
            name = NameGenerator.GenerateName(new float2(transform.position.x, transform.position.y), nameMap);
        }

        public void CalculateNeeds()
        {
            // Port consumptions
            consumption.quantities[ResourceType.Food] = citizens / 100f;
            consumption.quantities[ResourceType.Water] = citizens / 100f;
            consumption.quantities[ResourceType.Oranges] = citizens / 1000f;
            consumption.quantities[ResourceType.Gold] = citizens / 1000f;
            consumption.quantities[ResourceType.Wood] = citizens / 1000f;
            consumption.quantities[ResourceType.Drink] = citizens / 1000f;
            foreach (KeyValuePair<ResourceType, float> resource in consumption.quantities.ToList())
            {
                float has = resources.quantities[resource.Key];
                //if (resource.Value > has / 25f)  // If the port is consuming more than half of what it has
                //{
                    // Shrink the rations
                //    consumption.quantities[resource.Key] /= 2f;

                    // Decide to get more resourrces from other ports
                    needs.quantities[resource.Key] = resource.Value / has * 5;
                //}
            }

            // Ship needs
            float relations = 0;
            /*foreach (KeyValuePair<string, float> relation in pirateRelations)
            {
                relations += relation.Value;
            }
            relations /= pirateRelations.Count;*/

            needs.quantities[ResourceType.CannonBalls] = -relations;

            float totalNeedWeight = 0;
            foreach (KeyValuePair<ResourceType, float> resource in needs.quantities)
            {
                totalNeedWeight += TradeResources.WeightFromQuantity(resource.Key, resource.Value);
            }
            float shipCapacity = shipsOwned;
            shipNeed = (totalNeedWeight / 500f) - shipCapacity;
        }

        public int CalculateBarter(ResourceType offer, ResourceType take)
        {
            return (int)Math.Abs(Math.Ceiling((resources.quantities[take] + 0.1f) / (resources.quantities[offer] + 0.1f)));
        }

        public void ProduceResources()
        {
            foreach (KeyValuePair<ResourceType, float> prod in productions.quantities)
            {
                resources.quantities[prod.Key] += prod.Value * Time.deltaTime;
            }
        }

        public void ConsumeResources()
        {
            foreach (KeyValuePair<ResourceType, float> prod in consumption.quantities)
            {
                resources.quantities[prod.Key] -= prod.Value * Time.deltaTime;
            }
        }

        public void FetchNeeds(bool normalShipment = false)
        {
            float tmp = -1;
            if (normalShipment)
            {
                tmp = shipNeed;
                shipNeed = 1;
            }

            List<KeyValuePair<ResourceType, float>> sortedNeeds = needs.quantities.ToList();
            sortedNeeds.Sort((two, one) => { 
                return (int)((one.Value - fetchingNeeds.quantities[one.Key]) - (two.Value - fetchingNeeds.quantities[two.Key]));
            });

            KeyValuePair<ResourceType,float> need = sortedNeeds.ElementAt(0);

            //foreach (KeyValuePair<ResourceType, float> need in sortedNeeds)
            //{
                // Skip if we're already fetching all these resources
            if (need.Value - fetchingNeeds.quantities[need.Key] <= 0.25f && !normalShipment) return;
            print("Top need: " + need.Value + ", bottom need: " + sortedNeeds[sortedNeeds.Count - 1].Value);

            List<Port> sortedPorts = new List<Port>(ports);
            sortedPorts.Sort((two, one) =>
            {
                return (int)(one.needs.quantities[need.Key] - two.needs.quantities[need.Key]);
            });

            int i = 0;
            Port targetPort = null;
            if (paths == null) return;
            do
            {
                targetPort = sortedPorts[i];
                i++;
            } while ((targetPort == null || paths == null || !paths.ContainsKey(targetPort) || paths[targetPort] == null || paths[targetPort].currentNode == null || paths[targetPort].currentNode.prior == null) && i < paths.Count);

            if ((targetPort == null || paths == null || !paths.ContainsKey(targetPort) || paths[targetPort] == null || paths[targetPort].currentNode == null || paths[targetPort].currentNode.prior == null)) return;

            TradeDeal tradeDeal = new TradeDeal();
            tradeDeal.homePt = gameObject;
            tradeDeal.exchangePt = targetPort.gameObject;

            tradeDeal.exchange = sortedNeeds[sortedNeeds.Count - 1].Key;
            tradeDeal.take = need.Key;
            float rate = targetPort.CalculateBarter(tradeDeal.exchange, need.Key);
            tradeDeal.exchangeQt = (int)need.Value * 100;
            tradeDeal.takeQt = (int)(rate * need.Value) * 100; // We'll just assume this deal is possible. No deadlocks, please :)

            if (awaitingDispatch.Count > 0)
            {
                EnemyShipController1 ship = awaitingDispatch[0];
                awaitingDispatch.Remove(ship);
                resources.quantities[tradeDeal.exchange] -= tradeDeal.exchangeQt;
                ship.ship.cargo.quantities[tradeDeal.exchange] += tradeDeal.exchangeQt;
                //ship.gameObject.SetActive(true);
                ship.StartPath(paths[tradeDeal.exchangePt.GetComponent<Port>()]);
            }


            if (shipNeed <= 0 && dealsAwaitingShip != null)  // && dealsAwaitingShip.Count > 0)
            {
                dealsAwaitingShip.Add(tradeDeal);
            } else if (shipNeed > 0)
            {
                shipsOwned++;
                if (shipsOwned < 10)
                {
                    Ship ship = DeployTradeShip(targetPort);
                    ship.tradeDeal = tradeDeal;

                    resources.quantities[tradeDeal.exchange] -= tradeDeal.exchangeQt;
                    ship.cargo.quantities[tradeDeal.exchange] += tradeDeal.exchangeQt;
                    if (ship.cargo.quantities[tradeDeal.take] < 0) print("Quantities made 0 when initially dispatching");
                }
            }


            fetchingNeeds.quantities[tradeDeal.take] += tradeDeal.takeQt;
            //}

            if (normalShipment)
            {
                shipNeed = tmp;
            }
        }

        public void MaintainPort()
        {
            ProduceResources();
            CalculateNeeds();
            ConsumeResources();
            FetchNeeds();
        }

        private WaitForSeconds time;
        public IEnumerator coNormalFetch()
        {
            while (true)
            {
                if (shipsOwned < 10)
                    FetchNeeds(true);
                yield return time;
            }
        }

        private void Awake()
        {
            //time = new WaitForSeconds(10);
            //StartCoroutine(coNormalFetch());
        }

        private void Update()
        {
            MaintainPort();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            EnemyShipController1 ship = collision.GetComponent<EnemyShipController1>();
            if (ship != null)
            {
                TradeDeal trade = ship.ship.tradeDeal;
                if (trade == null) return;

                if (trade.exchangePt == gameObject && trade.finished == false)
                {
                    trade.finished = true;
                    resources.quantities[trade.exchange] += trade.exchangeQt;
                    resources.quantities[trade.take] -= trade.takeQt;
                    ship.ship.cargo.quantities[trade.exchange] -= trade.exchangeQt;
                    ship.ship.cargo.quantities[trade.take] += trade.takeQt;
                    if (ship.ship.cargo.quantities[trade.take] < 0) print("Quantities made 0 when returning from exhange port");
                    ship.pathDir = -1;
                    // ship.path = paths[trade.homePt.GetComponent<Port>()]; //StartPath(paths[trade.homePt.GetComponent<Port>()]);
                    // ship.currentPathNode = 0;
                    print("Headed elsewhere now");
                }
                else
                {
                    if (trade.homePt != gameObject || !trade.finished) return;
                    resources.quantities[trade.take] += trade.takeQt;
                    ship.ship.cargo.quantities[trade.take] -= trade.takeQt;
                    fetchingNeeds.quantities[trade.take] -= trade.takeQt;
                    if (ship.ship.cargo.quantities[trade.take] < 0) print("Quantities made 0 when arriving back home");

                    if (dealsAwaitingShip != null && dealsAwaitingShip.Count > 0)
                    {
                        print("Dispatched a ship");
                        TradeDeal newDeal = dealsAwaitingShip[0];
                        dealsAwaitingShip.Remove(newDeal);
                        ship.ship.cargo.quantities[newDeal.exchange] += newDeal.exchangeQt;

                        ship.ship.tradeDeal = newDeal;
                        ship.StartPath(paths[newDeal.exchangePt.GetComponent<Port>()]);
                    } else
                    {
                        //ship.gameObject.SetActive(false);
                        awaitingDispatch.Add(ship);
                    }
                }
            }
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

        public void Trade(Ship ship, TradeDeal deal)
        {
            // throw new NotImplementedException();
        }

        public void InRadius(Ship ship)
        {
            // throw new NotImplementedException();
        }
    }
}
