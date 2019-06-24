using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour
{
    public bool[,] BuildArea;
    public PolygonCollider2D PolCollider;

    private SpriteRenderer render;

    // Start is called before the first frame update
    void Awake()
    {
        render = GetComponent<SpriteRenderer>();
        PolCollider = GetComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Wenn der linke Mausbutton auf diesem Object geklickt wurde
    void OnMouseDown()
    {
        InteractWith();
    }

    public void SetSprite(Sprite sprite)
    {
        render.sprite = sprite;
        BuildArea = DefineBuildingArea(PolCollider);
    }

    private void InteractWith()
    {
        Debug.Log("I'm opening my Menues!!");
    }

    private bool[,] DefineBuildingArea(PolygonCollider2D placedObject)
    {
        float sizeX = placedObject.bounds.extents.x;
        float sizeY = placedObject.bounds.extents.y;
        bool[,] tileMap = new bool[(int)sizeX, (int)sizeY];

        float offset = GameController.TILE_SIZE / 2;
        Vector3 position = transform.position;

        //Geht über die Koordinaten im Grid, welche innerhalb der Bounds des zu platzierenden Objekts sind 
        for (int y = (int)(position.y - sizeY); y < (position.y + sizeY); y++)
        {
            for (int x = (int)(position.x - sizeX); x < (position.x + sizeX); x++)
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
                            tileMap[x, y] = true;

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
                                        tileMap[roundedCellY.x, roundedCellY.y] = true;

                                    }
                                    if (vagueCellPos.x % 1 != 0)
                                    {
                                        vagueCellPos.x = (int)vagueCellPos.x;
                                        roundedCellX.x = (int)vagueCellPos.x + xOffset;
                                        roundedCellX.y = (int)vagueCellPos.y;
                                        tileMap[roundedCellX.x, roundedCellX.y] = true;
                                    }

                                    Vector3Int pathCellPos = new Vector3Int((int)vagueCellPos.x, (int)vagueCellPos.y, 0);
                                    tileMap[pathCellPos.x, pathCellPos.y] = true;
                                }
                            }
                        }
                    }
                }
            }
        }
        return tileMap;
    }
}
