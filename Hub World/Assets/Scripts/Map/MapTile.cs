using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Map
{
    /**
    * Map-Tile
    * @author cgt102461: Nicolas Begic
    */
    public class MapTile : TileBase
    {
        public Sprite m_Sprite;
        public GameObject m_Prefab;

        //Bestimmt, ob dieses Tile auf der Tilemap blockiert ist
        public bool isBlocked { get; set; }

        /**
        * Überschreibt die GetTileData-Funktion aus der TileBase-Klasse
        * und ändert den Sprite und das Prefab des Tiles um auf die 
        * global gespeicherten Werte. 
        */
        public override void GetTileData(Vector3Int location, ITilemap tilemap, ref TileData tileData)
        {
            tileData.sprite = m_Sprite;
            tileData.gameObject = m_Prefab;
        }
    }
}
