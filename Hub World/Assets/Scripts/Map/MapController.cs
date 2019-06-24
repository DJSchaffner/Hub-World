using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Map{
    public class MapController : MonoBehaviour
    {
        private const int MAP_SIZE = 50;

        public GameObject GrassTile;
        public GameObject BlockedTile;
        private Tilemap[] map;
        private MapTile blockedTile;

        void Start()
        {
            map = GetComponentsInChildren<Tilemap>();
            MapTile grassTile = ScriptableObject.CreateInstance<MapTile>();
            grassTile.m_Sprite = GrassTile.GetComponent<SpriteRenderer>().sprite;
            grassTile.m_Prefab = GrassTile;

            blockedTile = ScriptableObject.CreateInstance<MapTile>();
            blockedTile.m_Sprite = BlockedTile.GetComponent<SpriteRenderer>().sprite;
            blockedTile.m_Prefab = BlockedTile;

            //InitMap(grassTile);
        }

        private void InitMap(MapTile tile)
        {
            for (int y = -MAP_SIZE; y < MAP_SIZE; y++)
            {
                for (int x = -MAP_SIZE; x < MAP_SIZE; x++)
                {
                    map[0].SetTile(new Vector3Int(x, y, 0), tile);
                }
            }
        }

        public void PlaceObject(int xCenter, int yCenter, PolygonCollider2D placedObject)
        {
            float sizeX = placedObject.bounds.extents.x;
            float sizeY = placedObject.bounds.extents.y;

            for (int y = (int)(yCenter - sizeY); y < (yCenter + sizeY); y++)
            {
                for (int x = (int)(xCenter - sizeX); x < (xCenter + sizeX); x++)
                {
                    if (placedObject.bounds.Contains(map[0].GetCellCenterWorld(new Vector3Int(x, y, 0))))
                    {
                        map[0].SetTile(new Vector3Int(x, y, 0), blockedTile);
                    }
                }
            }
        }

    }
}
