using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;
using Pathfinding;

/**
 * Abenteurer-Controller Komponente
 * @author cgt102461: Nicolas Begic
 */
public class AdventurerController : MonoBehaviour
{
    //Bewegungs-Geschwindigkeit eines Abenteurers
    private const float MOVE_SPEED = 0.2f;
    //Offset, den ein Abenteurer von seiner gewünschten Position entfernt sein darf
    private const float OFFSET = 0.1f;

    //Eigenschaften des Abenteurers
    private Adventurer needs;
    //Pathfinding Componente
    private AStar pathFinding;
    //Neues Ziel des Abenteurers
    private List<Vector3Int> newPath;
    private bool hasPath;

    // Start is called before the first frame update
    void Start()
    {
        needs = new Adventurer();

        pathFinding = new AStar();
        hasPath = false;
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (hasPath)
            Move();
    }

    /**
     * Bekommt ein neues Ziel auf der übergebenen TileMap übergeben
     * und setzt diesen als sein neues Ziel.
     * 
     * param: map TileMap
     * param: target Neues Ziel
     */
    public void StartPath(Tilemap map, Vector3Int target)
    {
        newPath = pathFinding.FindPath(map, new Vector3Int((int)transform.position.x, (int)transform.position.y, 0), target);

        if (newPath != null)
            hasPath = true;
    }

    /**
     * Wenn der Abenteurer ein Ziel hat, läuft er seinen Pfad nacheinander ab,
     * bis dieser komplett abgelaufen wurde.
     */
    private void Move()
    {
        if (newPath.Count > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, newPath[0], MOVE_SPEED);
            Vector3Int cellPos = new Vector3Int((int)transform.position.x, (int)transform.position.y, 0);
            if (cellPos.x <= newPath[0].x + OFFSET && cellPos.x >= newPath[0].x - OFFSET
                && cellPos.y <= newPath[0].y + OFFSET && cellPos.y >= newPath[0].y - OFFSET)
                newPath.Remove(newPath[0]);
        }
        else
            hasPath = false;
    }
}
