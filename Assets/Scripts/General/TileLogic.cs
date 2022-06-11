using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using TMPro;

public class TileLogic : MonoBehaviour
{
    public Tilemap logicTiles;
    public Sprite breakableTile;
    public static TileLogic instance;
   // public bool trigger;

    private void Awake()
    {
        instance = this;
        //trigger = false;
    }
    public static void TriggerTileLogic(Transform transformTarget)
    {
        Vector3 targetV3 = transformTarget.TransformPoint(Vector3.up);
        Vector3Int targetV3Int = instance.logicTiles.WorldToCell(targetV3);

        if (instance.logicTiles.HasTile(targetV3Int))
        {
            if (instance.logicTiles.GetSprite(targetV3Int).Equals(instance.breakableTile))
            {
                instance.logicTiles.SetTile(targetV3Int, null);
                //  Debug.Log("same tile");
            }
            //else if (instance.logicTiles.GetSprite(targetV3Int).Equals(instance.coinTile)&&instance.trigger==true)
            //{
            //    instance.trigger = false;
            //    instance.Coins++;
            //    instance.coinText.text = instance.Coins.ToString();
            // //   Debug.Log("coin Tile");
            //}

        }
    }

    
}
