using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour
{
    public bool[,] BuildArea { get; set; }

    private PolygonCollider2D polCollider;
    private SpriteRenderer render;

    // Awake is called before the first frame update and Start
    void Awake()
    {
        render = GetComponent<SpriteRenderer>();
    }

    // Wenn der linke Mausbutton auf diesem Object geklickt wurde
    void OnMouseDown()
    {
        InteractWith();
    }

    public void SetBuildingType(Sprite sprite)
    {
        render.sprite = sprite;
        if (polCollider != null)
        {
            Destroy(polCollider);
        }
        polCollider = gameObject.AddComponent<PolygonCollider2D>();
        BuildArea = DefineBuildingArea(polCollider);
        BuildArea = FillBuildingArea(BuildArea);
    }

    private void InteractWith()
    {
        Debug.Log("I'm opening my Menues!!");
    }

    private bool[,] DefineBuildingArea(PolygonCollider2D placedObject)
    {
        float sizeX = placedObject.bounds.size.x;
        float sizeY = placedObject.bounds.size.y;
        bool[,] tileMap = new bool[(int)sizeX +1, (int)sizeY + 1];

        float offset = GameController.TILE_SIZE / 2;
        Vector3 position = transform.position;

        int indexOffsetX = (int)(position.x - sizeX/2);
        int indexOffsetY = (int)(position.y - sizeY/2);

        //Geht über die Koordinaten im Grid, welche innerhalb der Bounds des zu platzierenden Objekts sind 
        for (int y = indexOffsetY; y < (position.y + sizeY/2); y++)
        {
            for (int x = indexOffsetX; x < (position.x + sizeX/2); x++)
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
                            //FUNZT NICHT! Muss noch zwei lauf variablen mitziehen
                            setTile(x, y, indexOffsetX, indexOffsetY, tileMap);

                            //Ist die Distanz zum nächsten Punkt des derzeit betrachteten Paths größer als zwei Grid-Zellen
                            if (j + 1 < placedObject.GetPath(i).Length && Vector2.Distance(pathPoint, placedObject.GetPath(i)[j + 1]) >= GameController.TILE_SIZE)
                            {
                                //Wird die Distanz zwischen dem aktuellen Punkt und dem nächsten Punkt berechnet
                                float distance = Vector2.Distance(pathPoint, placedObject.GetPath(i)[j + 1]);

                                //Abhängig von dieser wird (distance) mal die Position für eine Zelle zwischen beiden Punkten ermittelt
                                for (float k = 1 / distance; k <= 1; k += 1 / distance)
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
                                        setTile(roundedCellY.x, roundedCellY.y, indexOffsetX, indexOffsetY, tileMap);
                                    }
                                    if (vagueCellPos.x % 1 != 0)
                                    {
                                        vagueCellPos.x = (int)vagueCellPos.x;
                                        roundedCellX.x = (int)vagueCellPos.x + xOffset;
                                        roundedCellX.y = (int)vagueCellPos.y;
                                        setTile(roundedCellX.x, roundedCellX.y, indexOffsetX, indexOffsetY, tileMap);
                                    }

                                    Vector3Int pathCellPos = new Vector3Int((int)vagueCellPos.x, (int)vagueCellPos.y, 0);
                                    setTile(pathCellPos.x, pathCellPos.y, indexOffsetX, indexOffsetY, tileMap);
                                }
                            }
                        }
                    }
                }
            }
        }
        return tileMap;
    }


    private void setTile(int x, int y, int indexOffsetX, int indexOffsetY, bool[,] tileMap)
    {
        int indexX = x - indexOffsetX, indexY = y - indexOffsetY;
        if(indexX < 0)
            indexX = 0;
        else if (indexX > tileMap.GetLength(0))
            indexX = tileMap.GetLength(0);

        if(indexY < 0)
            indexY = 0;
        else if (indexY > tileMap.GetLength(1))
            indexY = tileMap.GetLength(1);

        tileMap[indexX, indexY] = true;
    }

    private bool[,] FillBuildingArea(bool[,] tileMap)
    {
        bool fillLine;
        int fillToX = 0, fillFromX = 0;

        for (int y = 0; y < tileMap.GetLength(1); y++)
        {
            fillLine = false;
            for (int x = 0; x < tileMap.GetLength(0); x++)
            {
                if (!fillLine && tileMap[x, y])
                {
                    fillLine = true;
                    fillFromX = x;
                }
                else if (fillLine && tileMap[x, y])
                {
                    fillToX = x;
                }
            }

            for (int x = fillFromX; x < fillToX; x++)
            {
                tileMap[x, y] = true;
            }
        }
        return tileMap;
    }
}
