using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using Map;

namespace Pathfinding
{
  public class Graph
    {
        public class Cell
        {
            public bool blocked     { get; set; }
            public bool completed   { get; set; }
            public float heuristic  { get; set; }

            public Cell(bool blocked, float heuristic) {
                this.blocked = blocked;
                this.completed = false;
                this.heuristic = heuristic;
            }
        }

        public int width        { get; set; }
        public int height       { get; set; }
        public Vector3Int start { get; set; }
        public Vector3Int end   { get; set; }
        public Cell[,] cells    { get; set; }

        public Graph(Tilemap map, Vector3Int start, Vector3Int end) {
            this.width = map.size.x;
            this.height = map.size.y;
            this.start = start;
            this.end = end;
            
            cells = new Cell[height, width];

            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    cells[y, x] = new Cell(((MapTile) map.GetTile(new Vector3Int(x, y, 0))).isBlocked, Math.Abs(end.x - x) + Math.Abs(end.y - y));
                }
            }
        }

        public Cell GetCell(Vector3Int vec) {
            return cells[vec.y, vec.x];
        }

        public bool IsInbounds(Vector3Int position) {
            return  position.x >= 0 && position.x < this.width && 
                    position.y >= 0 && position.y < this.height;
        }
    }   
}