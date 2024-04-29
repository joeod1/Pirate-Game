using Assets.Logic;
using Assets.Terrain;
using Assets.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Ships
{
    //public delegate void ();
    [Serializable]
    public class PlayerShipController1 : ShipController1, IBoards
    {
        // quick fix, shouldn't be overused
        public static PlayerShipController1 Instance;

        InputAction turnAction;
        InputAction accelerateAction;
        InputAction boardAction;
        InputAction shootLeftAction;
        InputAction shootRightAction;
        InputAction zoomAction;

        InputActionMap inputMap;
        public InputActionAsset actions;

        public Action<Ship> BoardPort;
        public TerrainZone terrainZone;
        public bool zoneUsed = false;
        public Camera cam;
        private bool chunkZoomRendered = false;

        public TargetSender BoardShipFunc;

        Ship boardableShip;
        Port boardablePort;

        bool controlled = false;

        public TMPro.TMP_Text healthText;
        public TMPro.TMP_Text goldText;

        public override void Start()
        {
            Instance = this;
            base.Start();

            UpdateHUD();

            ship.OnDamage += (float dmg) => { UpdateHUD(); } ;
            
            controlled = InitializeControls();

            terrainZone = new TerrainZone(new Vector2(0, 0), new Vector2Int(100, 100));
            if (zoneUsed == false && SystemsManager.Instance != null && SystemsManager.Instance.terrainGenerator != null) {
                SystemsManager.Instance.terrainGenerator.AddZone(terrainZone);
                SystemsManager.Instance.terrainGenerator.RenderZones();
                zoneUsed = true;
            }

            ship.OnSank += () =>
            {
                SystemsManager.SetHint("You succumb to the waves...");
                SystemsManager.FadeToBlack();

                StartCoroutine(coAnotherChance());
            };

            ship.cargo.quantities.OnChange += (ResourceType type) =>
            {
                TMPro.TMP_Text text = null;
                switch (type)
                {
                    case ResourceType.Wood:
                        text = InventoryUI.Instance.woodAmount; break;
                    case ResourceType.Drink:
                        text = InventoryUI.Instance.drinkAmount; break;
                    case ResourceType.Water:
                        text = InventoryUI.Instance.waterAmount; break;
                    case ResourceType.Food:
                        text = InventoryUI.Instance.foodAmount; break;
                    case ResourceType.CannonBalls:
                        text = InventoryUI.Instance.cannonballsAmount; break;
                    case ResourceType.Gold:
                        text = InventoryUI.Instance.goldAmount; break;
                }
                if (text == null) return;
                text.text = ship.cargo.quantities[type].ToString();
            };
        }

        public void UpdateHUD()
        {
            healthText.text = (int)ship.health + "/" + (int)ship.maxHealth;
            goldText.text = ship.cargo.quantities[ResourceType.Gold].ToString() + " GOLD";
        }

        public IEnumerator coAnotherChance()
        {
            yield return new WaitForSeconds(2);
            SystemsManager.SetHint("... A mermaid gives you one last chance");

            yield return new WaitForSeconds(2);
            SystemsManager.UnsetHint("... A mermaid gives you one last chance");
            ship.health = ship.maxHealth;
            ship.shipModel.transform.position -= new Vector3(0, 0, ship.sinkage);
            ship.sinkage = 0;
            ship.healthBar.SetActive(false);
            ship.showHealth = true;
            GetComponent<CapsuleCollider2D>().enabled = true;
            SystemsManager.FadeFromBlack();
        }

        public bool InitializeControls()
        {
            try
            {
                // Load and enable the topdown input map
                inputMap = actions.FindActionMap("TopDown");
                inputMap.Enable();

                // Load + enable inputs for controls
                turnAction = inputMap.FindAction("Turn");
                accelerateAction = inputMap.FindAction("Accelerate");
                boardAction = inputMap.FindAction("Board");
                shootLeftAction = inputMap.FindAction("LeftCannons");
                shootRightAction = inputMap.FindAction("RightCannons");
                zoomAction = inputMap.FindAction("Zoom");

                // Create a callback for boarding ships
                boardAction.performed += _ => AttemptBoard();

                // Create callbacks to fire cannons when the respective buttons are pressed
                shootLeftAction.performed += _ => ship.FireCannons(ship.portsideCannons);
                shootRightAction.performed += _ => ship.FireCannons(ship.starboardsideCannons);

                return true;
            } catch (Exception e)
            {
                print(e);
                return false;
            }
        }

        private void OnEnable()
        {
            if (inputMap != null)
            {
                inputMap.Enable();
            }
        }

        public void AttemptBoard()
        {
            // Try boarding a port first
            if (boardablePort != null && SystemsManager.DockPort != null)
            {
                SystemsManager.DockPort(boardablePort, ship);
                inputMap.Disable();
                return;
            }

            // Ensure the ship in question and the boarding function exist
            if (boardableShip != null && SystemsManager.BoardShip != null)
            {
                SystemsManager.BoardShip(boardableShip, ship);
                inputMap.Disable();
            }
        }

        public void ChangeSpeed()
        {
            Vector2 direction = new Vector2(0, 0);/*
            if (Input.GetKey(KeyCode.W)) direction.y = 2;
            if (Input.GetKey(KeyCode.A)) direction.x += 1.3f;
            if (Input.GetKey(KeyCode.D)) direction.x -= 1.3f;*/
            direction.y = 2 * accelerateAction.ReadValue<float>();
            direction.x = -1.3f * turnAction.ReadValue<float>();
            ship.ApplyForce(direction, Time.deltaTime);
            /*ship.ChangeVelocity(new UnityEngine.Vector2(
                turnAction.ReadValue<float>() * -25 * Time.deltaTime,
                accelerateAction.ReadValue<float>() * 10 * Time.deltaTime
                ));*/
        }

        private Vector3Int knownPosition = Vector3Int.zero;
        public void RenderTerrainIfNecessary()
        {
            Vector3Int currentPosition = SystemsManager.Instance.terrainGenerator.WorldToCell(transform.position);
            if ((currentPosition - knownPosition).magnitude > terrainZone.size.magnitude / 5f)
            {
                chunkZoomRendered = false;
                if (Input.GetKey(KeyCode.M))
                {
                    SystemsManager.Instance.terrainGenerator.ClearPlusRender(transform.position, new Vector2Int(200, 200));
                }
                else
                {
                    terrainZone.position = transform.position;
                    SystemsManager.Instance.terrainGenerator.RenderZones();  // ClearPlusRender(transform.position, renderBounds);
                }
                knownPosition = currentPosition;
            }
        }

        public void ManageCamera()
        {
            float zoom = zoomAction.ReadValue<float>();
            if (zoom != 0)
            {
                cam.orthographicSize += zoom / 2f;
                if (cam.orthographicSize < 2)
                {
                    cam.orthographicSize = 2;
                }
                else if (cam.orthographicSize > 50)
                {
                    cam.orthographicSize = 50;
                }
                terrainZone.Set(ship.transform.position, new Vector2Int((int)cam.orthographicSize * 4 + 10, (int)cam.orthographicSize * 3 + 10));
                if (!chunkZoomRendered)
                {
                    chunkZoomRendered = true;
                    SystemsManager.Instance.terrainGenerator.ClearPlusRender(transform.position, new Vector2Int(100, 100));
                }
            }

            cam.transform.position = new Vector3(ship.transform.position.x, ship.transform.position.y, -5);
        }

        private void Update()
        {
            if (!controlled) return;

            if (zoneUsed == false && SystemsManager.Instance != null && SystemsManager.Instance.terrainGenerator != null)
            {
                SystemsManager.Instance.terrainGenerator.AddZone(terrainZone);
                SystemsManager.Instance.terrainGenerator.RenderZones();
                zoneUsed = true;
            }


            ManageCamera();
            ChangeSpeed();
            RenderTerrainIfNecessary();
            //float x = turnAction.ReadValue<float>();
            //print(x);
        }

        public void EnteredRadius(GameObject other)
        {
            Ship otherShip = other.GetComponent<Ship>();
            if (otherShip != null)
            {
                if (boardableShip == otherShip) return;

                SystemsManager.SetHint(otherShip.name + "\npress space to board");
                boardableShip = otherShip;
                return;
            }

            Port otherPort = other.GetComponent<Port>();
            if (otherPort != null)
            {
                if (boardablePort == otherPort) return;

                SystemsManager.SetHint("at " + otherPort.name + "\npress space to dock");
                boardablePort = otherPort;
                return;
            }
        }

        public void LeftRadius(GameObject other)
        {
            Ship otherShip = other.GetComponent<Ship>();
            if (otherShip != null)
            {
                if (boardableShip == otherShip) boardableShip = null;
                SystemsManager.UnsetHint(otherShip.name + "\npress space to board");
                return;
            }

            Port otherPort = other.GetComponent<Port>();
            if (otherPort != null)
            {
                if (boardablePort == otherPort) boardablePort = null;
                SystemsManager.UnsetHint("at " + otherPort.name + "\npress space to dock");
                return;
            }
        }
    }
}
