using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Map{

    //Unterschiedliche Tile-Typen
    public enum TileTypes { BlockedTile = 0, GrassTile }

    /**
    * Map-Controller Komponente
    * @author cgt102461: Nicolas Begic
    */
    public class MapController : MonoBehaviour
    {
        //Anzahl der Tiles in einer Reihe, die zu Beginn gefüllt werden und die Größe der Spielwelt darstellen
        public const int MAP_SIZE = 100;

        //Konstanten für die Spiel-Karte und die Abenteurer-Spawnpoints
        private const int BORDER_WIDTH = 3;
        private const int HALF_WAY_SIZE = 8;
        private const int SPAWN_AREAS = 2;

        //Array aller unterschiedlicher Tile-Prefabs
        public GameObject[] Tiles;
        //Array aller Spawnpoints für Abenteurer 
        //(Dim1: Welche Seite? = Links/Rechts; Dim2: Welcher Punkt auf dieser Seite? = 0-WAY_SIZE)
        public Vector2Int[,] SpawnPoints { get; set; }

        //Alle Objekte bezüglich der Spiel-Karte
        private Tilemap[] map;
        private Grid grid;
        //TEMP: Wird aus Test-Zwecken angezeigt wenn Tile blockiert ist. 
        private MapTile blockedTile;
        private int halfMapSize;

        //Wird vor Start bei Entstehung des Map-Objekts aufgerufen
        void Awake()
        {
            map = GetComponentsInChildren<Tilemap>();
            grid = GetComponentInParent<Grid>();

            //Initialisierung aller Tile-Typen
            //TODO: Muss in Schleife umgewandelt werden
            MapTile grassTile = ScriptableObject.CreateInstance<MapTile>();
            grassTile.m_Sprite = Tiles[(int)TileTypes.GrassTile].GetComponent<SpriteRenderer>().sprite;
            grassTile.m_Prefab = Tiles[(int)TileTypes.GrassTile];

            blockedTile = ScriptableObject.CreateInstance<MapTile>();
            blockedTile.isBlocked = true;
            blockedTile.m_Sprite = Tiles[(int)TileTypes.BlockedTile].GetComponent<SpriteRenderer>().sprite;
            blockedTile.m_Prefab = Tiles[(int)TileTypes.BlockedTile];

            //Füllt die Karte
            InitMap(grassTile);
        }

        /**
         * Gibt eine Referenz auf die Tilemap der Spielkarte zurück 
         * return: Tilemap-ref
         */
        public Tilemap[] GetMap()
        {
            return map;
        }

        /**
         * Füllt die Tilemap mit einem übergebenen Tile
         * Zeichnet außerdem eine Border um die globalen Dimensionen
         * der gewünschten Karten-Größe.
         * 
         * Erschafft an ihrer linken und rechten Seite auf mittlerer Höhe
         * Spawn-Punkte für die Abenteurer.
         * 
         * param: tile Tile, mit dem die Karte gefüllt werden soll
         */
        private void InitMap(MapTile tile)
        {
            int totalMapSize = MAP_SIZE + BORDER_WIDTH;
            //Füllt die Karte und erschafft eine Border um sie herum
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
            //Erzeugt die Spawnpoints der Abenteurer
            int i = 0;
            for (int y = (halfMapSize - HALF_WAY_SIZE); y < (halfMapSize + HALF_WAY_SIZE); y++)
            {
                SpawnPoints[0, i] = new Vector2Int(BORDER_WIDTH, y);
                SpawnPoints[1, i] = new Vector2Int(MAP_SIZE - 1, y);
                i++;
            }
        }

        /**
         * Gibt einen Bool zurück,
         * welcher bestimmt ob sich die übergebene Position der Kamera
         * innerhalb der Karte befindet, bzw. die Kamera mit ihrer Größe
         * aus der Karte wandert.
         * 
         * param: position Position der Kamera
         * param: camSize Breite(x) und Höhe(y) der Kamera
         */
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

        /**
         * Gibt eine Position zurück, die die Kamera zurück auf die Karte bringt.
         * 
         * param: position Position der Kamera
         * param: camSize Breite(x) und Höhe(y) der Kamera
         */
        public Vector3 GetBackInBounds(Vector3 position, Vector2 camSize)
        {
            Vector3 newPos = new Vector3(0, 0, position.z);
            float boundsXRight = MAP_SIZE + BORDER_WIDTH;
            float boundsXLeft = 0;
            float boundsYUp = MAP_SIZE + BORDER_WIDTH;
            float boundsYDown = 0;

            float halfCamSizeX = (camSize.x / 2);
            float halfCamSizeY = (camSize.y / 2);

            //Berechnet die neue x-Position der Kamera
            if (position.x + halfCamSizeX > boundsXRight)
                newPos.x = position.x - (position.x + halfCamSizeX - boundsXRight);
            else if (position.x - halfCamSizeX < boundsXLeft)
                newPos.x = position.x + (halfCamSizeX - (position.x - boundsXLeft));
            else
                newPos.x = position.x;

            //Berechnet die neue y-Position der Kamera
            if (position.y + halfCamSizeY > boundsYUp)
                newPos.y = position.y - (position.y + halfCamSizeY - boundsYUp);
            else if (position.y - halfCamSizeY < boundsYDown)
                newPos.y = position.y + (halfCamSizeY - (position.y - boundsYDown));
            else
                newPos.y = position.y;

            return newPos;
        }

        /**
         * Füllt die Tilemap mit BlockedTiles an den Stellen
         * des übergebenen tileArrays, die mit true gefüllt sind.
         * 
         * param: xCenter x-Koord des MittelPunkt des Objektes auf der Tilemap
         * param: xCenter y-Koord des MittelPunkt des Objektes auf der Tilemap
         * param: tileArray Bool-Array in der Größe des zu platzierenden Objektes,
         *                  welches die Tiles mit true markiert hat,
         *                  die sich auf dem Gebäude befinden.
         */
        public void PlaceObject(int xCenter, int yCenter, bool[,] tileArray)
        {
            float sizeX = tileArray.GetLength(0);
            float sizeY = tileArray.GetLength(1);

            //Geht über das TileArray
            for (int y = 0; y < sizeY ; y++)
            {
                for (int x = 0; x < sizeX; x++)
                {
                    //Wenn die Position mit true markiert ist
                    if (tileArray[x, y])
                    {
                        //TEMP: Tile wird zu einem BlockedTile gemacht für Test-Zwecke
                        map[0].SetTile(new Vector3Int((int)(xCenter - sizeX/2) + x, (int)(yCenter - sizeY/2) + y, 0), blockedTile);

                        //Wird der isBlocked-Bool des sich dort auf der Tilemap befindenden Tiles auf true gesetzt
                        MapTile currTile = (MapTile)map[0].GetTile(new Vector3Int((int)(xCenter - sizeX / 2) + x, (int)(yCenter - sizeY / 2) + y, 0));
                        if (currTile != null)
                        {
                            currTile.isBlocked = true;
                        }
                    }
                }
            }
        }

        /**
         * Gibt einen Bool zurück, ob das übergebene TileArray
         * sich mit ihren true gesetzten bools mit auf der Tilemap auf isBlocked=true
         * gesetzten Tiles überschneidet.
         * 
         * param: xCenter x-Koord des MittelPunkt des Objektes auf der Tilemap
         * param: xCenter y-Koord des MittelPunkt des Objektes auf der Tilemap
         * param: tileArray Bool-Array in der Größe des zu platzierenden Objektes,
         *                  welches die Tiles mit true markiert hat,
         *                  die sich auf dem Gebäude befinden.
         */
        public bool IsPlacable(int xCenter, int yCenter, bool[,] tileArray)
        {
            float sizeX = tileArray.GetLength(0);
            float sizeY = tileArray.GetLength(1);

            //Geht über das TileArray
            for (int y = 0; y < sizeY; y++)
            {
                for (int x = 0; x < sizeX; x++)
                {
                    //Wenn die Position mit true markiert ist
                    if (tileArray[x, y])
                    {
                        MapTile currTile = (MapTile)map[0].GetTile(new Vector3Int((int)(xCenter - sizeX / 2) + x, (int)(yCenter - sizeY / 2) + y, 0));
                        //Und das Tile auf der TileMap ihren isBlocked-value ebenfalls auf true gesetzt hat
                        //wird false returned.
                        if (currTile != null && currTile.isBlocked)
                            return false;
                    }
                }
            }
            return true;
        }
    }
}
