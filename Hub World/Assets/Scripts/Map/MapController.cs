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
        private Grid grid;
        private MapTile blockedTile;

        void Start()
        {
            map = GetComponentsInChildren<Tilemap>();
            grid = GetComponentInParent<Grid>();
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

        public void PlaceObject(int xCenter, int yCenter, bool[,] tileArray)
        {
            float sizeX = tileArray.GetLength(0);
            float sizeY = tileArray.GetLength(1);

            for (int y = 0; y < sizeY ; y++)
            {
                for (int x = 0; x < sizeX; x++)
                {
                    if (tileArray[x, y])
                    {
                        map[0].SetTile(new Vector3Int((int)(xCenter - sizeX/2) + x, (int)(yCenter - sizeY/2) + y, 0), blockedTile);
                    }
                }
            }
        }
    }
}
