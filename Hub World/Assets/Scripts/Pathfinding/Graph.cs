using UnityEngine;
using UnityEngine.Tilemaps;
using System;

namespace Pathfinding
{
  public class Graph
    {
        public class Cell
        {
            public bool completed { get; set; }
            public float heuristic { get; set; }

            public Cell(float heuristic) {
                this.completed = false;
                this.heuristic = heuristic;
            }
        }

        public int width        { get; set; }
        public int height       { get; set; }
        public Vector3Int start { get; set; }
        public Vector3Int end   { get; set; }
        public Cell[,] cells   { get; set; }

        public Graph(Tilemap map, Vector3Int start, Vector3Int end) {
            this.width = map.size.x;
            this.height = map.size.y;
            this.start = start;
            this.end = end;
            
            cells = new Cell[height, width];

            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    cells[y, x] = new Cell(Math.Abs(end.x - x) + Math.Abs(end.y - y));
                }
            }
        }
    }   
}