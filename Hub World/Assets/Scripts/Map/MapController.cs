using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum TileTypes { BlockedTile = 0, GrassTile}

namespace Map{
    public class MapController : MonoBehaviour
    {
        private const int MAP_SIZE = 50;
        private const int BORDER_WIDTH = 3;

        public GameObject[] Tiles;

        private Tilemap[] map;
        private Grid grid;
        private MapTile blockedTile;
        private int totalMapSize;

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

        public Tilemap[] GetMap()
        {
            return map;
        }

        private void InitMap(MapTile tile)
        {
            totalMapSize = (MAP_SIZE + BORDER_WIDTH);
            for (int y = -totalMapSize; y < totalMapSize; y++)
            {
                for (int x = -totalMapSize; x < totalMapSize; x++)
                {
                    if(y <= -MAP_SIZE || x <= -MAP_SIZE || y >= MAP_SIZE-1 || x >= MAP_SIZE - 1)
                        map[0].SetTile(new Vector3Int(x, y, 0), blockedTile);
                    else
                        map[0].SetTile(new Vector3Int(x, y, 0), tile);
                }
            }
        }

        public bool InBounds(Vector2 position, Vector2 camSize)
        {
            float boundsXRight = transform.position.x + totalMapSize;
            float boundsXLeft = transform.position.x - totalMapSize;
            float boundsYUp = transform.position.y + totalMapSize;
            float boundsYDown = transform.position.y - totalMapSize;

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
            float boundsXRight = transform.position.x + totalMapSize;
            float boundsXLeft = transform.position.x - totalMapSize;
            float boundsYUp = transform.position.y + totalMapSize;
            float boundsYDown = transform.position.y - totalMapSize;

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
