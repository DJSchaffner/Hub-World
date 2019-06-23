using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Map
{
    public class MapTile : TileBase
    {
        public Sprite m_Sprite;
        public GameObject m_Prefab;

        public override void GetTileData(Vector3Int location, ITilemap tilemap, ref TileData tileData)
        {
            tileData.sprite = m_Sprite;
            tileData.gameObject = m_Prefab;
        }

    }
}
