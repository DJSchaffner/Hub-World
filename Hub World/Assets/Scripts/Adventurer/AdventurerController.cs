using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;
using Pathfinding;

public class AdventurerController : MonoBehaviour
{
    private const float MOVE_SPEED = 1f;

    private Adventurer needs;
    private AStar pathFinding;
    private List<Vector3Int> newPath;
    private bool hasPath;


    void Start()
    {
        needs = new Adventurer();

        pathFinding = new AStar();
        hasPath = false;
    }

    void Update()
    {
        if (hasPath)
            Move();
    }

    public void GetPath(Tilemap map, Vector3Int target)
    {
        newPath = pathFinding.FindPath(map, new Vector3Int((int)transform.position.x, (int)transform.position.y, 0), target);
        hasPath = true;
    }

    private void Move()
    {
        if (newPath.Count > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, newPath[0], MOVE_SPEED);
            Vector3Int cellPos = new Vector3Int((int)transform.position.x, (int)transform.position.y, 0);
            if (cellPos == newPath[0])
                newPath.Remove(newPath[0]);
        }
        else
            hasPath = false;
    }
}
