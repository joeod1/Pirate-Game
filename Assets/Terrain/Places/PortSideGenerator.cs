using Assets;
using Assets.Logic;
using Assets.PlatformerFolder;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortSideGenerator : MonoBehaviour
{
    public PlatformerPalette palette;

    public void coGeneratePort(Port target)
    {
        if (palette == null)
        {
            print("Palette is null :/");
        }
        if (palette.playerPrefab == null)
        {
            print("Player prefab is null :/");
        }
        palette.playerPrefab.transform.localPosition = new Vector3(2, 3);

        palette.sunLighting.gameObject.SetActive(true);

        palette.water.transform.localScale = new Vector3(1000, 5, 1);
        palette.water.transform.localPosition = new Vector3(0, -2.5f, 1);

        for (int x = 0; x < 100; x++) {
            palette.groundTilemap.SetTile(new Vector3Int(x, 0), palette.groundTop);
            for (int y = -1; y > -10; y--)
            {
                palette.groundTilemap.SetTile(new Vector3Int(x, y), palette.groundMiddle);
            }
        }

        GameObject weaponStand = Instantiate(palette.weaponStand, palette.partsContainer.transform);
        weaponStand.transform.localPosition = new Vector3(5f, 2.4f);

        GameObject foodStand = Instantiate(palette.foodStand, palette.partsContainer.transform);
        foodStand.transform.localPosition = new Vector3(11f, 0.8f);

        GameObject dock = Instantiate(palette.dock, palette.partsContainer.transform);
        dock.transform.localPosition = new Vector3(0, 0);

        foreach (NonPlayerController ctrl in palette.partsContainer.GetComponentsInChildren<NonPlayerController>())
        {
            ctrl.homePort = target;
        }
    }
}
