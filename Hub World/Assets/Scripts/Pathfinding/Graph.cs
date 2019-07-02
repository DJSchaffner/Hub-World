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

            /// <summary>
            /// Cell constructor
            /// </summary>
            /// <param name="isBlocked"></param>
            /// <param name="heuristic"></param>
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

        /// <summary>
        /// Graph constructor
        /// </summary>
        /// <param name="map"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
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

        /// <summary>
        /// Returns a cell at a given position
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public Cell GetCell(Vector3Int position) {
            return cells[position.y, position.x];
        }

        /// <summary>
        /// Checks if a given position is in the graph boundaries
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public bool IsInbounds(Vector3Int position) {
            return  position.x >= 0 && position.x < this.Width && 
                    position.y >= 0 && position.y < this.Height;
        }
    }   
}