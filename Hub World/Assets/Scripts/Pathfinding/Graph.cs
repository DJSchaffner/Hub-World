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
            public bool IsBlocked     { get; set; }
            public bool IsCompleted   { get; set; }
            public float Heuristic  { get; set; }

            public Cell(bool isBlocked, float heuristic) {
                this.IsBlocked = isBlocked;
                this.IsCompleted = false;
                this.Heuristic = heuristic;
            }
        }

        public int Width        { get; set; }
        public int Height       { get; set; }
        public Vector3Int Start { get; set; }
        public Vector3Int End   { get; set; }
        private Cell[,] cells    { get; set; }

        public Graph(Tilemap map, Vector3Int start, Vector3Int end) {
            this.Width = MapController.MAP_SIZE;
            this.Height = MapController.MAP_SIZE;
            this.Start = start;
            this.End = end;

            cells = new Cell[Height, Width];

            for (int y = 0; y < Height; y++) {
                for (int x = 0; x < Width; x++) {
                    cells[y, x] = new Cell(((MapTile) map.GetTile(new Vector3Int(x, y, 0))).isBlocked, Math.Abs(end.x - x) + Math.Abs(end.y - y));
                }
            }
        }

        public Cell GetCell(Vector3Int vec) {
            return cells[vec.y, vec.x];
        }

        public bool IsInbounds(Vector3Int position) {
            return  position.x >= 0 && position.x < this.Width && 
                    position.y >= 0 && position.y < this.Height;
        }
    }   
}