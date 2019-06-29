using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Map{

    public enum TileTypes { BlockedTile = 0, GrassTile }

    public class MapController : MonoBehaviour
    {
        public const int MAP_SIZE = 100;

        private const int BORDER_WIDTH = 3;
        private const int HALF_WAY_SIZE = 8;
        private const int SPAWN_AREAS = 2;

        public GameObject[] Tiles;
        public Vector2Int[,] SpawnPoints { get; set; }

        private Tilemap[] map;
        private Grid grid;
        private MapTile blockedTile;
        private int halfMapSize;

        void Awake()
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

        public Tilemap[] GetMap()
        {
            return map;
        }

        private void InitMap(MapTile tile)
        {
            int totalMapSize = MAP_SIZE + BORDER_WIDTH;
            for (int y = 0; y < totalMapSize; y++)
            {
                for (int x = 0; x < totalMapSize; x++)
                {
                    if(y < BORDER_WIDTH || x < BORDER_WIDTH || y > MAP_SIZE -1 || x > MAP_SIZE -1)
                        map[0].SetTile(new Vector3Int(x, y, 0), blockedTile);
                    else
                        map[0].SetTile(new Vector3Int(x, y, 0), tile);
                }
            }

            SpawnPoints = new Vector2Int[SPAWN_AREAS, HALF_WAY_SIZE*2];
            halfMapSize = ((MAP_SIZE / 2) + BORDER_WIDTH);

            int i = 0;
            for (int y = (halfMapSize - HALF_WAY_SIZE); y < (halfMapSize + HALF_WAY_SIZE); y++)
            {
                SpawnPoints[0, i] = new Vector2Int(BORDER_WIDTH, y);
                SpawnPoints[1, i] = new Vector2Int(MAP_SIZE - 1, y);
                i++;
            }
        }

        public bool InBounds(Vector2 position, Vector2 camSize)
        {
            float boundsXRight = MAP_SIZE + BORDER_WIDTH;
            float boundsXLeft = 0;
            float boundsYUp = MAP_SIZE + BORDER_WIDTH;
            float boundsYDown = 0;

            if (position.x + camSize.x/2 > boundsXRight
                || position.x - camSize.x/2 < boundsXLeft
                || position.y + camSize.y/2 > boundsYUp
                || position.y - camSize.y/2 < boundsYDown)
            {
                return false;
            }else
                return true;
        }

        public Vector3 GetBackInBounds(Vector3 position, Vector2 camSize)
        {
            Vector3 newPos = new Vector3(0, 0, position.z);
            float boundsXRight = MAP_SIZE + BORDER_WIDTH;
            float boundsXLeft = 0;
            float boundsYUp = MAP_SIZE + BORDER_WIDTH;
            float boundsYDown = 0;

            float halfCamSizeX = (camSize.x / 2);
            float halfCamSizeY = (camSize.y / 2);

            if (position.x + halfCamSizeX > boundsXRight)
                newPos.x = position.x - (position.x + halfCamSizeX - boundsXRight);
            else if (position.x - halfCamSizeX < boundsXLeft)
                newPos.x = position.x + (halfCamSizeX - (position.x - boundsXLeft));
            else
                newPos.x = position.x;

            if (position.y + halfCamSizeY > boundsYUp)
                newPos.y = position.y - (position.y + halfCamSizeY - boundsYUp);
            else if (position.y - halfCamSizeY < boundsYDown)
                newPos.y = position.y + (halfCamSizeY - (position.y - boundsYDown));
            else
                newPos.y = position.y;

            return newPos;
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
