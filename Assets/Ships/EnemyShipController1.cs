using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics;
using static UnityEngine.GraphicsBuffer;
using UnityEngine;
using Assets.Logic;
using Unity.VisualScripting;

namespace Assets.Ships
{
    [Serializable]
    public class EnemyShipController1 : ShipController1
    {
        static NameMap namesMap;
        public Vector2 targetDir;

        //public PathNode target;
        [DoNotSerialize]
        public Path path;
        public int currentPathNode;
        public Vector3 targetPos;
        public float targetDepth;

        static EnemyShipController1()
        {
            namesMap = NameGenerator.LoadFromFile("shipnames.json");
        }

        public override void Start()
        {
            base.Start();
            ship.name = NameGenerator.GenerateName(new float2(transform.position.x, transform.position.y), namesMap);
            gameObject.name = ship.name;
        }

        bool FightOrFlight()
        {
            if (ship.attacker == null) return false; // Why fight nothing?

            float armormentReadiness;
            int cannonballsNeeded = (int)(ship.attacker.health / 2); // cannonballs needed determined by number of cannonballs to deplete attacking's health
            if (cannonballsNeeded > 0)
            {
                armormentReadiness = ship.cargo.quantities[ResourceType.CannonBalls] / cannonballsNeeded; // if >1, we have more than enough cannonballs to take it out
                if (armormentReadiness < 1) return false;
            }

            float healthAdvantage = 1;
            if (ship.attacker.health > 0)
                healthAdvantage = ship.health / ship.attacker.health; // determine the health advantage

            // float fireRateAdvantage = attacking.CalculateFirePeriod() / CalculateFirePeriod(); // determine whether we can shoot faster. commented out because it's the same as healthAdvantage right now

            float solution = healthAdvantage; // * fireRateAdvantage; // simplest way to put all these factors  together. may need to add/adjust coefficients and constants
            if (solution > 0.5f) return true;
            return false;
        }

        public Vector2 direction;
        void MoveToTarget(Vector2 targetPos3, float depth)
        {
            if (depth != 0)
            {
                targetDepth = depth;
            }

            // rotate the ship to look at the target
            direction = new Vector2(0, 0);
            Vector2 targetPos = new Vector2(targetPos3.x, targetPos3.y);
            Vector2 thisPos2 = new Vector2(ship.transform.position.x, ship.transform.position.y);
            targetDir = (targetPos - thisPos2);
            float targetRot = math.atan2(targetPos.x - ship.transform.position.x, targetPos.y - ship.transform.position.y) * 180 / math.PI;
            float yMultiplier = 1;

            // turn horizontal and shoot at attackers if it's a good idea
            bool shouldFight = FightOrFlight();
            if (shouldFight && targetDir.magnitude < 4f && ship.sinkage < 0.1f)
            {
                targetRot = (targetRot + 90) % 360;
                yMultiplier = 0.1f;
            }
            else if (!shouldFight && ship.attacker != null)
            {
                ship.attacker = null;
            }

            // actually rotate and move
            float offRot = (-targetRot - ship.transform.rotation.eulerAngles.z) % 360;
            offRot = (offRot + 360) % 360;

            if (offRot <= 180 && offRot > 10)// && Math.Abs(rb2D.angularVelocity) > 30)
            {
                direction.x += 1;
            }
            else if (offRot > 180 && offRot < 350)// && Math.Abs(rb2D.angularVelocity) <= 30)
            {
                direction.x -= 1;
            }
            else
            {
                if (ship.attacker != null && targetDir.magnitude < 4f)
                    ship.FireCannons(ship.portsideCannons);
            }

            direction.y = ((math.abs(180 - offRot) / 180f) * targetDepth * 2 + 0.2f) * yMultiplier; //(target.depth * 0.3f + 0.1f); //(360 - math.abs(offRot)) / 360f;

            // if (direction.y < 0.001f) direction.y = 0;
            // if (direction.x < 0.001f) direction.x = 0;

            try
            {
                ship.ApplyForce(direction, Time.deltaTime);
            } catch(Exception e)
            {
                print(direction);
            }
        }

        void MoveToCell(Vector3Int targetCell, float depth)
        {
            if (targetCell == null || SystemsManager.Instance.terrainGenerator == null) return;
            MoveToTarget(SystemsManager.Instance.terrainGenerator.CellToWorld(targetCell), depth);
        }

        void MoveToNode(PathNode node)
        {
            if (node != null && node.cell != null)
                MoveToCell(node.cell, node.depth);
            else
                MoveToTarget(transform.position, 0.05f);
        }

        void ManagePath()
        {
            if (targetDir.magnitude < 2.56f)
            {
                if (/*target.prior == null*/ currentPathNode < path.nodes.Count && ship.attacker != null)
                {
                    targetPos = ship.attacker.transform.position;
                }
                /*if (/*target == null*/ /*currentPathNode == path.nodes.Count) return;
                /*PathNode next = path.nodes[currentPathNode + 1];//target.prior;
                if (next != null)
                {
                    target = next;
                }
                else
                {
                    target = null;
                }*/

                if (currentPathNode + 1 < path.nodes.Count)
                {
                    currentPathNode++;
                    targetDepth = path.nodes[currentPathNode].depth;
                } else
                {
                    path = null;
                }
            }
        }

        public void StartPath(Path path)
        {
            currentPathNode = 0;
            this.path = path;
        }

        private void Update()
        {
            if (ship.attacker != null)
            {
                targetPos = ship.attacker.transform.position;
                targetDepth = 0.5f;
                if (targetPos != null)
                    MoveToTarget(targetPos, 0f);
            }
            else if (path != null && currentPathNode < path.nodes.Count)//(target != null)
            {
                MoveToNode(path.nodes[currentPathNode]);//target);
                ManagePath();
            }
        }
    }
}
