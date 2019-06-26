using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum TileTypes { BlockedTile = 0, GrassTile}

namespace Map{
    public class MapController : MonoBehaviour
    {
        private const int MAP_SIZE = 50;

        public GameObject[] Tiles;

        private Tilemap[] map;
        private Grid grid;
        private MapTile blockedTile;

        void Start()
        {
            map = GetComponentsInChildren<Tilemap>();
            grid = GetComponentInParent<Grid>();
            MapTile grassTile = ScriptableObject.CreateInstance<MapTile>();
            grassTile.m_Sprite = Tiles[(int)TileTypes.GrassTile].GetComponent<SpriteRenderer>().sprite;
            grassTile.m_Prefab = Tiles[(int)TileTypes.GrassTile];

            blockedTile = ScriptableObject.CreateInstance<MapTile>();
            blockedTile.isBlocked = true;
            blockedTile.m_Sprite = Tiles[(int)TileTypes.BlockedTile].GetComponent<SpriteRenderer>().sprite;
            blockedTile.m_Prefab = Tiles[(int)TileTypes.BlockedTile];

            InitMap(grassTile);
        }

        private void InitMap(MapTile tile)
        {
            for (int y = -MAP_SIZE; y < MAP_SIZE; y++)
            {
                for (int x = -MAP_SIZE; x < MAP_SIZE; x++)
                {
                    if(y == -MAP_SIZE || x == -MAP_SIZE || y == MAP_SIZE-1 || x == MAP_SIZE - 1)
                        map[0].SetTile(new Vector3Int(x, y, 0), blockedTile);
                    else
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
                        //TEMP!
                        map[0].SetTile(new Vector3Int((int)(xCenter - sizeX/2) + x, (int)(yCenter - sizeY/2) + y, 0), blockedTile);

                        MapTile currTile = (MapTile)map[0].GetTile(new Vector3Int((int)(xCenter - sizeX / 2) + x, (int)(yCenter - sizeY / 2) + y, 0));
                        if (currTile != null)
                        {
                            currTile.isBlocked = true;
                        }
                    }
                }
            }
        }

        public bool IsPlacable(int xCenter, int yCenter, bool[,] tileArray)
        {
            float sizeX = tileArray.GetLength(0);
            float sizeY = tileArray.GetLength(1);

            for (int y = 0; y < sizeY; y++)
            {
                for (int x = 0; x < sizeX; x++)
                {
                    if (tileArray[x, y])
                    {
                        MapTile currTile = (MapTile)map[0].GetTile(new Vector3Int((int)(xCenter - sizeX / 2) + x, (int)(yCenter - sizeY / 2) + y, 0));
                        if (currTile != null && currTile.isBlocked)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
    }
}
