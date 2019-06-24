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


        public void PlaceObject(int xCenter, int yCenter, PolygonCollider2D placedObject)
        {
            float sizeX = placedObject.bounds.extents.x;
            float sizeY = placedObject.bounds.extents.y;
            float offset = grid.cellSize.x / 2;

            //Geht über die Koordinaten im Grid, welche innerhalb der Bounds des zu platzierenden Objekts sind 
            for (int y = (int)(yCenter - sizeY); y < (yCenter + sizeY); y++)
            {
                for (int x = (int)(xCenter - sizeX); x < (xCenter + sizeX); x++)
                {
                    //Aktuelle Grid-Position
                    Vector3Int cellPos = new Vector3Int(x, y, 0);

                    //Für jeden Path des Polygon-Colliders
                    for (int i = 0; i < placedObject.pathCount; i++)
                    {
                        //Für jeden Punkt eines Paths des Polygon-Colliders
                        for (int j = 0; j < placedObject.GetPath(i).Length; j++)
                        {
                            //Holt sich den aktuellen Punkt und wandelt ihn in Welt-Koords um
                            Vector2 pathPoint = placedObject.GetPath(i)[j];
                            Vector2 point = placedObject.transform.TransformPoint(pathPoint);

                            //Wenn der Punkt sich innerhalb der derzeit betrachteten Grid-Zelle befindet
                            if (point.x <= cellPos.x + offset && point.x >= cellPos.x - offset
                                && point.y <= cellPos.y + offset && point.y >= cellPos.y - offset)
                            {
                                //Wird dort ein neues Blocker-Tile hingesetzt
                                map[0].SetTile(new Vector3Int(x, y, 0), blockedTile);
                                //Ist die Distanz zum nächsten Punkt des derzeit betrachteten Paths größer als zwei Grid-Zellen
                                if (j+1 < placedObject.GetPath(i).Length && Vector2.Distance(pathPoint, placedObject.GetPath(i)[j+1]) >= (grid.cellSize.x))
                                {
                                    //Wird die Distanz zwischen dem aktuellen Punkt und dem nächsten Punkt berechnet
                                    float distance = Vector2.Distance(pathPoint, placedObject.GetPath(i)[j + 1]);

                                    //Abhängig von dieser wird (distance) mal die Position für eine Zelle zwischen beiden Punkten ermittelt
                                    for (float k = 1/distance; k <= 1; k += 1/distance)
                                    {
                                        //Gibt die nächste Position zwischen aktuellem Punkt und nächstem Punkt, abhängig von (k)
                                        Vector2 lerp = Vector2.Lerp(pathPoint, placedObject.GetPath(i)[j + 1], k);
                                        Vector2 worldLerp = placedObject.transform.TransformPoint(lerp);
                                        
                                        //Berechnet den Richtungs-Vektor zwischen dem aktuellen Punkt und der neuen Position
                                        Vector2 newPointDirection = worldLerp - point;
                                        //Berechnet die ungefähre Grid.Position
                                        Vector2 vagueCellPos = new Vector2(point.x + (newPointDirection.x * k), point.y + (newPointDirection.y * k));

                                        Vector3Int roundedCellX = new Vector3Int(0, 0, 0);
                                        Vector3Int roundedCellY = new Vector3Int(0, 0, 0);
                                        int xOffset = 1, yOffset = 1;
                                        if (newPointDirection.x < 0)
                                            xOffset = -1;
                                        if (newPointDirection.y < 0)
                                            yOffset = -1;

                                        //Rundet das Ergebnis
                                        if (vagueCellPos.y % 1 != 0)
                                        {
                                            vagueCellPos.y = (int)vagueCellPos.y;
                                            roundedCellY.y = (int)vagueCellPos.y + yOffset;
                                            roundedCellY.x = (int)vagueCellPos.x;
                                            BlockTile(roundedCellY, cellPos);
                                        }
                                        if (vagueCellPos.x % 1 != 0)
                                        {
                                            vagueCellPos.x = (int)vagueCellPos.x;
                                            roundedCellX.x = (int)vagueCellPos.x + xOffset;
                                            roundedCellX.y = (int)vagueCellPos.y;
                                            BlockTile(roundedCellX, cellPos);
                                        }
                                        
                                        Vector3Int pathCellPos = new Vector3Int((int)vagueCellPos.x, (int)vagueCellPos.y , 0);

                                        BlockTile(pathCellPos, cellPos);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void BlockTile(Vector3Int pathCellPos, Vector3Int cellPos)
        {
            TileBase tile = map[0].GetTile(new Vector3Int(pathCellPos.x, pathCellPos.y, 0));
            if (pathCellPos != cellPos && tile == null)
            {
                map[0].SetTile(new Vector3Int(pathCellPos.x, pathCellPos.y, 0), blockedTile);
            }
        }


        public void PlaceObject(int xCenter, int yCenter, bool[,] tileArray)
        {
            float sizeX = tileArray.GetLength(0) / 2;
            float sizeY = tileArray.GetLength(1) / 2;

            for (int y = 0; y < sizeY ; y++)
            {
                for (int x = 0; x < sizeX; x++)
                {
                    if (tileArray[x, y])
                    {
                        map[0].SetTile(new Vector3Int((int)(xCenter - sizeX) + x, (int)(yCenter - sizeY) + y, 0), blockedTile);
                    }
                }
            }
        }
    }
}
