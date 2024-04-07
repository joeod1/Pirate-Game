using Assets.PlatformerFolder;
using Assets.PlatformerFolder.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class NonPlayerController : SideController
{
    public GameObject target;

    public bool patroling = false;
    public bool reachedLeft = false;
    public bool reachedRight = false;
    public bool ladderBound = false;

    public LadderMap ladders;

    public override void Start()
    {
        base.Start();
    }

    private void MoveToPosition(Vector3 position, float distance = 0.1f)
    {
        // Handle (or not) if we're on a different level
        float floorDiff = (position.y) - (character.transform.position.y);
        int flooredFloorDiff = (int)(position.y / 3) - (int)(character.transform.position.y / 3);
        if (flooredFloorDiff != 0 || (character.onLadder > 0 && Math.Abs(floorDiff) > 0.05f))
        {
            if (character.onLadder > 0)
            {
                if (floorDiff > 0) character.ClimbLadder(300f);
                else character.ClimbLadder(-300f);
            }

            float value;
            if (flooredFloorDiff > 0) value = ladders.Get(character.transform.position.y);
            else value = ladders.Get(character.transform.position.y - 3);

            if (value != -1 && !ladderBound)
            {
                ladderBound = true;
                MoveToPosition(new Vector2(value, character.transform.position.y));
                ladderBound = false;
            }
            return;
        }


        if (Math.Abs(position.x - character.transform.position.x) < distance) {
            character.Walk(0);
            return;
        }

        if (position.x > character.transform.position.x)
        {
            character.Walk(0.5f);
        } else
        {
            character.Walk(-0.5f);
        }
    }

    private void MoveToTarget()
    {
        if (target != null)
        {
            Vector3 targetPosition = target.transform.position;
            MoveToPosition(targetPosition, 1.6f);
        }
    }

    private void Update()
    {
        MoveToTarget();
    }
}

