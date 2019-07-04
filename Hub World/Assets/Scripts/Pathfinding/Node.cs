using UnityEngine;
using System;

namespace Pathfinding
{
  public class Node : IComparable
    {
        public Vector3Int Position { get; set; }
        public Vector3Int Previous { get; set; }
        public float Traveled      { get; set; }
        public float Heuristic     { get; set; }

        /// <summary>
        /// Node constructor
        /// </summary>
        /// <param name="position"></param>
        /// <param name="previous"></param>
        /// <param name="traveled"></param>
        /// <param name="heuristic"></param>
        public Node (Vector3Int position, Vector3Int previous, float traveled, float heuristic) {
            this.Position = position;
            this.Previous = previous;
            this.Traveled = traveled;
            this.Heuristic = heuristic;
        }

        /// <summary>
        /// Returns the total distance for this node
        /// </summary>
        /// <returns></returns>
        public float GetTotal() {
            return this.Traveled + this.Heuristic;
        }

        /// <summary>
        /// Comparator
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(object obj) {
            if (obj == null)
                return 1;

            Node other = obj as Node;

            return this.GetTotal().CompareTo(other.GetTotal());
        }
    }
}